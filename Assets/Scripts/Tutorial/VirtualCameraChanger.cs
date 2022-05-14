using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VirtualCameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;

    private float timer = 0f;               // �o�ߎ���
    private const float limit_time = 10f;   // �ĎR�q���������鎞��

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;


        // �������VirtualCamera�Ƀu�����h�����(�\��)
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
    //    // �ꉞ
    //    mainVirtualCamera.enabled = true;
    //}

}
