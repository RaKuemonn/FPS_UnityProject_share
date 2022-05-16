using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeScaleControl : MonoBehaviour
{
    [SerializeField] private float time = 1.0f;

    private Tween timeScaleTween;

    // ヒットストップこれやりたい https://qiita.com/flankids/items/75dd457463918f48b70f
    void OnHitStop()
    {
        
        const float min_rate = 0.0f;
        const float max_rate = 1.0f;

        timeScaleTween = DOTween
            .To(
                () => min_rate, // 最初
                x =>
                {
                    Time.timeScale = x;
                },
                max_rate, // 最後
                time //
            )
            .SetUpdate(true);   // Time.timescaleを無視して更新する

    }


}
