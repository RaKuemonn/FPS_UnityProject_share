using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNaruController : MonoBehaviour
{
    public float moveSpeed;
    public float attackRange;


    private GameObject player;

    private float attackTimer = 0.0f;
    private float attackTimerMax = 3.0f;
    private float idleRange = 9.0f;

    private bool attackFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        IdleUodate();

        AttackUpdate();
    }

    private void AttackUpdate()
    {
        if (!attackFlag) return;
        var distance = Vector3.Distance(PlayerPosition(), transform.position);

        if (distance > attackRange)
        {
            UpdateGoToPlayer();
        }
        else
        {
            attackFlag = false;
            attackTimer = attackTimerMax;
        }
    }

    private void IdleUodate()
    {
        if (attackFlag) return;

        var distance = Vector3.Distance(PlayerPosition(), transform.position);
        if (idleRange < distance)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0) attackFlag = true;
        }
        else UpdateBackStep();
    }

    private void UpdateGoToPlayer()
    {
        if (player == null) return;

        var to_player_vec = PlayerPosition() - transform.position;

        var to_player_dir = to_player_vec.normalized;
        transform.Translate(to_player_dir * moveSpeed * Time.deltaTime);
    }

    private void UpdateBackStep()
    {
        if (player == null) return;

        var to_player_vec = PlayerPosition() - transform.position;

        var to_player_dir = -to_player_vec.normalized;
        transform.Translate(to_player_dir * moveSpeed * Time.deltaTime);
    }

    private Vector3 PlayerPosition()
    {
        var p_pos = player.transform.position;
        return new Vector3(p_pos.x, p_pos.y + 0.5f, p_pos.z);
    }
}
