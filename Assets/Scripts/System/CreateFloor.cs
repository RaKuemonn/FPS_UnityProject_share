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
                floorInfo.FloorData.id = i;     // idは要素順で設定される

                // Debug 床ごとに色替え
                var material = floor.GetComponent<Renderer>().material;
                if (material)
                {
#if UNITY_EDITOR
                    switch (datas[i].move_speed_state)
                    {
                        case FloorInfoMoveSpeed.Stop:
                            material.color = new Color(255f, 0f, 0f, 1f);   // 赤
                            break;


                        case FloorInfoMoveSpeed.SpeedUp:
                            material.color = new Color(0f, 0f, 255f, 1f);   // 蒼
                            break;


                        case FloorInfoMoveSpeed.Run:
                            material.color = new Color(0f, 255f, 0f, 1f);   // 緑
                            break;


                        case FloorInfoMoveSpeed.SpeedDown:
                            material.color = new Color(120f, 0f, 120f, 1f);// 紫
                            break;
                    }
#else
                    // ゲーム時は見えないようにする
                    material.color = new Color(0f,0f,0f,0f); // 透明
#endif
                }
                

                // 追加するコンポーネントデータがあれば
                if (datas[i].CondExprData)
                {
                    // Floorにコンポーネント(BaseComdExprを継承したものに限る)を追加して
                    var component = (BaseCondExpr)floor.AddComponent(
                        Type.GetType(datas[i].CondExprData.CondExprComponentName())
                    );

                    // 該当データの参照を渡す
                    component.data = datas[i].CondExprData;

                    // FloorのUnityEventとしてコンポーネントの関数を追加する。
                    floorInfo.FloorData.CompleteFloorConditionExpr.AddListener(component.OnCompleteCondExpr);
                }

            }

            // Enviromentsゲームオブジェクトを親にする
            floor.transform.SetParent(Enviroments);
        }

    }


}
  