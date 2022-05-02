using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonController : MonoBehaviour
{
    [SerializeField]
    private float m_idleTimeMax;
    private float m_idleTimer = 0.0f;

    private float m_kariTimer;

    // ÉCÅ[ÉWÉìÉO
    private float m_easingTimer;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;

    // êÌì¨äJén
    private bool m_battleFlag = false;
    // èWçá
    private bool m_assemblyFlag = false;

    private enum StateDra
    {
        Wander, // ÉvÉåÉCÉÑÅ[Ç™Ç≠ÇÈÇ‹Ç≈úpúj
        BattlePreparation, // êÌì¨à íuÇ…à⁄ìÆ
        Idle, // ë“ã@
        AttackStart, // çUåÇäJén
        Attack,          // çUåÇíÜ
        AttackEnd    // çUåÇèIóπ
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
            // úpúj
            case StateDra.Wander: ConditionWanderUpdate(); break;
            // ë“ã@à íuÇ…à⁄ìÆ
            case StateDra.BattlePreparation: ConditionBattlePreparationUpdate(); break;
            // ë“ã@
            case StateDra.Idle: ConditionIdleUpdate(); break;
            // çUåÇäJén
            case StateDra.AttackStart: ConditionAttackStartUpdate(); break;
            // çUåÇíÜ
            case StateDra.Attack: ConditionAttackUpdate(); break;
            // çUåÇèIóπ
            case StateDra.AttackEnd: ConditionAttackEndUpdate(); break;
        };
    }

    // úpúj
    private void ConditionWanderState()
    {
        state = StateDra.Wander;

        m_idleTimer = 0.0f;
    }

    private void ConditionWanderUpdate()
    {
        if (m_idleTimer > m_idleTimeMax) ConditionAttackStartState();

        m_idleTimer += Time.deltaTime;
    }

    // ë“ã@à íuÇ…à⁄ìÆ
    private void ConditionBattlePreparationState()
    {
        state = StateDra.BattlePreparation;

        m_idleTimer = 0.0f;
    }

    private void ConditionBattlePreparationUpdate()
    {
        if (m_idleTimer > m_idleTimeMax) ConditionAttackStartState();

        m_idleTimer += Time.deltaTime;
    }

    // ë“ã@
    private void ConditionIdleState()
    {
        state = StateDra.Idle;

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        // çUåÇÇµÇƒÇ¢Ç¢Ç∆Ç´ÇæÇØ
        if (m_battleFlag)
            if (m_idleTimer > m_idleTimeMax) ConditionAttackStartState();

        m_idleTimer += Time.deltaTime;
    }

    // çUåÇäJén
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

    // çUåÇ
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

    // çUåÇèIóπ
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
}
