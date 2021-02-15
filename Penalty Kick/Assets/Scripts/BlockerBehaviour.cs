using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BlockerBehaviour : MonoBehaviour
{
    [SerializeField]
    private ParticleMovement m_ball = null;
    [SerializeField]
    private float m_xRange;

    private float m_fBufferZone = 0.1f;

    private float m_fDelay;

    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_fDelay > 0)
        {
            m_fDelay -= Time.deltaTime;
        }

        if (m_fDelay <= 0)
        {
            Assert.IsNotNull(m_ball);
            if (!m_ball.GetIsKicked())
            {
                float displacement = transform.position.x - m_ball.transform.position.x;
                chooseJumpDir(displacement);
            }
            else
            { 
                float displacement = transform.position.x / m_xRange - m_ball.GetYaw()*2;
                chooseJumpDir(displacement);
            }
        }

    }

    void chooseJumpDir(float displacement)
    {
        if (displacement >= m_fBufferZone)
        {
            jump(-1.5f);
        }
        else if (displacement <= -m_fBufferZone)
        {
            jump(1.5f);
        }
    }

    void jump(float dir)
    {
        m_rb.AddForce(dir, 1.5f, 0, ForceMode.Impulse);
        m_fDelay = 0.2f;
    }
}
