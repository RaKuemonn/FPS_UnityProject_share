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

    // 画面外判定用変数
    Rect screenRect = new Rect(0, 0, 1, 1); // 画面内か判定するためのRect

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
        //// カメラの外に出たら表示しない
        //if (CheckInScreen() == false)
        //{
        //    var image = GetComponent<Image>();
        //    image.enabled = false;
        //    Debug.Log("Screen Out");
        //
        //    return;
        //}

        // ディゾルブ(召喚が終わり切ったら表示する
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
