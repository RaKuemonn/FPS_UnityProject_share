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
public class FloorData  // 10m*10m�}�X�̏������f�[�^
{
    public int id = 0;                              // ���̎��ʎq (�蓮����U��)
    public FloorInfoMoveSpeed move_speed_state = FloorInfoMoveSpeed.Stop; // ���������Ă��鑬�x��� (���̏�Ԃ��v���C���[�������蔻���p���Ď󂯎�邱�ƂŁA�v���C���[�̑��x�𐧌䂳���Ă���)
    public StopFloorConditionExpr FloorConditionExpr;   // Stop�����瑖��o��������
}
public enum FloorInfoMoveSpeed
{
    Stop,       // �ҋ@����,���S��~���鏰
    SpeedUp,    // ���x���グ�n�߂鏰
    Run,        // �ő呬�x�̏�
    SpeedDown,  // ���x�𗎂Ƃ��n�߂鏰
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

//        //// GUI�̍X�V������������s
//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    EditorUtility.SetDirty(_target);
//        //}
//    }
//}

