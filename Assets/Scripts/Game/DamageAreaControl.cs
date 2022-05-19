using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaControl : MonoBehaviour
{

    [SerializeField] private float damage;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") == false) return;

        collider
            .gameObject
            .GetComponent<PlayerAutoControl>()
            .OnDamage(damage);

        Destroy(gameObject);
    }
}
