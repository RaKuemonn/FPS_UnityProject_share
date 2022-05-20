using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantuneEnemyControl : BaseEnemy
{
    [SerializeField] private float destroy_time = 2.1f;
    [SerializeField] private TeleportTimerMaterialChange teleport;

    public override void OnDead()
    {
        base.OnDead();
    }

    protected override void CuttedImpulse(Vector3 impulse_)
    {
        teleport.OnDead();

        //GameObject.FindGameObjectWithTag("Slash");

        var rigidbody = GetComponent<Rigidbody>();

        // rigidbody�v���p�e�B�̕ύX
        {
            // �d�͂ɏ]��
            //rigidbody.useGravity = true;

            // �S���𖳂���
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        impulse_.Normalize();

        impulse_.z = 0f;


        // �e����΂�
        rigidbody.AddForce(impulse_, ForceMode.Impulse);
    }

    protected override float DestroyTime()
    {
        return destroy_time;
    }
}
