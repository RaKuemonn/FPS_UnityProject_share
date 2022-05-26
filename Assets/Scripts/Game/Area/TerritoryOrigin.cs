using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryOrigin : MonoBehaviour
{
    public bool is_in_territory;
    void OnTriggerStay(Collider collider)
    {
        // 初期化
        is_in_territory = false;

        if (collider.tag != "Enemy") return;

        if (collider
            .gameObject
            .GetComponent<BaseEnemy>()
            .IsDeath == true) return;

        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .GetEnterBattleArea() == false) return;


        // ここまできたらtrueにする
        is_in_territory = true;
    }
}
