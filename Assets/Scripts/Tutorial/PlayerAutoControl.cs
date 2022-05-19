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
    [SerializeField] private float m_fov_state_run = 90f;
    [SerializeField] private float m_fov_state_stop = 60f;

    [SerializeField] private PlayerStatus playerStatus;
    public float current_hp;
    public float max_hp;

    private float speed_rate_lerping;

    void Start()
    {
        // 体力最大値
        current_hp = playerStatus.max_hp;
        max_hp = playerStatus.max_hp;


        speed_rate_lerping = speed_rate;
    }

    // Update is called once per frame
    void Update()
    {
        speed_rate_lerping = Mathf.Lerp(speed_rate_lerping, speed_rate, Time.deltaTime * 2f);

        m_virtual_main_camera.m_Lens.FieldOfView = Mathf.Lerp(m_fov_state_stop, m_fov_state_run, speed_rate_lerping);


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


        noiseComponent.m_FrequencyGain = 5; // 固定値

        // 動いているなら
        if (speed_rate > 0f)
        {
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.25f, speed_rate_lerping);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f, speed_rate_lerping);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Constant = true;
            
            return;
        }
        

        // 立ち止まり
        {
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = 0f;
        }
    }

}
