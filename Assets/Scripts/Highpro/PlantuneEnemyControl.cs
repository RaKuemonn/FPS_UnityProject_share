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

        // rigidbodyプロパティの変更
        {
            // 重力に従う
            rigidbody.useGravity = true;

            // 拘束を無くす
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        // 弾き飛ばす
        rigidbody.AddForce(impulse_, ForceMode.Impulse);
    }
}
