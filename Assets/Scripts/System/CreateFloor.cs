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
            var data = datas[i];

            var position = new Vector3(0f, 0f, 10f * i);

            floor = Instantiate(floorDataTable.FloorPrefab, position, Quaternion.identity);
            {
                // Enviromentsゲームオブジェクトを親にする
                floor.transform.SetParent(Enviroments);

                var FloorInfo = floor.GetComponent<FloorInfo>();
                FloorInfo.FloorData = data;
                FloorInfo.FloorData.id = i;     // idは要素順で設定される

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
                

                if(FloorInfo.FloorData.move_speed_state != FloorInfoMoveSpeed.Stop)
                {continue;}
                

                // 追加するコンポーネントデータがあれば
                if (data.CondExprData)
                {

                    // Floorにコンポーネント(BaseComdExprを継承したものに限る)を追加して
                    floor.AddComponent(
                        Type.GetType(data.CondExprData.CondExprComponentName())
                    );

                    var component = floor.GetComponent<BaseCondExpr>();

                    // 該当データの参照を渡す
                    component.data = data.CondExprData;
                    
                    // FloorのUnityEventとしてコンポーネントの関数を追加する。
                    FloorInfo.FloorData.CompleteFloorConditionExpr.AddListener(component.OnCompleteCondExpr);
                }

            }

        }

    }


}
  