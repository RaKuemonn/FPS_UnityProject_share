using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SickleController : MonoBehaviour
{
    private float width = 3.0f;

    private Tween tween;

    private Vector3 target;
    public float timer;
    public float moveSpeed;
    private Vector3 direction;

    private float startTimer = 2.0f;
    private int count = 0;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
    

        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            return;
        }
        else if (startTimer <= 0 && count == 0) 
         {
            target = GetRandomTarget();
            //transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
            direction = target - transform.position;
            direction.Normalize();
            var test = (target + transform.position) / 2;

            var vec = boss.transform.position - transform.position;
            float dot = (boss.transform.forward.x * vec.x) + (boss.transform.forward.z * vec.z);
            test.x += dot * 4;

            Vector3[] path =
            {
            transform.position,
            test,
            target,
            new Vector3( target.x, target.y, target.z - 13 )
            };


            transform.DOLocalPath(path, 3.0f, PathType.CatmullRom)
                .SetEase(Ease.InExpo).SetLookAt(0.01f).SetOptions(false, AxisConstraint.Y);

            count ++;
        }


        this.transform.DOMoveY(-0.1f, 4f).OnUpdate(() =>
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
        float x = Random.Range(0, right.x * 2)  - right.x;
        float y = Random.Range(0, 5);
        float z = player.transform.position.z ;//Random.Range(zMinPosition, zMaxPosition);

        //Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }
}
