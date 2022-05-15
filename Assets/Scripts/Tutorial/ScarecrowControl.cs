using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowControl : BaseEnemy
{

    [SerializeField]
    private float m_idleTimeMax;

    private float m_idleTimer = 0.0f;

    private bool m_first_condition_idle_state = false;  // ���ڂ�ConditionIdleState()�ł̂ݎ��s

    private bool m_enter_battle_area = false;

    // �C�[�W���O
    private float m_easingTimer;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;
    
    private enum StateScarecrow
    {
        Idle,               // �ҋ@
        AttackStart, // �U���J�n
        Death,           // ���S
    };
    private StateScarecrow state = StateScarecrow.Idle;

    // Start is called before the first frame update
    void Start()
    {
        m_territoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // �ҋ@
            case StateScarecrow.Idle: ConditionIdleUpdate(); break;
            // �U���J�n
            case StateScarecrow.AttackStart: ConditionAttackStartUpdate(); break;
            // ���S
            case StateScarecrow.Death: ConditionDeathUpdate(); break;
        };

        if (m_death) ConditionDeathState();

        //Debug.Log(state);
    }
    
    // �ҋ@
    private void ConditionIdleState()
    {
        state = StateScarecrow.Idle;
        
        m_idleTimer = 0.0f;

        if (m_first_condition_idle_state == false)  // ���̂ݎ��s
        {
            m_first_condition_idle_state = true;
            m_battleFlag = true;
        }
    }

    private void ConditionIdleUpdate()
    {
        if (m_battleFlag/* ��񂾂��U���X�e�[�g�֑J�� */)
        {
            if (m_enter_battle_area && (m_idleTimer > m_idleTimeMax)) { ConditionAttackStartState(); }

            transform.rotation = TurnRotation(transform.forward, new Vector3(0, 0, -1), m_turnAngle, m_turnSpeed);
        }

        m_idleTimer += Time.deltaTime;
    }

    // �U���J�n
    private void ConditionAttackStartState()
    {
        state = StateScarecrow.AttackStart;

        m_startPosition = transform.position;

        m_endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);


        m_easingTimer = 0.0f;
    }

    private void ConditionAttackStartUpdate()
    {
        if (m_easingTimer > 1.5f)
        {
            // ���̏�őҋ@
            ConditionIdleState();
            return;
        }
        
        // �߂Â�
        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_startPosition, m_endPosition);

        m_easingTimer += Time.deltaTime;
    }
    
    // ���S
    private void ConditionDeathState()
    {
        state = StateScarecrow.Death;
        //m_animator.SetBool("Death", m_death);
    }

    private void ConditionDeathUpdate()
    {
    }

    // �퓬�G���A�ɓ�������R�[���o�b�N�����
    public void OnEnterBattleArea()
    {
        m_enter_battle_area = true;
    }
}
