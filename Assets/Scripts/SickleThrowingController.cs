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

    private float width = 1.0f;

    float rotationSpeed = 1080.0f;

    // プレイヤー高さ
    private float height = 2.0f;

    // 重力
    private float gravity = 3f;
    private Vector3 velocity = new Vector3( 0f, 0f, 0f );
    private float timerEasing = 0.0f;
    private Vector2 startPos;
    private Vector2 endPos;

    //[SerializeField]
    //private GameObject player;

    [SerializeField]
    private GameObject boss;

    private float slashAngle;
    public float GetRadianSlashAngle() { return slashAngle * Mathf.Deg2Rad; }
    public float GetSlashAbgle() { return slashAngle; }

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindWithTag("Player");
        target = g.transform.position;

        target.x += RandomTarget();

        target.y += 0.3f;
        startPos = transform.position;
        timerEasing = 0f;
        {
            startPos.x = transform.position.x;
            startPos.y = transform.position.z;

            endPos.x = g.transform.position.x;
            endPos.y = g.transform.position.z - 0;
        }

        slashAngle = Random.Range(0.0f, 360.0f);

        velocity = target - transform.position;
        velocity.Normalize();


        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        var rotate = transform.eulerAngles;
        var roll = rotate.z + rotationSpeed * Time.deltaTime;
        roll = rotationSpeed * Time.deltaTime;
        transform.localRotation *= Quaternion.Euler(0, 0, roll);


        var vel = velocity * (moveSpeed * Time.deltaTime);
        vel += transform.position;

        if (timer < 0) Destroy(gameObject);


        transform.position = new Vector3(vel.x, vel.y, vel.z);

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

    private float RandomTarget()
    {
        float haba = Random.Range(0, width * 2) - width;
        return haba;
    }
}
