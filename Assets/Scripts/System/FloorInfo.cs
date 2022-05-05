using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    public FloorData FloorData;

    void OnDestroy()
    {
        FloorData.CompleteFloorConditionExpr.RemoveAllListeners();
    }

    void OnTriggerStay(Collider collider)
    {
        var player = collider.transform.gameObject;
        if (player.name != "Player") {return;}
        
        

        // 移動速度倍率の制御 (床のenum stateから判断し速度を決定する)
        {
            const float zero_rate = 0f;
            const float half_rate = 0.5f;
            const float max_rate = 1f;

            var component = player.GetComponent<PlayerAutoControl>();

            // 条件が
            var boolean = new BooleanClass();
            FloorData.CompleteFloorConditionExpr?.Invoke(boolean);

            switch (FloorData.move_speed_state)
            {
                case FloorInfoMoveSpeed.Stop:
                    component.speed_rate = boolean.Boolean
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
