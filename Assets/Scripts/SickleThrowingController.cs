using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SickleThrowingController : BaseEnemy
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

    private float updateTimer = 0.0f;

    [SerializeField]
    private GameObject boss;
    

    private float slashAngle;
    public float GetRadianSlashAngle() { return slashAngle * Mathf.Deg2Rad; }
    public float GetSlashAbgle() { return slashAngle; }
    public void DisableMesh() { updateTimer = -0.1f; /* Update()でmesh.enable = falseにしている */}

    // Start is called before the first frame update
    void Start()
    {
      
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

        var rotate = transform.eulerAngles;
        var roll = rotate.z + rotationSpeed * Time.deltaTime;
        roll = rotationSpeed * Time.deltaTime;
        transform.localRotation *= Quaternion.Euler(0, 0, roll);

        var vel = velocity * (moveSpeed * Time.deltaTime);
        vel += transform.position;

        transform.position = new Vector3(vel.x, vel.y, vel.z);

        timerEasing += Time.deltaTime;
        timer -= Time.deltaTime;

        updateTimer -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") == false) return;

        if (updateTimer < 0.0f) return;

        // プレイヤーにダメージを与える
        collider.gameObject
            .GetComponent<PlayerAutoControl>()
            .OnDamage(m_damage);

        var position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // プレイヤーの前方に位置を押し出している
        position += collider.gameObject.transform.forward * 2.0f;

        AttackEffect(DamageEffect.DamageEffectType.Sickle, position);

        DisableMesh();
    }

    private float RandomTarget()
    {
        float haba = Random.Range(0, width * 2) - width;
        return haba;
    }

    public void Initilize()
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

        updateTimer = 3.0f;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);

        var mesh = transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        mesh.enabled = true;
    }
}
