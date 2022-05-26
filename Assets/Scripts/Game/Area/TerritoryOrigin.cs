using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryOrigin : MonoBehaviour
{
    public bool is_in_territory;    // SquareBattleAreaã‚Åtrue‚É•Ï‚¦‚Ä‚¢‚é
    public BaseEnemy enemy;
    
    void Update()
    {
        if (enemy == null) return;
        if (enemy.IsDeath == false) return;

        is_in_territory = false;
    }

    void OnTriggerExit(Collider collider)
    {
        // “G‚Å
        if (collider.tag != "Enemy") return;

        // “|‚³‚ê‚Ä‚¢‚Ä
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .IsDeath == false) return;
        
        // í“¬’†‚¾‚Á‚½‚ç
        if (collider
                .gameObject
                .GetComponent<BaseEnemy>()
                .GetEnterBattleArea() == false) return;
        
        // ‰Šú‰»
        is_in_territory = false;

    }
}
