using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAutoControl : MonoBehaviour
{
    [SerializeField] private GameObject m_player;                    // �v���C���[
    [SerializeField, Range(0f, 20f)] private float move_speed;       // �ړ����x (�Œ�l)
    [SerializeField, Range(0f, 1f)] public float speed_rate = 0f;    // �ړ����x�̔{�� (1.0f ~ 0.0f)
    [SerializeField] private CinemachineVirtualCamera m_virtual_main_camera;       // cinemachine��virtualCamera�̎Q��

    // Update is called once per frame
    void Update()
    {
        VirtualCameraNoise();

        m_player.transform.Translate(new Vector3(0f,0f,move_speed * speed_rate * Time.deltaTime));
    }

    void OnTriggerStay(Collider collider)
    {
        
    }
    
    private void VirtualCameraNoise()
    {
        
        var noiseComponent = m_virtual_main_camera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        // �����Ă���Ȃ�
        if (speed_rate > 0f)
        {

            noiseComponent.m_FrequencyGain = 5; // �Œ�l


            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.25f, speed_rate);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f, speed_rate);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Constant = true;
            
            return;
        }
        

        // �����~�܂�
        {
            noiseComponent.m_FrequencyGain = 1; // �Œ�l
            
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = 0f;
        }
    }

}
