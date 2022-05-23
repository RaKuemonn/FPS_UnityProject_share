using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCubeController : MonoBehaviour
{
    private float m_angleY = 0.0f;
    private float m_oldAngleY = 0.0f;

    private float m_easingTimer = 0.0f;
    private float m_easingTimeMax = 2.0f;

    private bool m_rotateCheck = true;
    public bool GetRotateCheck()
    {
        return m_rotateCheck;
    }

    public enum State
    { 
        Central, // ’†‰›
        Right,    // ‰E
        Left,       // ¶
    }
    private State m_state;
    public void SetState(State state)
    {
        m_state = state;
    }
    public State GetState()
    {
        return m_state;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_state = State.Central;

        // ˆê‰žŠp“x“o˜^
        m_angleY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case State.Central: CentralUpdate(); break;
            case State.Right: RightUpdate(); break;
            case State.Left: LeftUpdate(); break;
        }
    }


    // ’†‰›
    public void CentralSet()
    {
        m_state = State.Central;

        m_oldAngleY = transform.eulerAngles.y;

        m_easingTimer = 0.0f;

        m_rotateCheck = false;
    }

    private void CentralUpdate()
    {
        m_angleY = Easing.SineInOutF(m_easingTimer, m_easingTimeMax, m_oldAngleY, 0);

        m_easingTimer += Time.deltaTime;
        if (m_easingTimer > m_easingTimeMax)
        {
            m_easingTimer = m_easingTimeMax;
            m_rotateCheck = true;
        }

        transform.eulerAngles = new Vector3(0, m_angleY, 0);
    }

    // ‰E‰ñ“]
    public void RightSet()
    {
        m_state = State.Right;

        m_oldAngleY = transform.eulerAngles.y;

        m_easingTimer = 0.0f;

        m_rotateCheck = false;
    }

    private void RightUpdate()
    {
        m_angleY = Easing.SineInOutF(m_easingTimer, m_easingTimeMax, m_oldAngleY, 70);

        m_easingTimer += Time.deltaTime;
        if (m_easingTimer > m_easingTimeMax)
        {
            m_easingTimer = m_easingTimeMax;
            m_rotateCheck = true;
        }

        transform.eulerAngles = new Vector3(0, m_angleY, 0);
    }

    // ‰E‰ñ“]
    public void LeftSet()
    {
        m_state = State.Right;

        m_oldAngleY = transform.eulerAngles.y;

        m_easingTimer = 0.0f;

        m_rotateCheck = false;
    }

    private void LeftUpdate()
    {
        m_angleY = Easing.SineInOutF(m_easingTimer, m_easingTimeMax, m_oldAngleY, -70);

        m_easingTimer += Time.deltaTime;
        if (m_easingTimer > m_easingTimeMax)
        {
            m_easingTimer = m_easingTimeMax;
            m_rotateCheck = true;
        }

        transform.eulerAngles = new Vector3(0, m_angleY, 0);
    }
}
