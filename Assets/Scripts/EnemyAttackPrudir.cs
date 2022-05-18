using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // if (m_timer < 0)
        // {
        if (!m_gameObject)
        {
            Destroy(this.gameObject);
            return;
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
