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

[Serializable] public class FloorData  // 10m*10m�}�X�̏������f�[�^
{
    public int id;                              // ���̎��ʎq (�蓮����U��)
    public bool lock_floor_move_state;          // ���������Ă��鑬�x��Ԃ��Œ肷�邩 (true = ���I�ɕύX���Ȃ��A false = ���I�ɕύX�ł���)
    public FloorInfoMoveSpeed move_speed_state; // ���������Ă��鑬�x��� (���̏�Ԃ��v���C���[�������蔻���p���Ď󂯎�邱�ƂŁA�v���C���[�̑��x�𐧌䂳���Ă���)
}
public enum FloorInfoMoveSpeed
{
    Stop,       // �ҋ@����,���S��~���鏰
    SpeedUp,    // ���x���グ�n�߂鏰
    Run,        // �ő呬�x�̏�
    SpeedDown,  // ���x�𗎂Ƃ��n�߂鏰
}