using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowMovingControl : BaseEnemy
{
    [SerializeField] private float m_idleTimeMax;

    private float m_idleTimer = 0.0f;

    private bool m_first_condition_idle_state = false; // 一回目のConditionIdleState()でのみ実行

    // イージング
    private float m_easingTimer;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;

    private enum StateScarecrow
    {
        Idle, // 待機
        AttackStart, // 攻撃開始
        Death, // 死亡
    };

    private StateScarecrow state = StateScarecrow.Idle;

    // Start is called before the first frame update
    void Start()
    {
        m_territoryOrigin = transform.position;

        // 待機状態に設定
        ConditionIdleState();

        // 当たり判定の生成
        //CreateCollideOnCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // 待機
            case StateScarecrow.Idle:
                ConditionIdleUpdate();
                break;
            // 攻撃開始
            case StateScarecrow.AttackStart:
                ConditionAttackStartUpdate();
                break;
            // 死亡
            case StateScarecrow.Death:
                ConditionDeathUpdate();
                break;
        }

        ;

        if (IsDeath) ConditionDeathState();

        //Debug.Log(state);
    }

    // 待機
    private void ConditionIdleState()
    {
        state = StateScarecrow.Idle;

        m_idleTimer = 0.0f;

        if (m_first_condition_idle_state == false) // 一回のみ実行
        {
            m_first_condition_idle_state = true;
            m_battleFlag = true;
        }
    }

    private void ConditionIdleUpdate()
    {
        if (m_battleFlag /* 一回だけ攻撃ステートへ遷移 */)
        {
            if (m_enter_battle_area && (m_idleTimer > m_idleTimeMax))
            {
                ConditionAttackStartState();
            }
        }

        m_idleTimer += Time.deltaTime;
    }

    // 攻撃開始
    private void ConditionAttackStartState()
    {
        m_battleFlag = false; // これ以降trueにならない

        state = StateScarecrow.AttackStart;

        m_startPosition = transform.position;

        m_endPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);


        m_easingTimer = 0.0f;
    }

    private void ConditionAttackStartUpdate()
    {
        if (m_easingTimer > 1.5f)
        {
            // その場で待機
            ConditionIdleState();
            return;
        }

        // 近づく
        transform.position = Easing.SineInOut(m_easingTimer, 1.5f, m_startPosition, m_endPosition);

        m_easingTimer += Time.deltaTime;
    }

    // 死亡
    private void ConditionDeathState()
    {
        state = StateScarecrow.Death;
        //m_animator.SetBool("Death", m_death);
    }

    private void ConditionDeathUpdate()
    {
    }
    

}
