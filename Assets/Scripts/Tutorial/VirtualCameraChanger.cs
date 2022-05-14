using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VirtualCameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;

    private float timer = 0f;               // 経過時間
    private const float limit_time = 10f;   // 案山子を見続ける時間

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;


        // もう一つのVirtualCameraにブレンドされる(予定)
        mainVirtualCamera.enabled = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag != "Player") return;

        if (timer >= limit_time) return;

        timer += Time.deltaTime;
        
        if (timer >= limit_time)
        {
            mainVirtualCamera.enabled = true;
        }
    }


    //void OnTriggerExit(Collider collider)
    //{
    //    // 一応
    //    mainVirtualCamera.enabled = true;
    //}

}
