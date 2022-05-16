using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MessageRendererControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    [SerializeField] private TextDataTable textDataTable;

    public UnityEvent CallBackCondExprEndExplanation = new UnityEvent();

    private int read_page_count = 0; // 読んだページ数
    
    void Update()
    {
        // 作業途中 13:41から放置
        // ここでやりたいこと 現在のページのテキストを、textMeshProに一文字ずつ表示していく。
        // なにかしらボタンが押されたとき、ページ内のすべての文字を表示。もしくはつぎのページへ移動する。
        // 最後のページでぼたんが押されたとき、StopFloorのCondExprEndExplanationコンポーネントのコールバック関数を呼び出す
        
        // tutorial floor table data の床に追加する。
    }
}
