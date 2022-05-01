using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Fps_UnityProject_share/FloorDataTable", fileName = "FloorDataTable")]
public class FloorDataTable : ScriptableObject
{
    [SerializeField] public GameObject FloorPrefab;
    [SerializeField,Range(5f,10f)] public float floor_distance;
    public FloorData[] FloorDatas;
}

[Serializable]
public class FloorData  // 10m*10mマスの床が持つデータ
{
    public int id = 0;                              // 床の識別子 (手動割り振り)
    public FloorInfoMoveSpeed move_speed_state = FloorInfoMoveSpeed.Stop; // 床が持っている速度状態 (この状態をプレイヤーが当たり判定を用いて受け取ることで、プレイヤーの速度を制御させている)
    public StopFloorConditionExpr FloorConditionExpr;   // Stop床から走り出す条件式
}
public enum FloorInfoMoveSpeed
{
    Stop,       // 待機する,完全停止する床
    SpeedUp,    // 速度を上げ始める床
    Run,        // 最大速度の床
    SpeedDown,  // 速度を落とし始める床
}

//[CustomEditor(typeof(FloorData))]
//public class FloorDataEditor : Editor
//{
//    //private FloorData _target;

//    private SerializedProperty _property;

//    private void Awake()
//    {
//        _property = serializedObject.FindProperty("FloorData");
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        EditorGUILayout.PropertyField(_property);
//        serializedObject.ApplyModifiedProperties();

//        //EditorGUI.BeginChangeCheck();

//        //_target.id = EditorGUILayout.IntField("ID", _target.id);

//        //_target.move_speed_state =
//        //    (FloorInfoMoveSpeed)EditorGUILayout.EnumPopup("FloorInfoMoveSpeed", _target.move_speed_state);


//        //if (_target.move_speed_state == FloorInfoMoveSpeed.Stop)
//        //{
//        //    EditorGUILayout.LabelField("Cond Expr for Run");
//        //    _target.FloorConditionExpr = 
//        //        (StopFloorConditionExpr)EditorGUILayout.ObjectField(
//        //            "FloorConditionExpr Component has GameObject",
//        //            _target.FloorConditionExpr,
//        //            typeof(StopFloorConditionExpr), true);

//        //}

//        //// GUIの更新があったら実行
//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    EditorUtility.SetDirty(_target);
//        //}
//    }
//}

