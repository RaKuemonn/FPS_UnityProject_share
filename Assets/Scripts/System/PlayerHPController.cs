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
        
        // ダメージ処理
        {
            var damaged_hp = playerStatus.current_hp - damaged_; // ダメージを受けた体力値

            var safety_damaged = (damaged_hp >= 0f) ?
                damaged_ :                                  // そのままダメージを与える
                damaged_ + damaged_hp;                      // ０を下回らないダメージに変更する

            // 体力ゲージの更新
            GaugeReduction(safety_damaged);

            // ダメージ計算
            playerStatus.current_hp += -safety_damaged;
        }
        
        // ゲームオーバー処理
        if (playerStatus.current_hp <= 0f + float.Epsilon * 2.0f)
        {
            // なにかしらの関数を呼び出す。
            game_over_?.Invoke();
        }
    }

    private void GaugeReduction(float reducationValue, float time = 1f)
    {
        var playerStatus = masterData.PlayerStatus;

        var valueFrom = playerStatus.current_hp / playerStatus.max_hp;
        var valueTo = (playerStatus.current_hp - reducationValue) / playerStatus.max_hp;

        // 緑ゲージ減少
        hpBarGreen.fillAmount = valueTo;

        timer = time;
        if (redGaugeTween != null)
        {
            // kill() は　途中で停止
            redGaugeTween.Kill();
            Timer.Kill();
        }

        // 赤ゲージ減少のスタート時間タイマー
        Timer = DOTween.To(
            () => 0.0f,    // 何に
            x => {         // 何を

                if (x < 1.0f) return;

                // 赤ゲージ減少
                redGaugeTween = DOTween.To(
                    () => valueFrom,    // 何に
                    g => {         // 何を
                        if (hpBarRed)
                        { hpBarRed.fillAmount = g; }
                    },
                    valueTo,        // 目標値
                    time    // 書ける時間
                );

            },
            1.0f,        // 目標値
            time    // 書ける時間
        );
    }
}
