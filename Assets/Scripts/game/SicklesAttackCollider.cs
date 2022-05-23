using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
class SicklesAttackCollider : MonoBehaviour
{
    private GameObject targetObject;    // この当たり判定オブジェクトの影響先

    private Camera mainCamera;          // カメラ情報の参照用？

    private RectTransform canvas_rect;  // 親オブジェクトの位置参照用

    private RectTransform rect;         // 自身の形の参照用

    private float damage_timer;         // 無敵時間管理用

    private GameObject player;          // プレイヤーの位置参照用 (距離を計算するため)

    [SerializeField]
    private float toleranceLevel; // カウンター成功範囲

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        rect = GetComponent<RectTransform>();

        canvas_rect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        player = GameObject.FindGameObjectWithTag("Player");
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
        rect.localScale = new Vector3(1f, 1f, 1f) * scale;


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

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damage_timer > 0f) return;

        // 持っているmaterialの色が赤ではなく緑になっていたら
        if (targetObject.GetComponent<Renderer>().material.color.r > 0f) return;

        var slash = collider.gameObject;
        if (slash.tag != "Slash") return;
        slash.GetComponent<BoxCollider2D>().enabled = false;

        Debug.Log("slash hit");



        // 無敵時間の設定
        damage_timer = 1.0f;
        // カーソルのゲームオブジェクトからコンポーネントを取得する。
        var cursor = GameObject.FindGameObjectWithTag("Cursor")
            .GetComponent<CursorController>();
        cursor.SetChainTimer();

        // カウンターの可否判別
        {
            // TODO 1: slash(画像)の角度(angle)を取得して、単位ベクトル(Vector2)を作る。              ( )
            var slash_ = GameObject.FindGameObjectWithTag("Slash");
            var slashImage = slash_.GetComponent<SlashImageController>();
            float slashAngle = slashImage.RadianAngle2D();
            Vector2 slashVec = MathHelpar.AngleToVector2(slashAngle);

            // TODO 2: 鎌の指定コマンド(Vector2)とslashのベクトル(Vector2)の角度を算出する。   ( float resutl_angle = Vector2.Angle(ベクトル1, ベクトル2); )
            var sickleControllr = GetComponent<SickleController>();
            float sickleAngle = sickleControllr.GetRadianSlashAngle();
            Vector2 sickleVec = MathHelpar.AngleToVector2(sickleAngle);

            // TODO 3: 角度が一定以内なら、カウンター成功にする。
            var dot = Vector2.Dot(slashVec, sickleVec);
            dot = Mathf.Acos(dot);
            if (toleranceLevel > dot && dot > -toleranceLevel)
            {
                Destroy(targetObject);
            }
            else
            {
                // TODO 米　失敗した時の処理を書け
            }
        }



        // TODO:切断する処理の追加(一旦削除してある)
        const float const_destroy_time = 0.5f;
        Destroy(targetObject, const_destroy_time);

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
}