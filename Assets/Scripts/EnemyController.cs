using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float move_speed;

    private GameObject player;

    private float destroy_distance = 1f - float.Epsilon;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        UpdatePosition();

        CollidePlayer();
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

        // ÇªÇÍÇÊÇËãﬂÇØÇÍÇŒ
        Destroy(gameObject);

        // çƒê∂ê¨
        GameObject obj = Instantiate((GameObject)Resources.Load("Enemy"));
        //obj.transform.Translate();
        obj.transform.SetPositionAndRotation(PlayerPosition() + new Vector3(0f, 1f, 10f), Quaternion.identity);
        Debug.Log("enemy die");
    }

    private Vector3 PlayerPosition()
    {
        var p_pos = player.transform.position;
        return new Vector3(p_pos.x, p_pos.y + 0.5f, p_pos.z);
    }
}
