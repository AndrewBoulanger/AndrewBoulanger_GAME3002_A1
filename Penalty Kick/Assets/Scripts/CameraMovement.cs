using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private ParticleMovement m_ball = null;

    private Vector3 m_vOffset;
    private Vector3 m_vFurtherOffset;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(m_ball);
        m_vOffset = new Vector3(0f, -3f, 7f);
        m_vFurtherOffset = new Vector3(0f, 3f, -14f);
        transform.position = m_ball.transform.position - m_vOffset;
    }


    // Update is called once per frame
    void Update()
    {
        updateCameraPos();

    }

    void updateCameraPos()
    {
        Assert.IsNotNull(m_ball);
        if (m_ball.GetIsKicked())
        {
          transform.LookAt(m_ball.transform.position);
            Vector3 newPos = m_ball.transform.position + m_vFurtherOffset;
            newPos.y = 6f;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, 0.1f );
            transform.position = smoothedPos;

        }
        else
        {
            transform.RotateAround(m_ball.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
        }

        if(m_ball.m_bWasReset)
        {
            m_ball.m_bWasReset = false;
            transform.SetPositionAndRotation(m_ball.transform.position - m_vOffset, Quaternion.Euler(Vector3.zero));
        }
    }

}
