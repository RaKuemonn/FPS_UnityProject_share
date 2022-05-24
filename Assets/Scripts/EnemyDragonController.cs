using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragonController : BaseEnemy
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

    private Animator m_animator;

    public enum StateDra
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
    public StateDra GetState() { return state; }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_territoryOrigin = transform.position;
        m_turnAngle = 1.0f;

        // 当たり判定の生成
        //CreateCollideOnCanvas();
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
            // 死亡
            case StateDra.Death: ConditionDeathUpdate(); break;
        };

        if (IsDeath) ConditionDeathState();
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
        m_animator.SetFloat("MoveSpeed", 0.7f);


        var dir = m_targetPosition - transform.position;
        dir.Normalize();
        dir *= m_moveSpeed * Time.deltaTime;
        if (Turn(dir))  transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_targetPosition - transform.position;

        //Quaternion rotate = TurnRotation(transform.forward, dir, m_turnAngle, m_turnSpeed);
        //transform.rotation = rotate;

        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.2f * 0.2f) ConditionIdleState();
    }

    // 待機位置に移動
    private void ConditionBattlePreparationState()
    {
        state = StateDra.BattlePreparation;

        m_animator.SetFloat("MoveSpeed", 0.7f);

    }

    private void ConditionBattlePreparationUpdate()
    {
        var dir = m_locationPosition - transform.position;
        dir.Normalize();
        dir *= m_moveSpeed * Time.deltaTime;
        if (Turn(dir)) transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_locationPosition - transform.position;

        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.1f * 0.1f)
        {
            ConditionIdleState();
            m_battleFlag = true;
        }

        //transform.rotation = TurnRotation(transform.forward, dir, m_turnAngle, m_turnSpeed);
    }

    // 待機
    private void ConditionIdleState()
    {
        state = StateDra.Idle;


        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        m_animator.SetFloat("MoveSpeed", 0.0f);


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
            Turn(new Vector3(0, 0, -1));
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

        GameObject player = GameObject.FindWithTag("Player");

        m_endPosition = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + 1.7f);


        m_easingTimer = 0.0f;
    }

    private void ConditionAttackStartUpdate()
    {
        if (m_easingTimer > 1.5f)
        {
            ConditionAttackState();
            return;
        }

        m_animator.SetFloat("MoveSpeed", 0.5f);

        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_startPosition, m_endPosition);

        m_easingTimer += Time.deltaTime;
    }

    // 攻撃
    private void ConditionAttackState()
    {
        state = StateDra.Attack;

        m_animator.SetTrigger("Attack");

        m_kariTimer = 2.5f;
    }

    private void ConditionAttackUpdate()
    {
        GameObject player = GameObject.FindWithTag("Player");
        var dir = player.transform.position - transform.position;
        Turn(dir);

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("stand_by"))
        {
            ConditionAttackEndState();

            // プレイヤーにダメージを与える
            GameObject
                .FindGameObjectWithTag("Player")
                .GetComponent<PlayerAutoControl>()
                .OnDamage(m_damage);

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
        Turn(new Vector3(0,0,-1));

        if (m_easingTimer > 1.5f)
        {
            ConditionIdleState();
            return;
        }

        m_animator.SetFloat("MoveSpeed", 0.4f);

        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_endPosition, m_locationPosition);

        m_easingTimer += Time.deltaTime;
    }


    // 死亡
    private void ConditionDeathState()
    {
        state = StateDra.Death;
        m_animator.SetBool("Death",IsDeath);
    }

    private void ConditionDeathUpdate()
    {
    }
}
