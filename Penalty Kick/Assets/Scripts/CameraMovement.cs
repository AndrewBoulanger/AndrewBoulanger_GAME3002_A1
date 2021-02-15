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
        m_vFurtherOffset = new Vector3(0f, 3f, -10f);
        transform.position = m_ball.transform.position - m_vOffset;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        updateCameraPos();

    }

    void updateCameraPos()
    {
        Assert.IsNotNull(m_ball);
        if (m_ball.GetIsKicked())
        {
            // transform.Rotate(new Vector3(0f,Input.GetAxis("Mouse X"),0f));
            transform.RotateAround(m_ball.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
        }
        else
        {
            if ( Mathf.Abs((m_ball.transform.position - transform.position).sqrMagnitude) >= m_vFurtherOffset.sqrMagnitude)
            {
                Vector3 newPos = m_ball.transform.position + m_vFurtherOffset;
                Vector3 smoothedPos = Vector3.Lerp(transform.position, newPos, 15f * Time.deltaTime);
                transform.position = smoothedPos;
            }
            transform.LookAt(m_ball.transform.position);
        }
    }

}
