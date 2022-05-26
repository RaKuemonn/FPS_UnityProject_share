using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryOrigin : MonoBehaviour
{
    public bool is_in_territory;    // SquareBattleArea���true�ɕς��Ă���
    public BaseEnemy enemy;
    
    void Update()
    {
        if (enemy == null) return;
        if (enemy.IsDeath == false) return;

        is_in_territory = false;
    }

    void OnTriggerExit(Collider collider)
    {
        // �G��
        if (collider.tag != "Enemy") return;

        // �|����Ă���
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .IsDeath == false) return;
        
        // �퓬����������
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .GetEnterBattleArea() == false) return;
        
        // ������
        is_in_territory = false;

    }
}
