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

    // SquareBattleAreaオブジェクトに存在している敵の総数確認用リスト　(自動更新される)
    private List<BaseEnemy> enemies = new List<BaseEnemy>();
    private List<BaseEnemy> battle_enemies = new List<BaseEnemy>();
    
    private bool is_player_in;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (battle_enemies.Count >= battle_enemy_size) return;

            is_player_in = true;

            // 霧の表示を消す
            GameObject
                .FindGameObjectWithTag("Fog")
                .GetComponent<Renderer>()
                .enabled = false;

            // 戦闘させる敵をbattle_enemiesに追加していく
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

            // このBattleAreaゲームオブジェクト内にいる
            // battle_enemiesに含まれる通知を出す。
            // 敵(案山子しか設定してない)は戦闘体制になる。
            CallBack_FirstEnemies();
        }

        if (collider.tag == "Enemy")
        {
            // BattleAreaゲームオブジェクト内に入っている敵を追加している
            // (が、同じ敵を入れる可能性がありそう...)
            // (一回入った敵がBattleAreaから出ることがないようにしておけば問題ない。)

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
            // 集合させる
            enemy.SetAssemblyFlag(true);
            enemy.OnEnterBattleArea();

            // 位置決める
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
            // 中にいる敵の数を常に確認する。 (死んだ敵はOnTriggerExit()で感知できないので、この処理をしている)
            // 最後の削除後に、現在の戦闘に残っている敵の数がわかる

            // enemies
            {
                //var size = enemies.Count;
                //var remove_indexes = new int[size];
                //
                //// listから削除する物を列挙
                //for (int i = 0; i < size; ++i)
                //{
                //    remove_indexes[i] = -1; // 初期化
                //
                //    var enemy = enemies[i];
                //
                //    if (enemy.IsDeath == false) continue;
                //    remove_indexes[i] = i; // 削除条件(敵が死んでcolliderが消えている)を満たしていてもすぐ消さない(イテレーターが逝かれそう)
                //}
                //
                //// 削除
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
                //// listから削除する物を列挙
                //for (int i = 0; i < size; ++i)
                //{
                //    remove_indexes[i] = -1; // 初期化
                //
                //    var enemy = battle_enemies[i];
                //
                //    if (enemy.IsDeath == false) continue;
                //    remove_indexes[i] = i; // 削除条件(敵が死んでcolliderが消えている)を満たしていてもすぐ消さない(イテレーターが逝かれそう)
                //}
                //
                //// 削除
                //foreach (var removeIndex in remove_indexes)
                //{
                //    if (removeIndex == -1) continue;
                //
                //    battle_enemies.RemoveAt(removeIndex);
                //}

            }
            battle_enemies.RemoveAll(e => e.IsDeath == true);
        }

        // SquareBattleAreaの敵が減ったら
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

                    // 集合していない敵
                  if(enemy.GetEnterBattleArea())continue;

                  // 集合させる
                  enemy.SetAssemblyFlag(true);
                  enemy.OnEnterBattleArea();

                  battle_enemies.Add(enemy);

                  // 位置決める
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
