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

    void OnDestroy()
    {
        FloorData.StopFloorConditionExpr.RemoveAllListeners();
    }

    void OnTriggerStay(Collider collider)
    {
        var player = collider.transform.gameObject;
        if (player.name != "Player") return;
        
        // “–‚½‚Á‚Ä‚¢‚éŠÔ‚Í‘ØİŠÔ‚Æ‚µ‚ÄŠÔ‚ğ‰ÁZ‚µ‘±‚¯‚éB
         timer += Time.deltaTime;
        

        // ˆÚ“®‘¬“x”{—¦‚Ì§Œä (°‚Ìenum state‚©‚ç”»’f‚µ‘¬“x‚ğŒˆ’è‚·‚é)
        {
            const float zero_rate = 0f;
            const float half_rate = 0.5f;
            const float max_rate = 1f;

            var component = player.GetComponent<PlayerAutoControl>();

            switch (FloorData.move_speed_state)
            {
                case FloorInfoMoveSpeed.Stop:
                    // ğŒ(‘ØİŠÔ‚ª‘Ò‹@ŠÔ‚ğ’´‚¦‚Ä‚¢‚é)‚ª®‚Á‚Ä‚¢‚ê‚Î
                    var boolean = new BooleanClass();
                    FloorData.StopFloorConditionExpr?.Invoke(boolean);

                    component.speed_rate = boolean.Boolean
                    //component.speed_rate = (timer < idle_seconds)
                        ? zero_rate
                        : Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime);
                    break;

                case FloorInfoMoveSpeed.SpeedUp:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime);   // 1•b‚©‚¯‚ÄÅ‘å‘¬“x‚É
                    break;

                case FloorInfoMoveSpeed.Run:
                    component.speed_rate = max_rate;
                    break;

                case FloorInfoMoveSpeed.SpeedDown:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, half_rate, Time.deltaTime);   // 1•b‚©‚¯‚ÄÃ~ó‘Ô‚É
                    break;
            }
        }


    }
    
}
