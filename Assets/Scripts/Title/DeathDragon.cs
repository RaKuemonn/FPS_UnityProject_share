using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDragon : TitleEnemyController
{
    private static readonly int hashMove = Animator.StringToHash("MoveSpeed");
    private static readonly int hashDeath = Animator.StringToHash("Death");

    private float easingTimer = 0.0f;

    private float interval = 5.0f;

    private float checkTimer = 0.0f;

    public bool checkFlag = false;

    private float idleTimer = 4.0f;

    private enum State
    {
        Idle,
        Move,
        Death
    };
    private State state = State.Idle; 

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();


        velocity = new Vector3(-1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle: IdleUpdate(); break;
            case State.Move: MoveUpdate(); break;
            case State.Death: DeathUpdate(); break;

        };

        checkTimer += Time.deltaTime;
    }

    private void MoveState()
    {
        state = State.Move;
        m_animator.SetFloat(hashMove, 0.4f);
    }

    private void MoveUpdate()
    {
        if (1.0f > transform.position.x) checkFlag = true;
        if (0.8f > transform.position.x) DeathState();

        var move = velocity * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(move.x + transform.position.x, move.y + transform.position.y, move.z + transform.position.z);
    }

    private void IdleState()
    {

    }

    private void IdleUpdate()
    {
        if (checkTimer > interval)
        {
            int rand = Random.Range(0, 4);
            if (rand == 0)
            {
                MoveState();
            }
        }
    }

    private void DeathState()
    {
        m_animator.SetTrigger(hashDeath);
        state = State.Death;
        velocity.x = 1;
    }

    private void DeathUpdate()
    {
        if (idleTimer > 0.0f)
        {
            idleTimer -= Time.deltaTime;
            return;
        }
        moveSpeed = Random.Range(0.001f, 1.0f);

        var move = velocity.normalized * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(move.x + transform.position.x, move.y + transform.position.y, move.z + transform.position.z);
    }
}
