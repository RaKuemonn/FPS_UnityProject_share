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
    [SerializeField] private CinemachineVirtualCamera m_virtual_death_camera;
    [SerializeField] private float m_fov_state_run = 90f;
    [SerializeField] private float m_fov_state_stop = 60f;
    
    [SerializeField] private PlayerHPController hpController;

    private float speed_rate_lerping;
    
    private bool death;
    private float death_timer;
    private const float death_time = 3.0f;

    void Start()
    {
        speed_rate_lerping = speed_rate;
    }

    // Update is called once per frame
    void Update()
    {

        speed_rate_lerping = Mathf.Lerp(speed_rate_lerping, speed_rate, Time.deltaTime * 0.5f);

        m_virtual_main_camera.m_Lens.FieldOfView = Mathf.Lerp(m_fov_state_stop, m_fov_state_run, speed_rate_lerping);


        VirtualCameraNoise();

        DeathPerformance();

        m_player.transform.Translate(new Vector3(0f,0f,move_speed * speed_rate * Time.deltaTime));
    }
    
    
    private void VirtualCameraNoise()
    {
        
        var noiseComponent = m_virtual_main_camera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if(noiseComponent == null) return;

        noiseComponent.m_FrequencyGain = Mathf.Lerp(1.0f, 2.0f, 1.0f - speed_rate_lerping); // 固定値
        

        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.24f, speed_rate_lerping);
        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f * 0.5f, speed_rate_lerping);
        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Constant = true;
        
    }

    public void OnDamage(float damage_)
    {
        hpController.OnDamaged(
            damage_,
            delegate()
            {
                death = true;

                if (m_virtual_death_camera)
                {
                    m_virtual_death_camera.transform.rotation
                        = m_virtual_main_camera.transform.rotation;

                    m_virtual_death_camera.transform
                        .Rotate(m_virtual_death_camera.transform.right,
                            87.0f, Space.World);
                }
            });
    }

    private void DeathPerformance()
    {
        if (death == false) return;
        if (m_virtual_death_camera == null) return;

        // 使うカメラを変更する。
        m_virtual_death_camera.Priority = 11;

        // 速度をゼロにする
        speed_rate = 0.0f;

        death_timer += Time.deltaTime;

        if (death_timer < death_time)
        {
            return;
        }


        // death_timerがdeath_timeの時間を超えていたら
        // 遷移する
        m_virtual_death_camera.Priority = 1;
        GetComponent<ChangeScene>().Change();

    }
}
