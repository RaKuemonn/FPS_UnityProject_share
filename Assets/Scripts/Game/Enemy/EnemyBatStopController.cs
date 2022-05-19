using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatStopController : BaseEnemy
{
    private Animator m_animator;

    [SerializeField] private GameObject damageArea;

    public enum StateBat
    {
        Idle,               // 待機
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
            // 待機
            case StateBat.Idle: ConditionIdleUpdate(); break;
        };

    }
    
    // 待機
    private void ConditionIdleState()
    {
        state = StateBat.Idle;
    }

    private void ConditionIdleUpdate()
    {
        m_animator.SetFloat("Move", 0.0f);
    }

    public override void OnDead()
    {
        base.OnDead();

        Destroy(damageArea);
    }
}
