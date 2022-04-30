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

        var floorPrefab = floorDataTable.FloorPrefab;
        var datas = floorDataTable.FloorDatas;
        GameObject floor = null;

        int size = floorDataTable.FloorDatas.Length;
        for (int i = 0; i < size; ++i)
        {
            floor = Instantiate(floorPrefab, new Vector3(0f, 0f, 10f * i), Quaternion.identity);
            floor.GetComponent<FloorInfo>().FloorData = datas[i];
            floor.transform.SetParent(Enviroments);
        }
        
    }


}
  