using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SickleController : MonoBehaviour
{
    private float width = 5.0f;

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
        var test = (target + transform.position) / 2;

        // test.x += 3;

        Vector3[] path =
        {
            transform.position,
            test,
            new Vector3 (10, -10 ,0),
            new Vector3(-10, -100,-10)
        };


        transform.DOLocalPath(path, 2.0f, PathType.CatmullRom)
            .SetEase(Ease.Linear).SetLookAt(0.01f).SetOptions(false, AxisConstraint.Y);

        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.DOMoveY(-0.5f, 4f).OnUpdate(() =>
        {

        });

        if (timer < 0) Destroy(gameObject);

        var ydown = transform.position;
        ydown.y -= moveSpeed * Time.deltaTime;
        //var dir = direction * moveSpeed * Time.deltaTime;
        //dir += transform.position;
        //transform.position = new Vector3(dir.x, dir.y, dir.z);
        transform.position = new Vector3(transform.position.x, ydown.y, transform.position.z);


        timer -= Time.deltaTime;

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
        float y = 0;// Random.Range(0, 5);
        float z = player.transform.position.z - 8;//Random.Range(zMinPosition, zMaxPosition);

        //Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }
}
