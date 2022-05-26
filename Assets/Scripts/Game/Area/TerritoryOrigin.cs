using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryOrigin : MonoBehaviour
{
    public bool is_in_territory;
    void OnTriggerStay(Collider collider)
    {
        // ‰Šú‰»
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


        // ‚±‚±‚Ü‚Å‚«‚½‚çtrue‚É‚·‚é
        is_in_territory = true;
    }
}
