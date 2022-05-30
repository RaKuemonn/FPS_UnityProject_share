using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] private MasterData masterData;
    [SerializeField] private GameObject hpGauge;

    private Image hpBarRed;
    private Image hpBarGreen;

    private Tween redGaugeTween;
    private Tween Timer;

    private const float time = 2.0f;
    private float timer = 0f;

    public void Start()
    {
        var redBar = hpGauge.transform.Find("GaugeFrame/RedGaugeBar")?.gameObject;
        var greenBar = hpGauge.transform.Find("GaugeFrame/GreenGaugeBar")?.gameObject;

        hpBarRed   = redBar?.GetComponent<Image>();
        hpBarGreen = greenBar?.GetComponent<Image>();
    }

    public void Update()
    {

    }
    
    

    public void OnDamaged(float damaged_, Action game_over_)
    {
        var playerStatus = masterData.PlayerStatus;
        
        // �_���[�W����
        {
            var damaged_hp = playerStatus.current_hp - damaged_; // �_���[�W���󂯂��̗͒l

            var safety_damaged = (damaged_hp >= 0f) ?
                damaged_ :                                  // ���̂܂܃_���[�W��^����
                damaged_ + damaged_hp;                      // �O�������Ȃ��_���[�W�ɕύX����

            // �̗̓Q�[�W�̍X�V
            GaugeReduction(safety_damaged);

            // �_���[�W�v�Z
            playerStatus.current_hp += -safety_damaged;
        }
        
        // �Q�[���I�[�o�[����
        if (playerStatus.current_hp <= 0f + float.Epsilon * 2.0f)
        {
            // �Ȃɂ�����̊֐����Ăяo���B
            game_over_?.Invoke();
        }
    }

    private void GaugeReduction(float reducationValue, float time = 1f)
    {
        var playerStatus = masterData.PlayerStatus;

        var valueFrom = playerStatus.current_hp / playerStatus.max_hp;
        var valueTo = (playerStatus.current_hp - reducationValue) / playerStatus.max_hp;

        // �΃Q�[�W����
        hpBarGreen.fillAmount = valueTo;

        timer = time;
        if (redGaugeTween != null)
        {
            // kill() �́@�r���Œ�~
            redGaugeTween.Kill();
            Timer.Kill();
        }

        // �ԃQ�[�W�����̃X�^�[�g���ԃ^�C�}�[
        Timer = DOTween.To(
            () => 0.0f,    // ����
            x => {         // ����

                if (x < 1.0f) return;

                // �ԃQ�[�W����
                redGaugeTween = DOTween.To(
                    () => valueFrom,    // ����
                    g => {         // ����
                        if (hpBarRed)
                        { hpBarRed.fillAmount = g; }
                    },
                    valueTo,        // �ڕW�l
                    time    // �����鎞��
                );

            },
            1.0f,        // �ڕW�l
            time    // �����鎞��
        );
    }
}
