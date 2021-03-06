using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryOrigin : MonoBehaviour
{
    public bool is_in_territory;    // SquareBattleArea上でtrueに変えている
    public BaseEnemy enemy;
    
    void Update()
    {
        if (enemy == null) return;
        if (enemy.IsDeath == false) return;

        is_in_territory = false;
    }

    void OnTriggerExit(Collider collider)
    {
        // 敵で
        if (collider.tag != "Enemy") return;

        // 倒されていて
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .IsDeath == false) return;
        
        // 戦闘中だったら
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .GetEnterBattleArea() == false) return;
        
        // 初期化
        is_in_territory = false;

    }
}
