using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEnemyController : BaseEnemy
{

    [SerializeField] private GameObject damageArea;

    public override void OnDead()
    {
        base.OnDead();

        if(damageArea == null) return;
        
        Destroy(damageArea);
    }
}
