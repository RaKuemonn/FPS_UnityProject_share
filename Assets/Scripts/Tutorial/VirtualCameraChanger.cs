using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

//////////////////////////////////////////////////
//
// VirtualCameraの優先度を変更して、
// MainVirtualCameraから使用するカメラを切り替えている
//
//////////////////////////////////////////////////


[RequireComponent(typeof(BoxCollider))]
public class VirtualCameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera notMainVirtualCamera;

    private float timer = 0f;               // 経過時間
    private const float limit_time = 10f;   // 案山子を見続ける時間

    private int default_priority = 0;

    void Start()
    {
        // 元の優先度を保持しておく
        default_priority = notMainVirtualCamera.Priority;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;


        // 切り替え先のVirtualCameraの優先度を爆上げする
        notMainVirtualCamera.Priority = 100;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag != "Player") return;

        if (timer >= limit_time) return;

        timer += Time.deltaTime;
        
        if (timer >= limit_time)
        {
            // 優先度を元に戻す
            notMainVirtualCamera.Priority = default_priority;
        }
    }


    //void OnTriggerExit(Collider collider)
    //{
    //    // 一応Exit()でも設定し直す
    //    notMainVirtualCamera.Priority = default_priority;
    //}

}
