using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonController : MonoBehaviour
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

    // 戦闘開始
    private bool m_battleFlag = false;
    // 集合
    private bool m_assemblyFlag = false;

    // 目的地
    private Vector3 m_targetPosition;
    private Vector3 m_territoryOrigin;
    private float m_range = 5.0f;


    private enum StateDra
    {
        Wander,        // プレイヤーがくるまで徘徊
        BattlePreparation, // 戦闘位置に移動
        Idle,               // 待機
        AttackStart, // 攻撃開始
        Attack,          // 攻撃中
        AttackEnd,   // 攻撃終了
        Death,           // 死亡
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
            // 徘徊
            case StateDra.Wander: ConditionWanderUpdate(); break;
            // 待機位置に移動
            case StateDra.BattlePreparation: ConditionBattlePreparationUpdate(); break;
            // 待機
            case StateDra.Idle: ConditionIdleUpdate(); break;
            // 攻撃開始
            case StateDra.AttackStart: ConditionAttackStartUpdate(); break;
            // 攻撃中
            case StateDra.Attack: ConditionAttackUpdate(); break;
            // 攻撃終了
            case StateDra.AttackEnd: ConditionAttackEndUpdate(); break;
        };
    }

    // 徘徊
    private void ConditionWanderState()
    {
        state = StateDra.Wander;

        SetRandamTargetPosition();

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
        if (lengthSq < 0.5f * 0.5f) ConditionIdleState();
    }

    // 待機位置に移動
    private void ConditionBattlePreparationState()
    {
        state = StateDra.BattlePreparation;

        m_idleTimer = 0.0f;
    }

    private void ConditionBattlePreparationUpdate()
    {
        
    }

    // 待機
    private void ConditionIdleState()
    {
        state = StateDra.Idle;

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

    // 攻撃
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

    // 攻撃終了
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

    // ターゲット位置をランダム設定
    private void SetRandamTargetPosition()
    {
        float theta = Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = Random.Range(0f, m_range);
        m_targetPosition.x = m_territoryOrigin.x + Mathf.Sin(theta) * range;
        m_targetPosition.y = m_territoryOrigin.y;
        m_territoryOrigin.z = m_territoryOrigin.z * Mathf.Cos(theta) * range;
    }
}
