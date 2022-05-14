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
        ////////////////////////////////////////////////////
        ///
        /// ����m�F
        ///
        ////////////////////////////////////////////////////
        timer += Time.deltaTime;
        if (timer % 5f > 1f)
        {
            OnDamaged(30f);
            timer = 0f;
        }

        if (masterData.PlayerStatus.current_hp <= 0f)
        {
            masterData.PlayerStatus.current_hp = masterData.PlayerStatus.max_hp;
        }

    }
    
    

    public void OnDamaged(float damaged_)
    {
        var playerStatus = masterData.PlayerStatus;
        var damaged_hp = playerStatus.current_hp - damaged_; // �_���[�W���󂯂��̗͒l

        // �_���[�W����
        {
            var safety_damaged = (damaged_hp > 0f) ?
                damaged_ :                                  // ���̂܂܃_���[�W��^����
                damaged_ - damaged_hp;                      // �O�������Ȃ��_���[�W��^����

            // �_���[�W�v�Z
            playerStatus.current_hp -= safety_damaged;
            // �̗̓Q�[�W�̍X�V
            GaugeReduction(safety_damaged);
        }
        
        // �Q�[���I�[�o�[����
        if (damaged_hp <= 0f)
        {
            // �Ȃɂ�����̊֐����Ăяo���B

        }
    }

    private void GaugeReduction(float reducationValue, float time = 1f)
    {
        var playerStatus = masterData.PlayerStatus;

        var valueFrom = playerStatus.current_hp / playerStatus.max_hp;
        var valueTo = (playerStatus.current_hp - reducationValue) / playerStatus.max_hp;

        // �΃Q�[�W����
        hpBarGreen.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            // kill() �́@�r���Œ�~
            redGaugeTween.Kill();
        }


        // �ԃQ�[�W����
        redGaugeTween = DOTween.To(
            () => valueFrom,    // ����
            x => {         // ����
                if (hpBarRed)
                {hpBarRed.fillAmount = x;}
            },
            valueTo,        // �ڕW�l
            time    // �����鎞��
        );
    }
}
