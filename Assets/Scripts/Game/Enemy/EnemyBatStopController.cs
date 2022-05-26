using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatStopController : BaseEnemy
{
    private Animator m_animator;

    [SerializeField] private GameObject damageArea;

    private float m_idleTimer = 0.0f;
    private float m_idleTimeMax;

    private float m_easingTimer = 0.0f;
    private float m_easingTimeMax = 0.5f;

    //private Vector3 m_targetPosition;
    private Vector3 m_oldPosition;

    public enum StateBat
    {
        Idle,               // 待機
        Move,            // 移動
    };
    private StateBat state = StateBat.Idle;
    public StateBat GetState() { return state; }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_territoryOrigin = transform.position;

        m_idleTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            // 待機
            case StateBat.Idle: ConditionIdleUpdate(); break;
            // 移動
            case StateBat.Move: ConditionMoveUpdate(); break;
        };

    }
    
    // 待機
    private void ConditionIdleState()
    {
        state = StateBat.Idle;

        m_idleTimeMax = Random.Range(1.0f, 4.0f);

        m_idleTimer = 0.0f;
    }

    private void ConditionIdleUpdate()
    {
        m_animator.SetFloat("stand_by", 0.0f);

        if (m_idleTimer > m_idleTimeMax)
        {
            ConditionMoveState();
            return;
        }

        m_idleTimer += Time.deltaTime;
    }

    // 待機
    private void ConditionMoveState()
    {
        state = StateBat.Move;

        m_easingTimer = 0.0f;

        m_oldPosition = transform.position;

        float height = Random.Range(0.5f, 2.0f);
        float width = Random.Range(0f, 3.5f);

        GameObject player = GameObject.FindWithTag("Player");
        var dir = transform.position - player.transform.position;
        Vector2 xz = new Vector2(0f, 1f);
        float cross = (dir.x * xz.y) - (dir.z * xz.x);
        if (cross < 0.0f)
        {
            m_targetPosition = new Vector3(width, height, transform.position.z);
        }
        else
        {
            m_targetPosition = new Vector3(-width, height, transform.position.z);
        }

    }
    
    private void ConditionMoveUpdate()
    {

        Vector3 position = transform.position;
        Vector3 pos = Easing.SineInOut(m_easingTimer, m_easingTimeMax, m_oldPosition, m_targetPosition);
        transform.position = Vector3.Lerp(position, pos, 0.5f);

        if (m_easingTimer > m_easingTimeMax)
        {
            m_easingTimer = m_easingTimeMax;
            ConditionIdleState();
        }

        m_easingTimer += Time.deltaTime;
    }

    public override void OnDead()
    {
        base.OnDead();

        Destroy(damageArea);
    }

}
