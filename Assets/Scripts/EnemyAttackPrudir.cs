using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackPrudir : MonoBehaviour
{
    public GameObject m_gameObject;

    private RectTransform m_parentUI;

    private RectTransform rect;

    float m_timer = 2.0f; // ゆうよ期間

    // Start is called before the first frame update
    void Start()
    {
        // 親UIのRectTransformを保持
        m_parentUI = transform.parent.GetComponent<RectTransform>();

        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
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

        {
            var dot = Vector2.Dot(Camera.main.transform.forward.normalized,
                GameObject.FindGameObjectWithTag("Player").transform.forward.normalized);


            if (dot < 0.0f)
            {
                var image = GetComponent<Image>();
                image.enabled = false;

                return;
            }
        }

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(m_gameObject.transform.position);
        
        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        //Debug.Log(uiLocalPos);

        // RectTransformのローカル座標を更新
         rect.anchoredPosition = uiLocalPos;


        m_timer -= Time.deltaTime;
    }
}
