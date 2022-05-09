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
        // ���ɉe�������̂̓v���C���[�̂�
        var player = collider.transform.gameObject;
        if (player.name != "Player") {return;}
        
        

        // �ړ����x�{���̐��� (����enum state���画�f�����x�����肷��)
        {

            // ��������
            var boolean = new BooleanClass();
            FloorData.CompleteFloorConditionExpr?.Invoke(boolean);

            // �萔
            const float zero_rate = 0f;
            const float half_rate = 0.5f;
            const float max_rate = 1f;

            var component = player.GetComponent<PlayerAutoControl>();

            // ���̏�Ԃ��Ƃ̏���(���x����)�@(����enum�����͂��������Ŏg�p)
            switch (FloorData.move_speed_state)
            {
                case FloorInfoMoveSpeed.Stop:
                    component.speed_rate = boolean.Boolean /* CompleteFloorConditionExpr�C�x���g�̌��� */
                        ?/* true  */ Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime)
                        :/* false */ zero_rate;
                    break;


                case FloorInfoMoveSpeed.SpeedUp:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, max_rate, Time.deltaTime);   // 1�b�����čő呬�x��
                    break;


                case FloorInfoMoveSpeed.Run:
                    component.speed_rate = max_rate;
                    break;


                case FloorInfoMoveSpeed.SpeedDown:
                    component.speed_rate = Mathf.Lerp(component.speed_rate, half_rate, Time.deltaTime);   // 1�b�����ĐÎ~��Ԃ�
                    break;
            }


        }
    }
    
}
