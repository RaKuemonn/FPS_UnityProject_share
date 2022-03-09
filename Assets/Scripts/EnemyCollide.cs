using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollide : MonoBehaviour
{
    // 生成時、登録する用
    private GameObject targetObject;

    private Camera mainCamera;

    private RectTransform canvas_rect;

    private RectTransform rect;

    private float damage_timer;

    private CircleCollider2D circle_collider;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        rect = GetComponent<RectTransform>();

        canvas_rect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        circle_collider = GetComponent<CircleCollider2D>();

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



        // 画像のスケールを変えると, 
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

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damage_timer > 0f) return;

        // 持っているmaterialの色が赤ではなく緑になっていたら
        if (targetObject.GetComponent<Renderer>().material.color.r > 0f) return;

        if (collider.gameObject.tag != "Slash") return;
        collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;


        Debug.Log("slash hit");

        damage_timer = 1.0f;

        // カーソル
        GameObject
            .FindGameObjectWithTag("Cursor")
            .GetComponent<CursorController>()
            .SetTimer();

        Destroy(targetObject);

        // 再生成
        float random_x = Random.Range(-2f, 2f);
        GameObject e = Instantiate((GameObject)Resources.Load("Enemy_copy"));
        e.transform.SetPositionAndRotation(
            GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(random_x, 1f, 5f),
            Quaternion.identity);
    }
}
