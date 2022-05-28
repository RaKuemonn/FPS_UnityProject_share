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
        // �\������X�v���C�g�̑I��
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

        // �`��ʒu�̐ݒ�
        var position = attacker_position;
        position.y += 1.0f; // ����������ɏグ��

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(position);
        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (canvas as RectTransform),
            targetScreenPos,
            null,
            out var uiLocalPos
        );
        // RectTransform�̃��[�J�����W���X�V
        (transform as RectTransform).anchoredPosition = uiLocalPos;
    }
}
