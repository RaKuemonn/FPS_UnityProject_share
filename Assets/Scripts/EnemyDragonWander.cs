using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonWander : BaseEnemy
{
    private Animator m_animator;

    private float m_idleTimer;
    [SerializeField] private float m_idleTimeMax;
    [SerializeField] private float m_moveSpeed;

    public enum State
    {
        Wander,
        Idle,
        Death,
    };
    private State state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_territoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
         case State.Wander: ConditionWanderUpdate(); break;
         case State.Idle: ConditionIdleUpdate(); break;
         case State.Death: ConditionDeathUpdate(); break;
        };
    }

    // 徘徊
    private void ConditionWanderState()
    {
        state = State.Wander;

        SetRandamTargetPosition();

        m_idleTimer = 0.0f;
    }

    private void ConditionWanderUpdate()
    {
        m_animator.SetFloat("MoveSpeed", 0.7f);


        var dir = m_targetPosition - transform.position;
        dir.Normalize();
        dir *= m_moveSpeed * Time.deltaTime;
        if (Turn(dir)) transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_targetPosition - transform.position;

        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.2f * 0.2f) ConditionIdleState();
    }

    // 待機
    private void ConditionIdleState()
    {
        state = State.Idle;
        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        m_animator.SetFloat("MoveSpeed", 0.0f);

        if (m_idleTimer > m_idleTimeMax) ConditionWanderState();

        m_idleTimer += Time.deltaTime;
    }

    // 死亡
    private void ConditionDeathState()
    {
        state = State.Death;
        m_animator.SetBool("Death", IsDeath);
    }

    private void ConditionDeathUpdate()
    {
    }
}
