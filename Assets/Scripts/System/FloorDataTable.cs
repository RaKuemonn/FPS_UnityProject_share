using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(menuName = "Fps_UnityProject_share/FloorDataTable", fileName = "FloorDataTable")]
public class FloorDataTable : ScriptableObject
{
    [SerializeField] public GameObject FloorPrefab;
    public FloorData[] FloorDatas;
    //public int id;
}

[Serializable] public class FloorData  // 10m*10mマスの床が持つデータ
{
    public int id;                              // 床の識別子 (手動割り振り)
    public bool lock_floor_move_state;          // 床が持っている速度状態を固定するか (true = 動的に変更しない、 false = 動的に変更できる)
    public FloorInfoMoveSpeed move_speed_state; // 床が持っている速度状態 (この状態をプレイヤーが当たり判定を用いて受け取ることで、プレイヤーの速度を制御させている)
}
public enum FloorInfoMoveSpeed
{
    Stop,       // 待機する,完全停止する床
    SpeedUp,    // 速度を上げ始める床
    Run,        // 最大速度の床
    SpeedDown,  // 速度を落とし始める床
}