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
    [SerializeField] private float m_fov_state_run = 90f;
    [SerializeField] private float m_fov_state_stop = 60f;
    
    [SerializeField] private PlayerHPController hpController;

    private float speed_rate_lerping;

    void Start()
    {
        speed_rate_lerping = speed_rate;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed_rate <= 0f)
        {
            int a = 0;
        }

        speed_rate_lerping = Mathf.Lerp(speed_rate_lerping, speed_rate, Time.deltaTime * 0.5f);

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

        if(noiseComponent == null) return;

        noiseComponent.m_FrequencyGain = Mathf.Lerp(1.0f, 2.0f, speed_rate_lerping); // �Œ�l
        

        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.24f, speed_rate_lerping);
        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f * 0.5f, speed_rate_lerping);
        noiseComponent
            .m_NoiseProfile
            .OrientationNoise[0].X.Constant = true;

        return;

        // �����Ă���Ȃ�
        if (speed_rate > 0f)
        {
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Frequency = Mathf.Lerp(0.22f, 0.25f, speed_rate_lerping);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = Mathf.Lerp(0f, 2.5f, speed_rate_lerping);
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Constant = true;
            
            return;
        }
        

        // �����~�܂�
        {
            noiseComponent.m_NoiseProfile.OrientationNoise[0].X.Amplitude = 0f;
        }
    }

    public void OnDamage(float damage_)
    {
        hpController.OnDamaged(
            damage_,
            delegate () { GetComponent<ChangeScene>().Change(); });
    }
}
