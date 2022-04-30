using System;
using System.Collections;
using System.Collections.Generic;
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
}
public enum FloorInfoMoveSpeed
{
    Stop,       // �ҋ@����,���S��~���鏰
    SpeedUp,    // ���x���グ�n�߂鏰
    Run,        // �ő呬�x�̏�
    SpeedDown,  // ���x�𗎂Ƃ��n�߂鏰
}