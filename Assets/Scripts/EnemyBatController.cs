using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatController : BaseEnemy
{
    // 移動スピード
    public float m_moveSpeed;

    [SerializeField]
    private float m_idleTimeMax;
    private float m_idleTimer = 0.0f;

    private float m_kariTimer;

    // イージング
    private float m_easingTimer;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;


    private enum StateBat
    {
        Wander,        // プレイヤーがくるまで徘徊
        BattlePreparation, // 戦闘位置に移動
        Idle,               // 待機
        AttackStart, // 攻撃開始
        Attack,          // 攻撃中
        AttackEnd,   // 攻撃終了
        Death,           // 死亡
    };
    private StateBat state = StateBat.Idle;

    // Start is called before the first frame update
    void Start()
    {
        m_territoryOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 徘徊
    private void ConditionWanderState()
    {
        state = StateBat.Wander;

        SetRandamTargetPosition();
        Debug.Log(m_targetPosition);

        m_idleTimer = 0.0f;
    }

    private void ConditionWanderUpdate()
    {
        // 集合がかかったら集まる
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

        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.2f * 0.2f) ConditionIdleState();
    }

    // 待機位置に移動
    private void ConditionBattlePreparationState()
    {
        state = StateBat.BattlePreparation;
    }

    private void ConditionBattlePreparationUpdate()
    {
        var dir = m_locationPosition - transform.position;
        dir.Normalize();
        dir *= m_moveSpeed * Time.deltaTime;
        transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_locationPosition - transform.position;

        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.1f * 0.1f)
        {
            ConditionIdleState();
            m_battleFlag = true;
        }
    }

    // 待機
    private void ConditionIdleState()
    {
        state = StateBat.Idle;

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        // 集合がかかったら集まる
        if (m_assemblyFlag)
        {
            ConditionBattlePreparationState();
            m_assemblyFlag = false;
            return;
        }

        // 攻撃していいときだけ
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

    // 攻撃開始
    private void ConditionAttackStartState()
    {
        state = StateBat.AttackStart;

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

    // 攻撃
    private void ConditionAttackState()
    {
        state = StateBat.Attack;

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

    // 攻撃終了
    private void ConditionAttackEndState()
    {
        state = StateBat.AttackEnd;
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


    // 死亡
    private void ConditionDeathState()
    {
        state = StateBat.Death;
    }

    private void ConditionDeathUpdate()
    {
    }
}
