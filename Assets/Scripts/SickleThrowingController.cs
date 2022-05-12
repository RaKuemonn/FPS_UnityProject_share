using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SickleThrowingController : MonoBehaviour
{
    private Vector3 target;
    public float timer;
    public float moveSpeed;
    private Vector3 direction;

    // プレイヤー高さ
    private float height = 2.0f;

    // 重力
    private float gravity = 3f;
    private Vector3 velocity = new Vector3( 0f, 0f, 0f );
    private float timerEasing = 0.0f;
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    private float slashAngle;
    public float GetRadianSlashAngle() { return slashAngle * Mathf.Deg2Rad; }
    public float GetSlashAbgle() { return slashAngle; }

    // Start is called before the first frame update
    void Start()
    {
        target = player.transform.position;
        target.y += height;
        velocity.y = 5.0f;
        startPos = transform.position;
        timerEasing = 0f;
        {
            startPos.x = transform.position.x;
            startPos.y = transform.position.z;

            endPos.x = player.transform.position.x;
            endPos.y = player.transform.position.z - 0;
        }

        slashAngle = Random.Range(0.0f, 360.0f);
        //transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position);

        velocity.y -= gravity * Time.deltaTime;

        if (timer < 0) Destroy(gameObject);

        var newpos = transform.position + velocity * Time.deltaTime;

        Vector2 xzPos = Easing.SineInOutV2(timerEasing / 1.0f, 5.0f, startPos, endPos);

        transform.position = new Vector3(xzPos.x, newpos.y, xzPos.y);

        //
        //var ydown = transform.position;
        //ydown.y -= moveSpeed * Time.deltaTime;
        //
        //
        //
        ////var dir = direction * moveSpeed * Time.deltaTime;
        ////dir += transform.position;
        ////transform.position = new Vector3(dir.x, dir.y, dir.z);
        //transform.position = new Vector3(transform.position.x, ydown.y, transform.position.z);
        //
        //
        timerEasing += Time.deltaTime;
        timer -= Time.deltaTime;

    }
}
