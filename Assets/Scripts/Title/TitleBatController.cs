using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBatController : TitleEnemyController
{
    private static readonly int hashMove = Animator.StringToHash("Move");
    //private static readonly int hashSickleAttack = Animator.StringToHash("Attack2");
    //private static readonly int hashSickleAttackBerserker = Animator.StringToHash("Attack3");
    //private static readonly int hashDown = Animator.StringToHash("Down");
    //private static readonly int hashRevival = Animator.StringToHash("Revival");
    //private static readonly int hashSpeed = Animator.StringToHash("Zensin");

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        velocity = new Vector3(-1f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        var move = velocity * moveSpeed * Time.deltaTime;

        m_animator.SetFloat(hashMove, 0.3f);

        transform.position = new Vector3(move.x + transform.position.x, move.y + transform.position.y, move.z + transform.position.z);

        if (transform.position.x < -30) transform.position = new Vector3(30f, transform.position.y, transform.position.z);
    }
}
