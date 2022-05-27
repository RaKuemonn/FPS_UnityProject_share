using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackPrudir : MonoBehaviour
{
    public GameObject m_gameObject;

    private RectTransform m_parentUI;

    private RectTransform rect;

    float m_timer = 2.0f; // �䂤�����

    // Start is called before the first frame update
    void Start()
    {
        // �eUI��RectTransform��ێ�
        m_parentUI = transform.parent.GetComponent<RectTransform>();

        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // �J�����̊O�ɏo����\�����Ȃ�
        {
            var vector = m_gameObject.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
            var dot = Vector2.Dot(
                Camera.main.transform.forward.normalized,
                vector.normalized
                );


            if (dot < 0.3f)
            {
                var image = GetComponent<Image>();
                image.enabled = false;

                return;
            }
        }

        // �f�B�]���u(�������I���؂�����\������
        {
            var dissolve = m_gameObject.GetComponent<DissolveTimer_ChangeTexture>();

        }

        var mesh = m_gameObject.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        if (!mesh.enabled)
        {
            var image = GetComponent<Image>();
            image.enabled = false;
            return;
        }
        else
        {
            var image = GetComponent<Image>();
            image.enabled = true;
        }

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(m_gameObject.transform.position);
        
        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        //Debug.Log(uiLocalPos);

        // RectTransform�̃��[�J�����W���X�V
         rect.anchoredPosition = uiLocalPos;


        m_timer -= Time.deltaTime;
    }
}
