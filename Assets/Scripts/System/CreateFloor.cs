using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
    [SerializeField] private MasterData masterData;
    [SerializeField] private Transform Enviroments;

    public void Start()
    {
        var floorDataTable = masterData.FloorDataTable;

        
        var datas = floorDataTable.FloorDatas;
        GameObject floor = null;
        
        int size = floorDataTable.FloorDatas.Length;
        for (int i = 0; i < size; ++i)
        {
            floor = Instantiate(floorDataTable.FloorPrefab, new Vector3(0f, 0f, 10f * i), Quaternion.identity);
            {
                var floorInfo = floor.GetComponent<FloorInfo>();
                floorInfo.FloorData = datas[i];
                floorInfo.FloorData.id = i;     // id�͗v�f���Őݒ肳���

                // Debug �����ƂɐF�ւ�
                var material = floor.GetComponent<Renderer>().material;
                if (material)
                {
#if UNITY_EDITOR
                    switch (datas[i].move_speed_state)
                    {
                        case FloorInfoMoveSpeed.Stop:
                            material.color = new Color(255f, 0f, 0f, 1f);   // ��
                            break;


                        case FloorInfoMoveSpeed.SpeedUp:
                            material.color = new Color(0f, 0f, 255f, 1f);   // ��
                            break;


                        case FloorInfoMoveSpeed.Run:
                            material.color = new Color(0f, 255f, 0f, 1f);   // ��
                            break;


                        case FloorInfoMoveSpeed.SpeedDown:
                            material.color = new Color(120f, 0f, 120f, 1f);// ��
                            break;
                    }
#else
                    // �Q�[�����͌����Ȃ��悤�ɂ���
                    material.color = new Color(0f,0f,0f,0f); // ����
#endif
                }
                

                // �ǉ�����R���|�[�l���g�f�[�^�������
                if (datas[i].CondExprData)
                {
                    // Floor�ɃR���|�[�l���g(BaseComdExpr���p���������̂Ɍ���)��ǉ�����
                    var component = (BaseCondExpr)floor.AddComponent(
                        Type.GetType(datas[i].CondExprData.CondExprComponentName())
                    );

                    // �Y���f�[�^�̎Q�Ƃ�n��
                    component.data = datas[i].CondExprData;

                    // Floor��UnityEvent�Ƃ��ăR���|�[�l���g�̊֐���ǉ�����B
                    floorInfo.FloorData.CompleteFloorConditionExpr.AddListener(component.OnCompleteCondExpr);
                }

            }

            // Enviroments�Q�[���I�u�W�F�N�g��e�ɂ���
            floor.transform.SetParent(Enviroments);
        }

    }


}
  