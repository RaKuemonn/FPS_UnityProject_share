using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTimer_ChangeTexture : MonoBehaviour
{
    // 対象のシェーダーが適用されたGameObjectを設定する
    [SerializeField] 
    private GameObject obj; 

    Renderer r;

    // 初期値
    [SerializeField]
    float Initial_value = 2;
    // 終了値
    [SerializeField]
    float End_value = -1;

    // 消えるスピード
    [SerializeField]
    float LostSpeed = 0.5f;

    // 値
    float value;

    // ディゾルブ開始許可
    bool test = false;

    // ディゾルブ完了
    bool complete = false;

    // 経過時間を格納する変数
    float time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        // 値変更処理
        r = obj.GetComponent<Renderer>();
        // ディゾルブ以外のアルファ値を0にして非表示にする
        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);
        
        // 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        r.materials[r.materials.Length - 1].SetTexture("Main_Texture", r.materials[0].GetTexture("_BaseMap"));
        r.materials[r.materials.Length - 1].SetTexture("Normal", r.materials[0].GetTexture("_BumpMap"));
        
        value = Initial_value;
    }
    void Update()
    {
        // 間隔
        float duration = 1;

        // ゲーム時間
        time += Time.deltaTime;

        // ディゾルブ完了したら入らないように
        //if (r.materials[r.materials.Length - 1].GetFloat("TransparencyLevel") < End_value)
        if (complete == false)
        {
            // 生成されたらディゾルブ開始
            // 今は仮で2秒後に開始するようにしている
            if (time > 2)
            {
                test = true;
            }

            if (test)
            //if (time > 2)
            {
                // 計算
                value -= Time.deltaTime * LostSpeed;
                var t = value / duration;

                // ディゾルブ
                r.materials[r.materials.Length - 1].SetFloat("TransparencyLevel", t);

                // ディゾルブをかけ終わったらtrueに
                if (r.materials[r.materials.Length - 1].GetFloat("TransparencyLevel") < End_value)
                {
                    complete = true;
                    // 敵が鎌を投擲し始めていいように許可
                    // 投げる = true;
                }
            }
        }
    }
}
