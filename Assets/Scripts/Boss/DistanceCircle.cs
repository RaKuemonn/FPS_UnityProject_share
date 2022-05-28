using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCircle : MonoBehaviour
{
    [SerializeField] private PlayerAutoControl player;
    [SerializeField] private GameObject target;                 // �������̈ʒu�ɕ\�������
    [SerializeField] private Image Circle;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private PlayerStatus playerStatus;
    private bool cutted;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = new Vector3(
            target.transform.position.x,
            target.transform.position.y,
            target.transform.position.z);

    }

    void Update()
    {
        if (CheckDoRendering() == false)
        {
            // �`�悵�Ȃ�
            Circle.enabled = false;
            return;
        }

        if (cutted)
        {
            // �`�悵�Ȃ�
            Circle.enabled = false;
            return;
        }

        // �`�悷��
        Rendering();
    }

    float ToPlayerDistance()
    {
        return Vector3.Distance(
            player.transform.position,
            target.transform.position);
    }

    bool IsTherePlayerFront()
    {
        Rect screenRect = new Rect(0, 0, 1, 1);

        var viewportPos = Camera.main.WorldToViewportPoint(target.transform.position);
        if (screenRect.Contains(viewportPos) == false) return false;


        var vector = target.transform.position -
                     player.transform.position;

        var dot = Vector3.Dot(
            Camera.main.transform.forward.normalized,
            vector.normalized
        );

        return dot > 0.4f;
    }

    bool CheckDoRendering()
    {
        if (IsTherePlayerFront() == false) return false;

        return true;
    }

    void Rendering()
    {
        Circle.enabled = true;

        var position = target.transform.position;
        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(position);
        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            targetScreenPos,
            null,
            out var uiLocalPos
        );
        // RectTransform�̃��[�J�����W���X�V
        (transform as RectTransform).anchoredPosition = uiLocalPos;

        //var in_range = playerStatus.sword_area_radius;
        //var distance = ToPlayerDistance();
        //var max_distance = Vector3.Distance(
        //    player.transform.position,
        //    startPosition);
        //var rate = 1.0f - ((distance - in_range) / (max_distance - in_range)); 

        Circle.fillAmount = 1.0f;


    }

    public void Cut()
    {
        cutted = true;
    }
}
