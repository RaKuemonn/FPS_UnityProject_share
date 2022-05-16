using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BattleAreaTutorial : MonoBehaviour
{
    // BattleArea�I�u�W�F�N�g�ɑ��݂��Ă���G�̑����m�F�p���X�g�@(�����X�V�����)
    private List<Collider> enemyColliders = new List<Collider>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // ����BattleArea�Q�[���I�u�W�F�N�g���ɂ���
            // �SEnemy�ɒʒm���o���B
            // �G(�ĎR�q�����ݒ肵�ĂȂ�)�͐퓬�̐��ɂȂ�B
            CallBack_AllScarecrows();
        }

        if (collider.tag == "Enemy")
        {
            // BattleArea�Q�[���I�u�W�F�N�g���ɓ����Ă���G��ǉ����Ă���
            // (���A�����G������\�������肻��...)
            // (���������G��BattleArea����o�邱�Ƃ��Ȃ��悤�ɂ��Ă����Ζ��Ȃ��B)

            enemyColliders.Add(collider);
        }
    }

    void Update()
    {
        // ���ɂ���G�̐�����Ɋm�F����B (���񂾓G��OnTriggerExit()�Ŋ��m�ł��Ȃ��̂ŁA���̏��������Ă���)
        // �Ō�̍폜��ɁA���݂̐퓬�Ɏc���Ă���G�̐����킩��


        var size = enemyColliders.Count;
        var remove_indexes = new int[size];

        // list����폜���镨���
        for (int i = 0; i < size; ++i)
        {
            remove_indexes[i] = -1; // ������

            var enemyCollider = enemyColliders[i];

            if (enemyCollider) continue;
            remove_indexes[i] = i;  // �폜����(�G�������collider�������Ă���)�𖞂����Ă��Ă����������Ȃ�(�C�e���[�^�[�������ꂻ��)
        }

        // �폜
        foreach (var removeIndex in remove_indexes)
        {
            if(removeIndex == -1)continue;

            enemyColliders.RemoveAt(removeIndex);
        }
    }


    public int InAreaEnemySize()
    {
        return enemyColliders.Count;
    }

    private void CallBack_AllScarecrows()
    {
        foreach (var collider in enemyColliders)
        {
            var control = collider?.gameObject.GetComponent<ScarecrowMovingControl>();

            if (!control) continue;

            control.OnEnterBattleArea();
        }
    }
}


