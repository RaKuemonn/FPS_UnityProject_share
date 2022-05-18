using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // if (m_timer < 0)
        // {
        if (!m_gameObject)
        {
            Destroy(this.gameObject);
            return;
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
