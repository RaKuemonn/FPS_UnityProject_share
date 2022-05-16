using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

//////////////////////////////////////////////////
//
// VirtualCamera�̗D��x��ύX���āA
// MainVirtualCamera����g�p����J������؂�ւ��Ă���
//
//////////////////////////////////////////////////


[RequireComponent(typeof(BoxCollider))]
public class VirtualCameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera notMainVirtualCamera;

    private float timer = 0f;               // �o�ߎ���
    private const float limit_time = 10f;   // �ĎR�q���������鎞��

    private int default_priority = 0;

    void Start()
    {
        // ���̗D��x��ێ����Ă���
        default_priority = notMainVirtualCamera.Priority;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;


        // �؂�ւ����VirtualCamera�̗D��x�𔚏グ����
        notMainVirtualCamera.Priority = 100;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag != "Player") return;

        if (timer >= limit_time) return;

        timer += Time.deltaTime;
        
        if (timer >= limit_time)
        {
            // �D��x�����ɖ߂�
            notMainVirtualCamera.Priority = default_priority;
        }
    }


    //void OnTriggerExit(Collider collider)
    //{
    //    // �ꉞExit()�ł��ݒ肵����
    //    notMainVirtualCamera.Priority = default_priority;
    //}

}
