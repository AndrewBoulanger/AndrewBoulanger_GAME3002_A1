using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour
{
    [SerializeField]
    private bool m_bAiming;
    [SerializeField]
    private Vector3 m_vDirection;
    [SerializeField]
    private float m_fKickStrength = 0;

    private Vector3 m_vPrevDir;
    private bool m_bPowerDecreasing = false;

    private Rigidbody m_rb;
    [SerializeField]
    private GameObject m_arrow;

    private UIManager m_interface = null;

    //getters and setters
    public bool GetIsKicked()
    { return m_bAiming; }
    public float GetYaw()
    {
        return m_vDirection.x;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        m_interface = GetComponent<UIManager>();
        m_interface.OnRequestUpdateUI(m_vDirection, m_fKickStrength, 0);
        m_rb = GetComponent<Rigidbody>();
        m_bAiming = true;

    }

    void UpdateArrow()
    {
        if (transform.hasChanged)
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X"), 0.0f);
            m_arrow.transform.RotateAround(transform.position, transform.right, -Input.GetAxisRaw("Vertical"));

            m_vDirection = (m_arrow.transform.position - transform.position).normalized;
            if (m_vPrevDir != m_vDirection)
            {
                m_interface.OnRequestUpdateUI(m_vDirection, m_fKickStrength, 0);
                m_vPrevDir = m_vDirection;
            }
        }
    }

    void kickBall()
    {
        if(m_bAiming)
        {
            m_bAiming = false;

            m_rb.AddForce((m_arrow.transform.position - transform.position).normalized * m_fKickStrength, ForceMode.Impulse);

            m_arrow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            kickBall();
        }

        if(m_bAiming)
        {
            UpdateArrow();

            if (Input.GetKey(KeyCode.Space))
            {
                if (m_fKickStrength >= 50.0f)
                    m_bPowerDecreasing = true;
                else if(m_fKickStrength <= 0)
                    m_bPowerDecreasing = false;

                if(m_bPowerDecreasing)
                    m_fKickStrength -= 25f * Time.deltaTime;
                else
                    m_fKickStrength += 25f * Time.deltaTime;

                m_interface.UpdateKickStrength(m_fKickStrength);
            }
        }

    }
    private void FixedUpdate()
    {
        if(!m_bAiming)
            m_interface.UpdateSpeed(m_rb.velocity.magnitude);
    }
}
