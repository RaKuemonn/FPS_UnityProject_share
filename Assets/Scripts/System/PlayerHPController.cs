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
        /// 動作確認
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
        var damaged_hp = playerStatus.current_hp - damaged_; // ダメージを受けた体力値

        // ダメージ処理
        {
            var safety_damaged = (damaged_hp > 0f) ?
                damaged_ :                                  // そのままダメージを与える
                damaged_ - damaged_hp;                      // ０を下回らないダメージを与える

            // ダメージ計算
            playerStatus.current_hp -= safety_damaged;
            // 体力ゲージの更新
            GaugeReduction(safety_damaged);
        }
        
        // ゲームオーバー処理
        if (damaged_hp <= 0f)
        {
            // なにかしらの関数を呼び出す。

        }
    }

    private void GaugeReduction(float reducationValue, float time = 1f)
    {
        var playerStatus = masterData.PlayerStatus;

        var valueFrom = playerStatus.current_hp / playerStatus.max_hp;
        var valueTo = (playerStatus.current_hp - reducationValue) / playerStatus.max_hp;

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
                {hpBarRed.fillAmount = x;}
            },
            valueTo,        // 目標値
            time    // 書ける時間
        );
    }
}
