using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnakeController : BaseEnemy
{
    private static readonly int hashAttack = Animator.StringToHash("Attack");
    private static readonly int hashDeath = Animator.StringToHash("Death");


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


    public enum StateRab
    {
        Wander,        // プレイヤーがくるまで徘徊
        BattlePreparation, // 戦闘位置に移動
        Idle,               // 待機
        AttackStart, // 攻撃開始
        Attack,          // 攻撃中
        AttackEnd,   // 攻撃終了
        Death,           // 死亡
    };
    private StateRab state = StateRab.Idle;
    public StateRab GetState() { return state; }

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
            // 徘徊
            case StateRab.Wander: ConditionWanderUpdate(); break;
            // 待機位置に移動
            case StateRab.BattlePreparation: ConditionBattlePreparationUpdate(); break;
            // 待機
            case StateRab.Idle: ConditionIdleUpdate(); break;
            // 攻撃開始
            case StateRab.AttackStart: ConditionAttackStartUpdate(); break;
            // 攻撃中
            case StateRab.Attack: ConditionAttackUpdate(); break;
            // 攻撃終了
            case StateRab.AttackEnd: ConditionAttackEndUpdate(); break;
            // 死亡
            case StateRab.Death: ConditionDeathUpdate(); break;
        };

        if (IsDeath) ConditionDeathState();

    }

    // 徘徊
    private void ConditionWanderState()
    {
        state = StateRab.Wander;

        SetRandamTargetPosition();
        Debug.Log(m_targetPosition);

        m_idleTimer = 0.0f;
    }

    private void ConditionWanderUpdate()
    {
      //  m_animator.SetFloat("Move", 0.7f);

        // 集合がかかったら集まる
        if (m_assemblyFlag)
        {
            ConditionBattlePreparationState();
            m_assemblyFlag = false;
            return;
        }



        var dir = m_targetPosition - transform.position;
        dir.Normalize();
        dir *= m_moveSpeed * Time.deltaTime;
        if (Turn(dir)) transform.position = new Vector3(dir.x + transform.position.x, transform.position.y, transform.position.z + dir.z);

        dir = m_targetPosition - transform.position;



        // 目的地に着いたら待機
        var lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (lengthSq < 0.2f * 0.2f) ConditionIdleState();
    }

    // 待機位置に移動
    private void ConditionBattlePreparationState()
    {
        state = StateRab.BattlePreparation;
    }

    private void ConditionBattlePreparationUpdate()
    {
     //   m_animator.SetFloat("Move", 0.7f);

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
    }

    // 待機
    private void ConditionIdleState()
    {
        state = StateRab.Idle;

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
     //   m_animator.SetFloat("Move", 0.0f);


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
        state = StateRab.AttackStart;

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

     //   m_animator.SetFloat("Move", 0.5f);

        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_startPosition, m_endPosition);

        m_easingTimer += Time.deltaTime;
    }

    // 攻撃
    private void ConditionAttackState()
    {
        state = StateRab.Attack;
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
        state = StateRab.AttackEnd;
        m_easingTimer = 0.0f;
    }

    private void ConditionAttackEndUpdate()
    {
       // m_animator.SetFloat("Move", 0.4f);


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
        state = StateRab.Death;
        m_kariTimer = 0.0f;
        m_animator.SetTrigger("Death");

    }

    private void ConditionDeathUpdate()
    {
        if (m_kariTimer > 2.0f) Destroy(gameObject);

        m_kariTimer += Time.deltaTime;
    }
}
