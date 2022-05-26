using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareBattleArea : MonoBehaviour
{
    public const int battle_enemy_size = 3;

    [SerializeField] private TerritoryOrigin TerritoryOrigin_1;
    [SerializeField] private TerritoryOrigin TerritoryOrigin_2;
    [SerializeField] private TerritoryOrigin TerritoryOrigin_3;

    // SquareBattleArea�I�u�W�F�N�g�ɑ��݂��Ă���G�̑����m�F�p���X�g�@(�����X�V�����)
    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    private List<BaseEnemy> battle_enemies = new List<BaseEnemy>();
    
    private bool is_player_in;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (battle_enemies.Count >= battle_enemy_size) return;

            is_player_in = true;

            // ���̕\��������
            GameObject
                .FindGameObjectWithTag("Fog")
                .GetComponent<Renderer>()
                .enabled = false;

            // �퓬������G��battle_enemies�ɒǉ����Ă���
            //int i = 0;
            //foreach (var enemy in enemies)
            //{
            //    if (i > battle_enemy_size - 1) break;
            //
            //    battle_enemies.Add(enemy);
            //    ++i;
            //}

            var size = enemies.Count;
            var divide_three_size = size / 3;
            for (int i = 0; i < battle_enemy_size; ++i)
            {
                battle_enemies.Add(enemies[i * divide_three_size]);
            }

            // ����BattleArea�Q�[���I�u�W�F�N�g���ɂ���
            // battle_enemies�Ɋ܂܂��ʒm���o���B
            // �G(�ĎR�q�����ݒ肵�ĂȂ�)�͐퓬�̐��ɂȂ�B
            CallBack_FirstEnemies();
        }

        if (collider.tag == "Enemy")
        {
            // BattleArea�Q�[���I�u�W�F�N�g���ɓ����Ă���G��ǉ����Ă���
            // (���A�����G������\�������肻��...)
            // (���������G��BattleArea����o�邱�Ƃ��Ȃ��悤�ɂ��Ă����Ζ��Ȃ��B)

            BaseEnemy currentEnemy = collider.gameObject.GetComponent<BaseEnemy>();
            currentEnemy.OnDeadEvent += (enemy, arg) => EraseList(enemy as BaseEnemy);

            enemies.Add(currentEnemy);

            Debug.Log("CurrentEnemyCount:" + enemies.Count);

        }
    }
    
    void CallBack_FirstEnemies()
    {
        foreach (var enemy in battle_enemies)
        {
            // �W��������
            enemy.SetAssemblyFlag(true);
            enemy.OnEnterBattleArea();

            // �ʒu���߂�
            if (TerritoryOrigin_1.is_in_territory == false)
            { enemy.SetLocationPosition(TerritoryOrigin_1.transform.position); TerritoryOrigin_1.is_in_territory = true; TerritoryOrigin_1.enemy = enemy; continue; }

            else if (TerritoryOrigin_2.is_in_territory == false)
            { enemy.SetLocationPosition(TerritoryOrigin_2.transform.position); TerritoryOrigin_2.is_in_territory = true; TerritoryOrigin_2.enemy = enemy; continue; }

            else if (TerritoryOrigin_3.is_in_territory == false)
            { enemy.SetLocationPosition(TerritoryOrigin_3.transform.position); TerritoryOrigin_3.is_in_territory = true; TerritoryOrigin_3.enemy = enemy; continue; }

        }
    }

    

    void Update()
    {

        //////////////////////////////////////
        if (is_player_in == false) return;
        //////////////////////////////////////
        

        {
            // ���ɂ���G�̐�����Ɋm�F����B (���񂾓G��OnTriggerExit()�Ŋ��m�ł��Ȃ��̂ŁA���̏��������Ă���)
            // �Ō�̍폜��ɁA���݂̐퓬�Ɏc���Ă���G�̐����킩��

            // enemies
            {
                //var size = enemies.Count;
                //var remove_indexes = new int[size];
                //
                //// list����폜���镨���
                //for (int i = 0; i < size; ++i)
                //{
                //    remove_indexes[i] = -1; // ������
                //
                //    var enemy = enemies[i];
                //
                //    if (enemy.IsDeath == false) continue;
                //    remove_indexes[i] = i; // �폜����(�G�������collider�������Ă���)�𖞂����Ă��Ă����������Ȃ�(�C�e���[�^�[�������ꂻ��)
                //}
                //
                //// �폜
                //foreach (var removeIndex in remove_indexes)
                //{
                //    if (removeIndex == -1) continue;
                //
                //    enemies.RemoveAt(removeIndex);
                //}

            }
            enemies.RemoveAll(e => e.IsDeath == true);

            // battle_enemies
            {
                //var size = battle_enemies.Count;
                //var remove_indexes = new int[size];
                //
                //// list����폜���镨���
                //for (int i = 0; i < size; ++i)
                //{
                //    remove_indexes[i] = -1; // ������
                //
                //    var enemy = battle_enemies[i];
                //
                //    if (enemy.IsDeath == false) continue;
                //    remove_indexes[i] = i; // �폜����(�G�������collider�������Ă���)�𖞂����Ă��Ă����������Ȃ�(�C�e���[�^�[�������ꂻ��)
                //}
                //
                //// �폜
                //foreach (var removeIndex in remove_indexes)
                //{
                //    if (removeIndex == -1) continue;
                //
                //    battle_enemies.RemoveAt(removeIndex);
                //}

            }
            battle_enemies.RemoveAll(e => e.IsDeath == true);
        }

        // SquareBattleArea�̓G����������
        {

            if (battle_enemies.Count < battle_enemy_size)
            {
                //var enemy = enemies.First();
                //enemy.OnEnterBattleArea();
                foreach (var enemy in enemies)
                { 

                  if (TerritoryOrigin_1.is_in_territory &&
                      TerritoryOrigin_2.is_in_territory &&
                      TerritoryOrigin_3.is_in_territory) break;

                    // �W�����Ă��Ȃ��G
                  if(enemy.GetEnterBattleArea())continue;

                  // �W��������
                  enemy.SetAssemblyFlag(true);
                  enemy.OnEnterBattleArea();

                  battle_enemies.Add(enemy);

                  // �ʒu���߂�
                  if (TerritoryOrigin_1.is_in_territory == false)
                  { enemy.SetLocationPosition(TerritoryOrigin_1.transform.position); TerritoryOrigin_1.is_in_territory = true; TerritoryOrigin_1.enemy = enemy; continue; }

                  else if (TerritoryOrigin_2.is_in_territory == false)
                  { enemy.SetLocationPosition(TerritoryOrigin_2.transform.position); TerritoryOrigin_2.is_in_territory = true; TerritoryOrigin_2.enemy = enemy; continue; }

                  else if (TerritoryOrigin_3.is_in_territory == false)
                  { enemy.SetLocationPosition(TerritoryOrigin_3.transform.position); TerritoryOrigin_3.is_in_territory = true; TerritoryOrigin_3.enemy = enemy; continue; }
                  
                }
            }
        }
    }

    public void EraseList(BaseEnemy enemy_)
    {
        enemies.Remove(enemy_);
        battle_enemies.Remove(enemy_);
        Debug.Log("CurrentEnemyCount:" + enemies.Count);
    }

    public int InAreaEnemySize()
    {
        return enemies.Count;
    }
}
