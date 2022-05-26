using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scene_game�ł����p���Ă���
[RequireComponent(typeof(BoxCollider))]
public class BattleAreaTutorial : MonoBehaviour
{
    // BattleArea�I�u�W�F�N�g�ɑ��݂��Ă���G�̑����m�F�p���X�g�@(�����X�V�����)
    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // ����BattleArea�Q�[���I�u�W�F�N�g���ɂ���
            // �SEnemy�ɒʒm���o���B
            // �G�͐퓬�̐��ɂȂ�B
            CallBack_AllEnemies();
        }

        if (collider.tag == "Enemy")
        {
            // BattleArea�Q�[���I�u�W�F�N�g���ɓ����Ă���G��ǉ����Ă���
            // (���A�����G������\�������肻��...)
            // (���������G��BattleArea����o�邱�Ƃ��Ȃ��悤�ɂ��Ă����Ζ��Ȃ��B)

            BaseEnemy currentEnemy = collider.gameObject.GetComponent<BaseEnemy>();
            currentEnemy.OnDeadEvent += (enemy, arg) => EraseList(enemy as BaseEnemy);

                enemies.Add(currentEnemy);

                Debug.Log("CurrentEnemyCount:" + InAreaEnemySize());
            
        }
    }

    void Update()
    {
        {
            // ���ɂ���G�̐�����Ɋm�F����B (���񂾓G��OnTriggerExit()�Ŋ��m�ł��Ȃ��̂ŁA���̏��������Ă���)
            // �Ō�̍폜��ɁA���݂̐퓬�Ɏc���Ă���G�̐����킩��


            var size = enemies.Count;
            var remove_indexes = new int[size];

            // list����폜���镨���
            for (int i = 0; i < size; ++i)
            {
                remove_indexes[i] = -1; // ������

                var enemy = enemies[i];

                if (enemy.IsDeath == false) continue;
                remove_indexes[i] = i; // �폜����(�G�������collider�������Ă���)�𖞂����Ă��Ă����������Ȃ�(�C�e���[�^�[�������ꂻ��)
            }

            // �폜
            foreach (var removeIndex in remove_indexes)
            {
                if (removeIndex == -1) continue;

                enemies.RemoveAt(removeIndex);
            }
        }
    }


    public int InAreaEnemySize()
    {
        return enemies.Count;
    }

    private void CallBack_AllEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnEnterBattleArea();
        }
    }

    public void EraseList(BaseEnemy enemy_)
    {
        enemies.Remove(enemy_);
        Debug.Log("CurrentEnemyCount:" + InAreaEnemySize());
    }
}


