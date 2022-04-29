using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoControl : MonoBehaviour
{
    [SerializeField] private GameObject m_player;                    // プレイヤー
    [SerializeField, Range(0f, 20f)] private float move_speed;       // 移動速度 (固定値)
    [SerializeField, Range(0f, 1f)] public float speed_rate = 0f;    // 移動速度の倍率 (1.0f ~ 0.0f)
    
    
    
    // Update is called once per frame
    void Update()
    {


        m_player.transform.Translate(new Vector3(0f,0f,move_speed * speed_rate * Time.deltaTime));
    }

    void OnTriggerStay(Collider collider)
    {
        
    }
    
}
