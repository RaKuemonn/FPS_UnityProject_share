using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// *注意* シーンごとに別のマスターデータが存在する。
public class MasterData : MonoBehaviour
{
    public FloorDataTable FloorDataTable { get; private set; }

    [SerializeField] private FloorDataTable floorDataTable;

    public void Awake() // コンポーネントが生成された際、呼び出される関数
    {
        FloorDataTable = floorDataTable;

    }
}
