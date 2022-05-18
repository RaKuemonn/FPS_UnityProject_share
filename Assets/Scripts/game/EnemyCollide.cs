using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyCollide : MonoBehaviour
{
    [SerializeField] private float impulse_power;   // 斬った時の衝撃力

    private GameObject targetObject;    // この当たり判定オブジェクトの影響先

    private Camera mainCamera;          // カメラ情報の参照用？

    private RectTransform canvas_rect;  // 親オブジェクトの位置参照用

    private RectTransform rect;         // 自身の形の参照用

    private float damage_timer;         // 無敵時間管理用
    
    private GameObject player;          // プレイヤーの位置参照用 (距離を計算するため)

    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        rect = GetComponent<RectTransform>();

        canvas_rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        playerStatus = GameObject.FindGameObjectWithTag("SceneSystem").GetComponent<MasterData>().PlayerStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (damage_timer > 0f)
        {
            damage_timer += -Time.deltaTime;

            if (damage_timer < 0f)
            {
                damage_timer = 0f;
            }
        }


        if (CheckTarget()) return;

        UpdatePosition();

        UpdateCollisionCircleSize();
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    private void UpdatePosition()
    {
        if (targetObject == null) return;

        var screen_position = mainCamera.WorldToScreenPoint(
            targetObject.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas_rect,
            screen_position,
            null,
            out var canvas_position);

        rect.anchoredPosition = canvas_position;

    }

    private void UpdateCollisionCircleSize()
    {
        if (player == null) return;


        var distance = Vector3.Distance(player.transform.position, targetObject.transform.position);

        var scale = -0.2f * distance + 3f;
        //var scale = 3.5f + 0.3f * distance;



        // 画像のスケールを変えると, 当たり判定もついてくるよ
        rect.localScale = new Vector3(1f,1f,1f) * scale;


    }

    private bool CheckTarget()
    {
        // 参照している敵が消されたら
        if (targetObject) return false;

        //Debug.Log("delete enemy collide");

        // 自分自身も消す
        Destroy(gameObject);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damage_timer > 0f) return;

        // 敵当たり判定に当たったオブジェクトは Slashか
        if (collider.tag != "Slash") return;
        
        // 間合いに入っているか
        if (InSwordArea() == false) return;

        // Slashの当たり判定がこれ以上効かないようにする
        collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;



#if UNITY_EDITOR
        Debug.Log("slash hit");
#endif


        // 無敵時間の設定
        damage_timer = 1.0f;

        // カーソルの
        var cursor = GameObject.FindGameObjectWithTag("Cursor")
            .GetComponent<CursorController>();

        cursor.SetChainTimer();
        

        // 切断する処理   TODO : EnemyControllerコンポーネント内に記述を移すのもあり。
        if(false/* 条件式を記述　例:体力が0以下ならとか */)
        {

            Vector3 normal;
            {
                var slash_ray =
                    collider.gameObject.
                        GetComponent<SlashImageController>().
                        SlashRay();
                
                var cursol_ray = cursor.CursolRay();


                const float distance = 5.0f;
                var origin_position = mainCamera.transform.position;
                var cursol_far = cursol_ray.GetPoint(distance);
                var slash_far = slash_ray.GetPoint(distance);

                var a = slash_far  - origin_position;
                var b = cursol_far - origin_position;

                normal = Vector3.Cross(a, b).normalized;
            }

            var result =
                MeshCut.CutMesh(
                    targetObject,                                   // 斬るオブジェクト
                    mainCamera.transform.position,    // 平面上の位置
                    normal                                          // 平面の法線
                    );

            // TODO : 斬った時にはねるようにしたい
            var impulse_copy = normal;

            // 斬ったオブジェクトの事後処理、削除予約
            ObjectCutted(result.copy_normalside, false, impulse_copy);
            ObjectCutted(result.original_anitiNormalside, true, impulse_copy * -1.0f);
        }
        else
        {
            // 下のObjectCutted関数からコピペしたテスト用処理
            // ScarecrowはEnemyControllerを持たない
            //targetObject.GetComponent<EnemyController>().SetCutPerformance(false, new Vector3(0f,0f,0f));

            
            // 衝撃を与える処理
            {
                var impulse = cursor.CursolRay().direction * impulse_power;

                impulse.y += impulse_power; // 上方向に微妙に力を加える

                targetObject.GetComponent<BaseEnemy>().OnCutted(impulse);
            }

            const float const_destroy_time = 0.5f;
            Destroy(targetObject, const_destroy_time);
        }
        
    }

    private void ObjectCutted(GameObject object_, bool is_, Vector3 impulse_direction_)
    {
        if (object_ == null) return;

        var impulse_ = impulse_direction_ * 2.0f;
        impulse_.y += 0.1f;                          // 跳ねさせる
        object_.GetComponent<EnemyController>().SetCutPerformance(is_, impulse_);
        
        const float const_destroy_time = 0.5f;
        Destroy(object_, const_destroy_time);

    }

    private bool InSwordArea()
    {
        var distance = Vector3.Distance(player.transform.position, targetObject.transform.position);
        var sword_area_radius = playerStatus.sword_area_radius;

        return (distance <= sword_area_radius);
    }
}
