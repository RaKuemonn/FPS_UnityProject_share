using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public enum DamageEffectType
    {
        Dragon,
        Bat,
        Rabbit,
        Sickle,
    }

    [SerializeField] private EffectTable damegeEffectTable;
    [SerializeField] private Image DamageImage;
    
    // Update is called once per frame
    void Update()
    {
        if (DamageImage == null) return;

        var color = DamageImage.color;
        if (color.a > 0.1f)
        {
            float alpha = Mathf.Lerp(color.a, 0.0f, 10.0f * Time.deltaTime);
            DamageImage.color = new Color(color.r, color.g, color.b, alpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectRenderDamageEffect(DamageEffectType type_, Vector3 attacker_position)
    {
        // 表示するスプライトの選択
        switch (type_)
        {
            case DamageEffectType.Dragon:
                DamageImage.sprite = damegeEffectTable.effects[0]?.Sprite;
                DamageImage.rectTransform.sizeDelta = new Vector2(300f, 300f);
                break;
            case DamageEffectType.Bat:
                DamageImage.sprite = damegeEffectTable.effects[1]?.Sprite;
                DamageImage.rectTransform.sizeDelta = new Vector2(300f, 300f);
                break;
            case DamageEffectType.Rabbit:
                DamageImage.sprite = damegeEffectTable.effects[2]?.Sprite;
                DamageImage.rectTransform.sizeDelta = new Vector2(500f, 500f);
                break;
            case DamageEffectType.Sickle:
                DamageImage.sprite = damegeEffectTable.effects[3]?.Sprite;
                DamageImage.rectTransform.sizeDelta = new Vector2(300f, 300f);
                break;
        }

        if (attacker_position == null) return;

        
        var canvas = GameObject.Find("Canvas")?.transform;
        if (canvas == null) return;

        // 描画位置の設定
        var position = attacker_position;
        position.y += 1.0f; // 高さを微妙に上げる

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(position);
        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (canvas as RectTransform),
            targetScreenPos,
            null,
            out var uiLocalPos
        );
        // RectTransformのローカル座標を更新
        (transform as RectTransform).anchoredPosition = uiLocalPos;
    }
}
