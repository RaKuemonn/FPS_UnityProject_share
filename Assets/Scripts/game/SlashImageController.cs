using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SlashImageController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private Image image;

    [SerializeField] private Material cutSurfaceMaterial = null;

    public float damage;    // Slashの生成時にCursorControllerで設定される。

    [SerializeField] private RectTransform child_rect_transform;

    [SerializeField] private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    { 
        // 音を再生
        GetComponent<AudioSource>().PlayOneShot(clip);


        var color = GetComponent<Image>().color;
        image = GetComponent<Image>();
        image.color = new Color(color.r, color.g, color.b, 225.0f);
        //Debug.Log(gameObject.GetComponent<BoxCollider2D>().size);

        // 力技 power
        {
            var rectTransform = GetComponent<RectTransform>();
            var image_screen_position = rectTransform.position;
            var image_width = rectTransform.sizeDelta.x * rectTransform.lossyScale.x;                                        // 前提:スケールが全て1.0f
            var image_radian = rectTransform.eulerAngles.z * Mathf.Deg2Rad;
            //var input_direction = new Vector2(Mathf.Cos(image_radian), Mathf.Sin(image_radian));

            var width_half = image_width / 2f;
            var width_div5 = image_width / 5f;

            // 2D回転関数 ※ラムダ式で書いただけ 
            Func<Vector2, float, Vector2> Rotate = (Vector2 vector_, float radian_) =>
            {
                return new Vector2(
                    vector_.x * Mathf.Cos(radian_) - vector_.y * Mathf.Sin(radian_),
                    vector_.x * Mathf.Sin(radian_) + vector_.y * Mathf.Cos(radian_)
                );
            };

            // 傾いたベクトル (スクリーン上でのRayの発射位置計算用)
            var vectors = new Vector2[] // size 6
            {
                Rotate(new Vector2(-width_half + width_div5 * 0f, 0f), image_radian),
                Rotate(new Vector2(-width_half + width_div5 * 1f, 0f), image_radian),
                Rotate(new Vector2(-width_half + width_div5 * 2f, 0f), image_radian),
                Rotate(new Vector2(-width_half + width_div5 * 3f, 0f), image_radian),
                Rotate(new Vector2(-width_half + width_div5 * 4f, 0f), image_radian),
                Rotate(new Vector2(-width_half + width_div5 * 5f, 0f), image_radian),   // 0~5
            };

            //  (たぶんできた)　傾きに合わせた位置にする必要がある
            var screen_positions = new Vector2[] // size 6
            {
                new Vector2(image_screen_position.x + vectors[0].x, image_screen_position.y + vectors[0].y),
                new Vector2(image_screen_position.x + vectors[1].x, image_screen_position.y + vectors[1].y),
                new Vector2(image_screen_position.x + vectors[2].x, image_screen_position.y + vectors[2].y),
                new Vector2(image_screen_position.x + vectors[3].x, image_screen_position.y + vectors[3].y),
                new Vector2(image_screen_position.x + vectors[4].x, image_screen_position.y + vectors[4].y),
                new Vector2(image_screen_position.x + vectors[5].x, image_screen_position.y + vectors[5].y),   // 0~5
            };

            // 発射するRay達
            var rays = new Ray[] // size 6
            {
                Camera.main.ScreenPointToRay(screen_positions[0]),
                Camera.main.ScreenPointToRay(screen_positions[1]),
                Camera.main.ScreenPointToRay(screen_positions[2]),
                Camera.main.ScreenPointToRay(screen_positions[3]),
                Camera.main.ScreenPointToRay(screen_positions[4]),
                Camera.main.ScreenPointToRay(screen_positions[5]),  // 0~5
            };

            Collider result_hit_collider = null;           // 最終的に使用する当たり判定の結果
            Ray result_hit_ray = new Ray();
            // 当たり判定処理
            {
                float hit_distance = float.MaxValue;

                foreach (var ray in rays)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, playerStatus.sword_area_radius) /* Rayを投射 */ == false)
                    {
#if UNITY_EDITOR
                        Debug.Log("Miss");
#endif
                        continue;
                    }


#if UNITY_EDITOR
                    Debug.Log("CurrentHit:" + hit.collider?.gameObject?.name);
#endif


                    // Tagが敵
                    if (hit.collider?.CompareTag("Enemy") == false) continue;

                    // 距離がhit_distanceより長く、遠い場所にある。
                    if (hit.distance > hit_distance) continue;

                    // 最短結果の敵
                    result_hit_collider = hit.collider;
                    hit_distance = hit.distance;
                    result_hit_ray = ray;
                }
            }


            // 当たっていなければ早期returnさせる
            if (result_hit_collider == null) return;


#if UNITY_EDITOR
            foreach (var ray in rays)
            {
                Debug.DrawRay(ray.origin, ray.direction);
            }
#endif


            // 当たっている敵に処理を行う
            BaseEnemy enemy = result_hit_collider.gameObject.GetComponent<BaseEnemy>();

            var current_enemy_hp = enemy.GetHP() - damage;

            // 体力があれば
            if (current_enemy_hp > 0f)
            {
                // ダメージを与える処理
                // (多分関数呼び出しか、コールバックさせる。)
                enemy.SetHP(current_enemy_hp);


            }
            // 体力がなければ
            else
            {
                enemy.OnDead();
                enemy.SetHP(0f);

                // 切断処理
                {
                    Vector3 normal;
                    {
                        const float distance = 5.0f;
                        var origin_position = rays[0].origin;
                        var far_left = rays[0].GetPoint(distance);
                        var far_right = rays[1].GetPoint(distance);

                        var left = far_left - origin_position;
                        var right = far_right - origin_position;

                        normal = Vector3.Cross(left, right).normalized;
                    }

                    var result =
                        MeshCut.CutMesh(
                            enemy.gameObject,                                          // 斬るオブジェクト
                            Camera.main.transform.position,   // 平面上の位置
                            normal,                                          // 平面の法線
                            true,
                            cutSurfaceMaterial
                        );

                    var original = result.original_anitiNormalside;
                    var copy = result.copy_normalside;



                    Action<GameObject, Vector3> Cutted = (GameObject object_, Vector3 normal_direction_) =>
                    {

#if UNITY_EDITOR
                        if (object_ == null)
                        {
                            Debug.Log("nulllllllllllllllllllllllll");
                        }
#endif
                        // 死亡処理
                        const float impulse_power = 8f;
                        var impulse = (result_hit_ray.direction + normal_direction_) * impulse_power;

                        object_?.GetComponent<BaseEnemy>().OnCutted(impulse);
                    };

                    Cutted.Invoke(original, -1.0f * normal);
                    Cutted.Invoke(copy, normal);

                }

            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {

        if (image == null) return;
        
        var color   = image.color;
        if (color.a > 0.1f)
        {
            float alpha = Mathf.Lerp(color.a, 0.0f, 10.0f * Time.deltaTime);
            image.color = new Color(color.r, color.g, color.b, alpha);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public Ray SlashRay()
    {
        return Camera.main.ScreenPointToRay(child_rect_transform.position);
    }

    public float RadianAngle2D()
    {
        return image.rectTransform.eulerAngles.z * Mathf.Deg2Rad;
    }

}
