using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonController : MonoBehaviour
{
    // �ړ��X�s�[�h
    public float m_moveSpeed;

    [SerializeField]
    private float m_idleTimeMax;
    private float m_idleTimer = 0.0f;

    private float m_kariTimer;

    // �C�[�W���O
    private float m_easingTimer;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;

    // �퓬�J�n
    private bool m_battleFlag = false;
    // �W��
    private bool m_assemblyFlag = false;

    // �ړI�n
    private Vector3 m_targetPosition;
    private Vector3 m_territoryOrigin;
    private float m_range = 5.0f;


    private enum StateDra
    {
        Wander,        // �v���C���[������܂Ŝp�j
        BattlePreparation, // �퓬�ʒu�Ɉړ�
        Idle,               // �ҋ@
        AttackStart, // �U���J�n
        Attack,          // �U����
        AttackEnd,   // �U���I��
        Death,           // ���S
    };
    private StateDra state = StateDra.Idle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            // �p�j
            case StateDra.Wander: ConditionWanderUpdate(); break;
            // �ҋ@�ʒu�Ɉړ�
            case StateDra.BattlePreparation: ConditionBattlePreparationUpdate(); break;
            // �ҋ@
            case StateDra.Idle: ConditionIdleUpdate(); break;
            // �U���J�n
            case StateDra.AttackStart: ConditionAttackStartUpdate(); break;
            // �U����
            case StateDra.Attack: ConditionAttackUpdate(); break;
            // �U���I��
            case StateDra.AttackEnd: ConditionAttackEndUpdate(); break;
        };
    }

    // �p�j
    private void ConditionWanderState()
    {
        state = StateDra.Wander;

        SetRandamTargetPosition();

        m_idleTimer = 0.0f;
    }

    private void ConditionWanderUpdate()
    {
        // �W��������������W�܂�
        if (m_assemblyFlag)
        {
            ConditionBattlePreparationState();
            m_assemblyFlag = false;
            return;
        }

        var dir = m_targetPosition - transform.position;
        dir *= m_moveSpeed * Time.deltaTime;
        transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_targetPosition - transform.position;

        // �ړI�n�ɒ�������ҋ@
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.5f * 0.5f) ConditionIdleState();
    }

    // �ҋ@�ʒu�Ɉړ�
    private void ConditionBattlePreparationState()
    {
        state = StateDra.BattlePreparation;

        m_idleTimer = 0.0f;
    }

    private void ConditionBattlePreparationUpdate()
    {
        
    }

    // �ҋ@
    private void ConditionIdleState()
    {
        state = StateDra.Idle;

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        // �W��������������W�܂�
        if (m_assemblyFlag)
        {
            ConditionBattlePreparationState();
            m_assemblyFlag = false;
            return;
        }

        // �U�����Ă����Ƃ�����
        if (m_battleFlag)
        {
            if (m_idleTimer > m_idleTimeMax) ConditionAttackStartState();
        }
        else
        {
            if (m_idleTimer > m_idleTimeMax) ConditionWanderState();
        }


        m_idleTimer += Time.deltaTime;
    }

    // �U���J�n
    private void ConditionAttackStartState()
    {
        state = StateDra.AttackStart;

        m_startPosition = transform.position;

        m_endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);

        m_easingTimer = 0.0f;
    }

    private void ConditionAttackStartUpdate()
    {
        if (m_easingTimer > 1.5f)
        {
            ConditionAttackState();
            return;
        }

        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_startPosition, m_endPosition);

        m_easingTimer += Time.deltaTime;
    }

    // �U��
    private void ConditionAttackState()
    {
        state = StateDra.Attack;

        m_kariTimer = 2.5f;
    }

    private void ConditionAttackUpdate()
    {
        if (m_kariTimer < 0.0f)
        {
            ConditionAttackEndState();
            return;
        }

        m_kariTimer -= Time.deltaTime;
    }

    // �U���I��
    private void ConditionAttackEndState()
    {
        state = StateDra.AttackEnd;
        m_easingTimer = 0.0f;
    }

    private void ConditionAttackEndUpdate()
    {
        if (m_easingTimer > 1.5f)
        {
            ConditionIdleState();
            return;
        }

        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_endPosition, m_startPosition);

        m_easingTimer += Time.deltaTime;
    }

    // �^�[�Q�b�g�ʒu�������_���ݒ�
    private void SetRandamTargetPosition()
    {
        float theta = Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = Random.Range(0f, m_range);
        m_targetPosition.x = m_territoryOrigin.x + Mathf.Sin(theta) * range;
        m_targetPosition.y = m_territoryOrigin.y;
        m_territoryOrigin.z = m_territoryOrigin.z * Mathf.Cos(theta) * range;
    }
}
