using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float move_speed;                // 移動スピード (公開しているので、Inspectorで変更可能)

    private GameObject player;              // プレイヤーの位置参照用

    private const float destroy_distance = 1f - float.Epsilon;

    private bool is_create_collide = true;  // 敵生成時に、当たり判定用のEnemyCollideOnScreenを生成するのか。 

    private bool is_cutted = false;         // 斬られているのか。 斬られていたら通常更新はしない。

    private bool is_original = true;        // 斬られたときにあたらしく生成された方か、元のものか判断するよう


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        CreateCollideOnCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_cutted) return;

        UpdatePosition();
        CollidePlayer();
    }

    public void SetCutPerformance(bool is_original_)
    {
        var rigid = transform.gameObject.AddComponent<Rigidbody>();
        is_cutted = true;
        is_create_collide = false;
        is_original = is_original_;

        float x = !is_original_ ? 1f : 0f;
        rigid.AddForce(new Vector3(x, 0.1f, 0.0f), ForceMode.Impulse);
    }

    private void UpdatePosition()
    {
        if (player == null) return;

        var to_player_vec = PlayerPosition() - transform.position;

        var to_player_dir = to_player_vec.normalized;
        transform.Translate(to_player_dir * move_speed * Time.deltaTime);
    }

    private void CollidePlayer()
    {
        var distance = Vector3.Distance(PlayerPosition(), transform.position);

        if (distance > destroy_distance) return;

        // それより近ければ
        Destroy(gameObject);

        //// 再生成
        //GameObject obj = Instantiate((GameObject)Resources.Load("Enemy_copy"));
        ////obj.transform.Translate();
        //obj.transform.SetPositionAndRotation(PlayerPosition() + new Vector3(0f, 1f, 10f), Quaternion.identity);
        Debug.Log("enemy die");
    }

    private Vector3 PlayerPosition()
    {
        if (player)
        {
            var p_pos = player.transform.position;
            return new Vector3(p_pos.x, p_pos.y + 0.5f, p_pos.z);
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }

    }

    private void CreateCollideOnCanvas()
    {
        // 斬られたときに生成されたオブジェクトでなければ（SetCutPerformance()で、変更されていなければ）
        if (is_create_collide == false) return;

        // 当たり判定用のオブジェクトをCanvas下に生成
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }

    void OnDestroy()
    {

        if (isQuitting) return;

        if (is_original == false) return;

        // 再生成
        float random_x = Random.Range(-2f, 2f);

        GameObject e = Instantiate((GameObject)Resources.Load("Enemy_test"));
        e.transform.SetPositionAndRotation(
            GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(random_x, 1f, 5f),
            Quaternion.identity);
    }

    private static bool isQuitting = false;
    void OnApplicationQuit()
    {

        isQuitting = true;

    }
}
