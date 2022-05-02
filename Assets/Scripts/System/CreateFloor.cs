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

        //var Floor = floorDataTable.Floor;
        //var size = Floor.Length;
        //for (int i = 0; i < size; ++i)
        //{
        //    Floor[i].GetComponent<FloorInfo>().FloorData.id = i;
        //}
        
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

                // �ǉ�����R���|�[�l���g�̖��O�����Ă�����Ă����
                if (datas[i].CondExprName.Length > 0)
                {
                    // Floor�ɃR���|�[�l���g(BaseComdExpr���p���������̂Ɍ���)��ǉ�����
                    var component = (BaseCondExpr)floor.AddComponent(Type.GetType(datas[i].CondExprName));
                    // Floor��UnityEvent�Ƃ��ăR���|�[�l���g�̊֐���ǉ�����B
                    floorInfo.FloorData.StopFloorConditionExpr.AddListener(component.OnCompleteCondExpr);
                }

            }

            floor.transform.SetParent(Enviroments);
        }

    }


}
  