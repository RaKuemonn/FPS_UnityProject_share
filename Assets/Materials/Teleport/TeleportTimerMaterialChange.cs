using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTimerMaterialChange : MonoBehaviour
{
    [SerializeField] private GameObject obj; // 対象のシェーダーが適用されたGameObjectを設定する

    // updateで使うためにコメント解除 黒マテリアル入れるときはだめだっけ？
    Renderer r;
    // インスペクター側からテレポートマテリアルの追加する
    [SerializeField]
    Material[] materials = new Material[1];
    //Material[] materials = new Material[2];
    //GMaterial[] materials = new Material[3];
    
    // 初期値
    [SerializeField]
    float[] initial_value = { -2.0f, 0.5f};

    // 終了値
    [SerializeField]
    float[] End_value = { 0.5f, -0.5f};

    // 消えるスピード
    [SerializeField]
    float[] LostSpeed = { 2.0f, 0.5f };

    //方向逆転
    float sign;

    // 値
    float[] value = {0,0};

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
        //r = obj.GetComponent<Renderer>();
        

        // OnDead関数に移動
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        //r.materials[r.materials.Length - 1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        //r.materials[r.materials.Length - 1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //sign = r.materials[r.materials.Length - 1].GetFloat("_sign");



        //方向逆転しているかどうか

        //value = initial_value;
        //test = false;
        //time = 0.0f;
    }
    void Update()
    {
        // 間隔
        float duration = 1;

        time += Time.deltaTime;

        // テレポート完了したら入らないように
        if (complete == false)
        {
            // 今は仮で2秒後に開始するようにしている
            //if (time > 2 && test == false)
            //{
            //    test = true;
            //}


            if (test)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
                    //方向逆転してないとき
                    if (r.materials[i].GetFloat("_sign") == 0)
                    {
                        value[i] -= Time.deltaTime * LostSpeed[i];
                        var t = value[i] / duration;
                        //for (int i = 0; i < materials.Length; i++)
                        //{
                        //    materials[i].SetFloat("_Slider", t);
                        //}
                        //for (int i = 0; i < r.materials.Length; i++)
                        {
                            // 見た目に関わるのは1つだけなので0でいい
                            r.materials[i].SetFloat("_Slider", t);
                        }
                    }
                    //方向逆転してるとき
                    else if (r.materials[i].GetFloat("_sign") == 1)
                    {
                        value[i] += Time.deltaTime * LostSpeed[i];
                        var t = value[i] / duration;

                        //for (int i = 0; i < materials.Length; i++)
                        //{
                        //    materials[i].SetFloat("_Slider", t);
                        //}
                        //for (int i = 0; i < r.materials.Length; i++)
                        {
                            // 見た目に関わるのは1つだけなので0でいい
                            r.materials[i].SetFloat("_Slider", t);
                        }
                    }

                    // テレポートをかけ終わったらtrueに
                    //if (materials[materials.Length - 1].GetFloat("_Slider") > End_value)
                    if (r.materials[0].GetFloat("_Slider") > End_value[0] && r.materials[0].GetFloat("_Slider") < End_value[1])
                    {
                        complete = true;
                        // 消去許可
                        // 投げる = true;
                    }
                }
            }
        }
    }

    public void OnDead()
    {
        test = true;

        //var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        // 
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);
        //
        //// 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        //r.materials[1].SetTexture("_MainTex",
        //    r.materials[0].GetTexture("_BaseMap"));
        //r.materials[1].SetTexture("_BumpMap",
        //    r.materials[0].GetTexture("_BumpMap"));
        //
        //sign = r.materials[1].GetFloat("_sign");
        //
        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[2];
        //
        //
        ////r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);


#if false

        // SlashHighを使って　成功
        // materialsも3に変える

        var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        r.materials[1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        r.materials[1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        sign = r.materials[1].GetFloat("_sign");


        materials[0] = r.sharedMaterials[1];
        materials[1] = r.sharedMaterials[1];

        value = initial_value;

#else
        //var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        //// 0番目に入ってるマテリアルのalbedoとnormalをディゾルブマテリアルにコピーする
        //r.materials[1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        //r.materials[1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //sign = r.materials[1].GetFloat("_sign");

        //r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);

        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[1];
        //materials[2] = r.sharedMaterials[2];

        // ここで末尾(2番目、 r.materials[1])に黒マテリアルが追加される
        r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();

        // r.materials[0]を使うのでコメントアウト
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g,r.materials[0].color.b, 0);

        // 0番目に入ってるマテリアルのalbedoとnormaをテレポートマテリアルにコピーする
        materials[0].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        materials[0].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //テレポートマテリアルの情報から方向逆転しているか取得
        //r.materials[1].SetFloat("_sign", materials[0].GetFloat("_sign"));

        //r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);
        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[1];
        //materials[2] = r.sharedMaterials[2];

        // 見た目を担うr.materials[0]にテクスチャをコピーしてあるテレポートマテリアルを入れる
        r.materials[0].shader = materials[0].shader;
        r.materials[0].CopyPropertiesFromMaterial(materials[0]);

        //r.materials[1].SetFloat("_sign", 1);
        value[0] = initial_value[0];
        value[1] = initial_value[1];
#endif
    }
}
