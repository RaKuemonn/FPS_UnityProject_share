using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    public FloorData FloorData;

    [SerializeField, Range(0f, 100f)] public float idle_seconds = 3f;
    public float timer;

    void Start()
    {
        timer = 0f;
    }

    void OnTriggerStay(Collider collider)
    {
        var player = collider.transform.gameObject;
        if (player.name != "Player") return;
        
        // 当たっている間は滞在時間として時間を加算し続ける。
         timer += Time.deltaTime;
        

        // 移動速度倍率の制御 (床のenum stateから判断し速度を決定する)
        {
            const float zero_rate = 0f;
            const float half_rate = 0.5f;
            const float max_rate = 1f;

            var component = player.GetComponent<PlayerAutoControl>();

            switch (FloorData.move_speed_state)
            {
                case FloorInfoMoveSpeed.Stop:
                    // 条件(滞在時間が待機時間を超えている)が整っていれば
                    component.speed_rate = (timer < idle_seconds)
                        ? zero_rate
                        : Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime);
                    break;

                case FloorInfoMoveSpeed.SpeedUp:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime);   // 1秒かけて最大速度に
                    break;

                case FloorInfoMoveSpeed.Run:
                    component.speed_rate = max_rate;
                    break;

                case FloorInfoMoveSpeed.SpeedDown:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, half_rate, Time.deltaTime);   // 1秒かけて静止状態に
                    break;
            }
        }


    }

}
