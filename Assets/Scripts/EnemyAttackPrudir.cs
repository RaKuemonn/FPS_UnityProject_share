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

    // ��ʊO����p�ϐ�
    Rect screenRect = new Rect(0, 0, 1, 1); // ��ʓ������肷�邽�߂�Rect

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
        //// �J�����̊O�ɏo����\�����Ȃ�
        //if (CheckInScreen() == false)
        //{
        //    var image = GetComponent<Image>();
        //    image.enabled = false;
        //    Debug.Log("Screen Out");
        //
        //    return;
        //}

        // �f�B�]���u(�������I���؂�����\������
        {
            var dissolve = m_gameObject.GetComponent<DissolveTimer_ChangeTexture>();
            var image = GetComponent<Image>();
            if (dissolve)
            {
                if (dissolve.GetComplete())
                {
                    Debug.Log("Screen In , do dissolved");
                    image.enabled = true;
                }
                else
                {

                    Debug.Log("Screen In , not dissolved");
                    image.enabled = false;
                    return;
                }
            }
            
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
    Vector3 GetViewportPos(Vector3 targetPos)
    {
        return Camera.main.ViewportToWorldPoint(targetPos);
    }
    bool CheckInScreen()
    {
        var viewportPos = Camera.main.WorldToViewportPoint(m_gameObject.transform.position);
        if (screenRect.Contains(viewportPos) == false) return false;


        var vector = m_gameObject.transform.position -
                     GameObject.FindGameObjectWithTag("Player").transform.position;

        var dot = Vector2.Dot(
            Camera.main.transform.forward.normalized,
            vector.normalized
        );

        return dot > 0.3f;
    }

    public void Invisible()
    {
        GetComponent<Image>()
            .enabled = false;

        GetComponentInChildren<SlashDirectionController>()
            .gameObject
            .GetComponent<Image>()
            .enabled = false;

    }




}
