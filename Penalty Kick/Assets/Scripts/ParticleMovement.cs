using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class ParticleMovement : MonoBehaviour
{
    //kick info variables
    
    private bool m_bAiming = true;
    private Vector3 m_vDirection = Vector3.zero;
    private Vector3 m_vPrevDir = Vector3.zero;
    private float m_fKickStrength = 0;
    [SerializeField]
    private Vector3 m_vStartingPos = Vector3.zero ;

    //UI variables
    [SerializeField]
    private GameObject m_arrow = null;
    [SerializeField]
    private UIManager m_interface = null;
    private bool m_bPowerDecreasing = false;

    private int m_iScore = 0;
    private bool m_bJustScored = false;
    private int m_numOfKicks = 0;

    //reset tools
    [SerializeField]
    private bool m_bDebugReset = false;
    public bool m_bWasReset;
    private bool m_bPrepairToReset = false;
    private float m_fResetTimer;


    private Rigidbody m_rb;
    [SerializeField]
    private MySceneManager m_sceneManager = null;

    //getters and setters
    public bool GetIsKicked()
    { return !m_bAiming; }
    public float GetYaw()
    {
        return m_vDirection.x;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(m_interface);
        m_interface.OnRequestUpdateUI(m_vDirection, m_fKickStrength, 0);
        m_rb = GetComponent<Rigidbody>();
        m_rb.sleepThreshold = 10f;  //10f ~ 0.5 m/s
        m_bAiming = true;

        transform.position = m_vStartingPos;
    }

    //set arrow based on mouse and vertical inputs, updates m_vDirection and UI
    void UpdateArrow()
    {
        if (transform.hasChanged)
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X"), 0.0f);
            m_arrow.transform.RotateAround(transform.position, transform.right, -Input.GetAxisRaw("Vertical"));

            m_vDirection = (m_arrow.transform.position - transform.position).normalized;
            if (m_vPrevDir != m_vDirection)
            {
                m_interface.OnRequestUpdateUI(m_vDirection, m_fKickStrength, m_iScore);
                m_vPrevDir = m_vDirection;
            }
        }
    }

    //add impulse force to ball (arrowDir * kickStrength), stop aiming after that
    void kickBall()
    {
        if(m_bAiming)
        {
            m_rb.AddForce(m_vDirection * m_fKickStrength, ForceMode.Impulse);

            m_bAiming = false;

            m_arrow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //call kick ball when space is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            kickBall();
        }

        if(m_bAiming)
        {
            UpdateArrow();

            //raise or lower kick strength while space is held
            if (Input.GetKey(KeyCode.Space))
            {
                if (m_fKickStrength >= 50.0f)
                    m_bPowerDecreasing = true;
                else if(m_fKickStrength <= 0)
                    m_bPowerDecreasing = false;

                if(m_bPowerDecreasing)
                    m_fKickStrength -= 35f * Time.deltaTime;
                else
                    m_fKickStrength += 35f * Time.deltaTime;

                m_interface.UpdateKickStrength(m_fKickStrength);
            }
        }
        else
            checkResetCriteria();
    }
    private void FixedUpdate()
    {
        //update speed while the ball is moving
        if(!m_bAiming)
            m_interface.UpdateSpeed(m_rb.velocity.magnitude);
    }

    //reset everything for the next kick, wasReset will reset the camera position
    private void resetBall()
    {
        m_bWasReset = true;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(m_vStartingPos, Quaternion.Euler(Vector3.zero));
        m_fKickStrength = 0f;
        m_bPowerDecreasing = false;
        m_arrow.SetActive(true);
        m_vDirection = Vector3.zero;
        m_bAiming = true;
        m_bJustScored = false;
        m_interface.OnRequestUpdateUI(m_vDirection, m_fKickStrength, m_iScore);
        m_bPrepairToReset = false;

        m_numOfKicks++;
        //load game over screen
        if (m_numOfKicks >= 10)
        {
            m_sceneManager.LoadScene(false);
        }
    }

    void prepairToReset(float time)
    {
        m_fResetTimer = time;
        m_bPrepairToReset = true;
    }
    void checkResetCriteria()
    {
        if (m_bPrepairToReset)
        {
            m_fResetTimer -= Time.deltaTime;
            if (m_fResetTimer <= 0)
                resetBall();
        }
        //check for reset criteria (wrong direction, scored goal, past net or no velocity)
        else if (m_bDebugReset)
        {
            m_bDebugReset = false;
            resetBall();
        }
        else if(m_bJustScored || transform.position.z >= 30f || m_rb.velocity.z < 0)
        {
            prepairToReset(4f);
            print("reset: Scored, moving backwards or past goal");
        }
        else if (m_rb.IsSleeping() || transform.position.y < -1.0f)
        {
            resetBall();
            print("reset: stopped moving or fell through the floor");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoalZone") && !m_bJustScored)
        {
            m_iScore++;
            m_bJustScored = true;
            m_interface.UpdateScore(m_iScore);
            print("scored");
        }
    }


    private void OnDisable()
    {
        PlayerPrefs.SetInt("Score", m_iScore);
    }
}
