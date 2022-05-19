using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaControl : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") == false) return;
        
        
    }
}
