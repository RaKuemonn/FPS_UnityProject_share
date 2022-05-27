using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SickleController : BaseEnemy
{
    private float width = 1.5f;
    private Vector3 velocity = new Vector3(0f, 0f, 0f);
    private float updateTimer = 0.0f;
    float rotationSpeed = 1080.0f;
    private Tween tween;

    private Vector3 target;
    public float timer;
    public float moveSpeed;
    private Vector3 direction;

    private float startTimer = 2.0f;
    private int count = 0;

    private bool look = false;
    public void SetLook(bool set) { look = set; }

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    [SerializeField] public GameObject EffectCircle;

    private float slashAngle;
    public float GetRadianSlashAngle() { return slashAngle * Mathf.Deg2Rad; }
    public float GetSlashAngle() { return slashAngle; }

    public void DisableMesh() { updateTimer = -0.1f; /* Update()でmesh.enable = falseにしている */}

    // Start is called before the first frame update
    void Start()
    {
        var mesh = transform.Find("kama").GetComponent<SkinnedMeshRenderer>();
        mesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (updateTimer < 0)
        {
            var mesh = GetComponentInChildren<Renderer>();
            mesh.enabled = false;
            return;
        }

        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
            return;
        }

        //var 

        var rotate = transform.eulerAngles;
        var roll = rotate.z + rotationSpeed * Time.deltaTime;
        roll = rotationSpeed * Time.deltaTime;
        transform.localRotation *= Quaternion.Euler(0, 0, roll);


        var vel = velocity * (moveSpeed * Time.deltaTime);
        vel += transform.position;

        //if (timer < 0) Destroy(gameObject);


        transform.position = new Vector3(vel.x, vel.y, vel.z);
    
        //  else if (startTimer <= 0 && count == 0) 
        //   
        //      //transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
        //      direction = target - transform.position;
        //      direction.Normalize();
        //      var test = (target + transform.position) / 2;
        //      var vec = boss.transform.position - transform.position;
        //      float dot = (boss.transform.forward.x * vec.x) + (boss.transform.forward.z * vec.z);
        //      test.x += dot * 2;
        //      Vector3[] path =
        //      {
        //      transform.position,
        //      //test,
        //      //target,
        //      new Vector3( target.x, target.y, target.z + (-20) )
        //      };
        //      transform.DOLocalPath(path, 4.0f, PathType.CatmullRom)
        //          .SetEase(Ease.InExpo).SetLookAt(0.01f).SetOptions(false, AxisConstraint.Y);
        //      count++;
        //  }
        //  this.transform.DOMoveY(target.y, 4f).OnUpdate(() =>
        //  {
        //  });
        //  transform.DOLocalRotate(new Vector3(180, 90, 270f), 0.1f, RotateMode.FastBeyond360)
        //.SetEase(Ease.Linear)
        //.SetLoops(-1, LoopType.Restart);
        //  var ydown = transform.position;
        //  ydown.y -= moveSpeed * Time.deltaTime;
        //  //var dir = direction * moveSpeed * Time.deltaTime;
        //  //dir += transform.position;
        //  //transform.position = new Vector3(dir.x, dir.y, dir.z);
        //  transform.position = new Vector3(transform.position.x, ydown.y, transform.position.z);
        //  timer -= Time.deltaTime;
        updateTimer -= Time.deltaTime;
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
        float y = Random.Range(1, 3);
        float z = Random.Range(0, right.z * 2) - right.z;
        //Random.Range(zMinPosition, zMaxPosition);

        //Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }

    public void Initilize(float timer)
    {
        GameObject g = GameObject.FindWithTag("Player");
        target = g.transform.position;//GetRandomTarget();
        target.y += 1f;
        //target.y += 0.3f;
        transform.localScale = new Vector3(1, 1, 1);

        slashAngle = Random.Range(0.0f, 360.0f);

        velocity = target - transform.position;
        velocity.Normalize();

        updateTimer = 6.0f;

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);

        var mesh = transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        mesh.enabled = true;
        var Circle = EffectCircle;
        var Arrow = Circle.GetComponentInChildren<SlashDirectionController>().gameObject;

        //Circle.GetComponent<Image>().enabled = true;
        //Arrow.GetComponent<Image>().enabled = true;

        startTimer = timer;

        m_hp = 1;

        gameObject.GetComponent<DissolveTimer_ChangeTexture>()?.OnGenerate();


        //slashAngle = Random.Range(0.0f, 360.0f);
        //
        //look = true;
        //
        //target = GetRandomTarget();
        //
        //var mesh = transform.Find("kama").GetComponent<SkinnedMeshRenderer>();
        //mesh.enabled = true;
        //
        //startTimer = 2.0f;
        //count = 0;
    }   //

    protected override float DestroyTime()
    {
        return -1.0f; // マイナスだと破棄されない
    }

    protected override void CuttedImpulse(Vector3 impulse_)
    {
        // なにもしない
    }
}
