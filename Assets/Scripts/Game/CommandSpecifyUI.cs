using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSpecifyUI : MonoBehaviour
{
    [SerializeField] private PlayerAutoControl player;
    [SerializeField] private GameObject target;                 // �������̈ʒu�ɕ\�������
    [SerializeField] private GameObject Arrow;
    [SerializeField] private Image Frame;
    [SerializeField] private Image Circle;
    [SerializeField] private float rendering_radius_to_target;
    [SerializeField] private float degree_z;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private float invisible_time;
    [SerializeField] private float offset_y = 1.5f;
    private bool cutted;
    private float timer;

    void Start()
    {
        // �����_���ł͂Ȃ�target�̎����Ă�euler�ɂ���
        Arrow.transform.eulerAngles =
            new Vector3(0f, 0f, degree_z);

    }

    void Update()
    {
        if (CheckDoRendering() == false)
        {
            // �`�悵�Ȃ�
            Arrow.GetComponent<Image>().enabled = false;
            Frame.enabled = false;
            Circle.enabled = false;
            return;
        }

        // �`�悷��
        if (cutted)
        {
            CuttedRendering();
            return;
        }

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
        if (target == null) return false;

        if (ToPlayerDistance() > rendering_radius_to_target) return false;
        if (IsTherePlayerFront() == false) return false;

        return true;
    }

    void Rendering()
    {
        Arrow.GetComponent<Image>().enabled = true;
        Frame.enabled = true;
        Circle.enabled = true;

        var position = target.transform.position;
        position.y += offset_y;
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

    }

    private void CuttedRendering()
    {
        timer += Time.deltaTime;

        if (timer >= invisible_time)
        {
            // �`�悵�Ȃ�
            Arrow.GetComponent<Image>().enabled = false;
            Frame.enabled = false;
            Circle.enabled = false;
            return;
        }

        var rate = 1.0f - timer / invisible_time;
        Arrow.GetComponent<Image>().color = new Color(0f,0f,0f, rate);
        Frame.color = new Color(0f, 0f, 0f, rate);
        Circle.color = new Color(0f, 0f, 0f, rate);

        // �`�悷��
        Rendering();

    }

    public void Cut()
    {
        cutted = true;
    }
}
