using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTimerMaterialChange : MonoBehaviour
{
    [SerializeField] private GameObject obj; // 対象のシェーダーが適用されたGameObjectを設定する

    Renderer r;
    
    // 初期値
    [SerializeField]
    float initial_value = -2.0f;

    // 終了値
    [SerializeField]
    float End_value = 0.5f;

    // 消えるスピード
    [SerializeField]
    float LostSpeed = 0.75f;

    //方向逆転
    float sign;

    // 値
    float value;

    // テレポート開始許可
    bool test = false;

    // テレポート完了
    bool complete = false;

    // 経過時間を格納する変数
    float time = 0.0f; 


    // Start is called before the first frame update
    void Start()
    {
        // 値変更処理
        r = obj.GetComponent<Renderer>();

        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        r.materials[r.materials.Length - 1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        r.materials[r.materials.Length - 1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));


        sign = r.materials[r.materials.Length - 1].GetFloat("_sign");
        //方向逆転しているかどうか

        value = initial_value;
        test = false;
        time = 0.0f;
    }
    void Update()
    {
        // 間隔
        float duration = 1;

        time += Time.deltaTime;

        // ディゾルブ完了したら入らないように
        if (complete == false)
        {
            // 今は仮で2秒後に開始するようにしている
            if (time > 2 && test == false)
            {
                test = true;
            }


            if (test)
            {
                //方向逆転してないとき
                if (sign == 0)
                {
                    value -= Time.deltaTime * LostSpeed;
                    var t = value / duration;
                    for (int i = 0; i < r.materials.Length; i++)
                    {
                        r.materials[i].SetFloat("_Slider", t);
                    }
                }
                //方向逆転してるとき
                else if (sign == 1)
                {
                    value += Time.deltaTime * LostSpeed;
                    var t = value / duration;

                    for (int i = 0; i < r.materials.Length; i++)
                    {
                        r.materials[i].SetFloat("_Slider", t);
                    }
                }

                // ディゾルブをかけ終わったらtrueに
                if (r.materials[r.materials.Length - 1].GetFloat("_Slider") > End_value)
                {
                    complete = true;
                    // 敵が鎌を投擲し始めていいように許可
                    // 投げる = true;
                }
            }
        }
    }
}
