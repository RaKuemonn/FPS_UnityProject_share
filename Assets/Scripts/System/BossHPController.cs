using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BossHPController : MonoBehaviour
{
    [SerializeField] private GameObject hpGauge;

    [SerializeField] private EnemyBossController boss;

    private Image hpBarRed;
    private Image hpBarGreen;

    private Tween redGaugeTween;

    //private float timer = 0f;

    public void Start()
    {
        var redBar = hpGauge.transform.Find("GaugeFrame/RedGaugeBar")?.gameObject;
        var greenBar = hpGauge.transform.Find("GaugeFrame/GreenGaugeBar")?.gameObject;

        hpBarRed = redBar?.GetComponent<Image>();
        hpBarGreen = greenBar?.GetComponent<Image>();
    }

    public void OnDamaged(float damaged_)
    {
        var damaged_hp = boss.GetHP() - damaged_; // ダメージを受けた体力値

        // ダメージ処理
        {
            var safety_damaged = (damaged_hp > 0f) ?
                damaged_ :                                  // そのままダメージを与える
                damaged_ - damaged_hp;                      // ０を下回らないダメージを与える

            // ダメージ計算
            boss.SetHP(boss.GetHP() - safety_damaged);
            // 体力ゲージの更新
            GaugeReduction(safety_damaged);
        }
    }

    private void GaugeReduction(float reducationValue, float time = 1f)
    {

        var valueFrom = boss.GetHP() / boss.GetMaxHP();
        var valueTo = (boss.GetHP() - reducationValue) / boss.GetMaxHP();

        // 緑ゲージ減少
        hpBarGreen.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            // kill() は　途中で停止
            redGaugeTween.Kill();
        }


        // 赤ゲージ減少
        redGaugeTween = DOTween.To(
            () => valueFrom,    // 何に
            x => {         // 何を
                if (hpBarRed)
                { hpBarRed.fillAmount = x; }
            },
            valueTo,        // 目標値
            time    // 書ける時間
        );
    }
}
