using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitThrowingController : BaseEnemy
{
    public PlayerAutoControl PlayerAutoControl;
    public Vector3 StartPosition;

    //private Rigidbody rigidbody;

    public float arrive_time;   // ThrowingStartAreaControlで設定
    private float timer = 0f;

    
    void Update()
    {
        if (IsDeath)
        {
            return;
        }

        TranslatePosition();

        timer += Time.deltaTime;
    }

    void TranslatePosition()
    {
        var rate = timer / arrive_time;

        if (rate > 1f)
        {
            rate = 1f;
        }

        transform.position = new Vector3(
            Mathf.Lerp(StartPosition.x, PlayerAutoControl.transform.position.x, rate),
            Mathf.Lerp(StartPosition.y, PlayerAutoControl.transform.position.y, rate),
            Mathf.Lerp(StartPosition.z, PlayerAutoControl.transform.position.z, rate));

    }

    public override void OnDead()
    {
        base.OnDead();
        GetComponent<Collider>().enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") == false) return;

        collider
            .gameObject
            .GetComponent<PlayerAutoControl>()
            .OnDamage(m_damage);

        var position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        // プレイヤーの前方に位置を押し出している
        position += collider.gameObject.transform.forward * 2.0f;

        AttackEffect(DamageEffect.DamageEffectType.Rabbit, position);

        Destroy(gameObject);
    }
}
