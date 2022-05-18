using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatStopController : BaseEnemy
{
    [SerializeField]
    private float m_idleTimeMax;
    private float m_idleTimer = 0.0f;

    private float m_kariTimer;
    private Animator m_animator;


    public enum StateBat
    {
        Idle,               // �ҋ@
        Death,           // ���S
    };
    private StateBat state = StateBat.Idle;
    public StateBat GetState() { return state; }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_territoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // �ҋ@
            case StateBat.Idle: ConditionIdleUpdate(); break;
            // ���S
            case StateBat.Death: ConditionDeathUpdate(); break;
        };

        if (IsDeath) ConditionDeathState();

    }
    
    

    // TODO:��Ɠr�� (��ԍŋ�) (�ҋ@���[�V�����������i���ɂ�������)
    

    // �ҋ@
    private void ConditionIdleState()
    {
        state = StateBat.Idle;

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        m_animator.SetFloat("Move", 0.0f);
        
        m_idleTimer += Time.deltaTime;
    }
    
    // ���S
    private void ConditionDeathState()
    {
        state = StateBat.Death;
        m_kariTimer = 0.0f;
        m_animator.SetBool("Death", IsDeath);

    }

    private void ConditionDeathUpdate()
    {
        if (m_kariTimer > 2.0f) Destroy(gameObject);

        m_kariTimer += Time.deltaTime;
    }
}
