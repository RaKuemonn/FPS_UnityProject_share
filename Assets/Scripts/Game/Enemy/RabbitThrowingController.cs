using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitThrowingController : BaseEnemy
{
    public PlayerAutoControl PlayerAutoControl;
    public Vector3 StartPosition;

    public float arrive_time;   // ThrowingStartAreaControl‚ÅÝ’è
    private float timer;


    void Start()
    {
        timer = 0f;
    }
    
    void Update()
    {
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


        Debug.Log(PlayerAutoControl.transform.position);
        Debug.Log(StartPosition);
    }

    public override void OnDead()
    {
        base.OnDead();
    }
}
