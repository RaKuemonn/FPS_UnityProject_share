using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleController : MonoBehaviour
{
    private float width = 3.0f;

    private Vector3 target;
    public float timer;
    public float moveSpeed;
    private Vector3 direction;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        target = GetRandomTarget();
        //transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        direction = target - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0) Destroy(gameObject);

        var dir = direction * moveSpeed * Time.deltaTime;
        dir += transform.position;
        transform.position = new Vector3(dir.x, dir.y, dir.z);

        timer -= Time.deltaTime;

        Debug.Log(transform.position);
    }

    // 鎌を投げる位置をランダムにする
    public Vector3 GetRandomTarget()
    {
        var forward = player.transform.forward;
        Vector3 up = new Vector3(0, 1, 0);
        var right = Vector3.Cross(forward, up);
        right.Normalize();
        right *= width;

        //それぞれの座標をランダムに生成する
        float x = Random.Range(-right.x, right.z);
        float y = Random.Range(0, 5);
        float z = player.transform.position.z;//Random.Range(zMinPosition, zMaxPosition);

        //Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }
}
