using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// *����* �V�[�����Ƃɕʂ̃}�X�^�[�f�[�^�����݂���B
public class MasterData : MonoBehaviour
{
    public FloorDataTable FloorDataTable { get; private set; }

    [SerializeField] private FloorDataTable floorDataTable;

    public void Awake() // �R���|�[�l���g���������ꂽ�ہA�Ăяo�����֐�
    {
        FloorDataTable = floorDataTable;

    }
}
