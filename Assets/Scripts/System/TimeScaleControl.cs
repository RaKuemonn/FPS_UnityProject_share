using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TimeScaleControl : MonoBehaviour
{
    [SerializeField] private float time = 1.0f;

    private Tween timeScaleTween;

    // �q�b�g�X�g�b�v�����肽�� https://qiita.com/flankids/items/75dd457463918f48b70f
    void OnHitStop()
    {
        
        const float min_rate = 0.0f;
        const float max_rate = 1.0f;

        timeScaleTween = DOTween
            .To(
                () => min_rate, // �ŏ�
                x =>
                {
                    Time.timeScale = x;
                },
                max_rate, // �Ō�
                time //
            )
            .SetUpdate(true);   // Time.timescale�𖳎����čX�V����

    }


}
