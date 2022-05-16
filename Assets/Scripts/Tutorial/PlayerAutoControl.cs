using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAutoControl : MonoBehaviour
{
    [SerializeField] private GameObject m_player;                    // プレイヤー
    [SerializeField, Range(0f, 20f)] private float move_speed;       // 移動速度 (固定値)
    [SerializeField, Range(0f, 1f)] public float speed_rate = 0f;    // 移動速度の倍率 (1.0f ~ 0.0f)
    [SerializeField] private CinemachineVirtualCamera m_virtual_main_camera;       // cinemachineのvirtualCameraの参照

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
        
        // 動いているなら
        if (speed_rate > 0f)
        {

            noiseComponent.m_FrequencyGain = 5; // 固定値


            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.25f, speed_rate);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f, speed_rate);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Constant = true;
            
            return;
        }
        

        // 立ち止まり
        {
            noiseComponent.m_FrequencyGain = 1; // 固定値
            
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = 0f;
        }
    }

}
