using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// *����* �V�[�����Ƃɕʂ̃}�X�^�[�f�[�^�����݂���B
public class MasterData : MonoBehaviour
{
    public FloorDataTable FloorDataTable { get; private set; }

    [SerializeField] private FloorDataTable floorDataTable;

    public PlayerStatus PlayerStatus { get; private set; }

    [SerializeField] private PlayerStatus playerStatus;

    public EffectTable DamageEffectTable { get; private set; }

    [SerializeField] private EffectTable damageEffectTable;


    public void Awake() // �R���|�[�l���g���������ꂽ�ہA�Ăяo�����֐�
    {
        FloorDataTable = floorDataTable;

        playerStatus.current_hp = playerStatus.max_hp;
        PlayerStatus = playerStatus;

        DamageEffectTable = damageEffectTable;

    }
}
