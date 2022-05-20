using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantuneEnemyControl : BaseEnemy
{

    public override void OnDead()
    {
        base.OnDead();
    }

    protected override void CuttedImpulse(Vector3 impulse_)
    {
        var rigidbody = GetComponent<Rigidbody>();

        // rigidbody�v���p�e�B�̕ύX
        {
            // �d�͂ɏ]��
            rigidbody.useGravity = true;

            // �S���𖳂���
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        // �e����΂�
        rigidbody.AddForce(impulse_, ForceMode.Impulse);
    }
}
