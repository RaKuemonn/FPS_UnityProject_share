using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scene_gameでも利用している
[RequireComponent(typeof(BoxCollider))]
public class BattleAreaTutorial : MonoBehaviour
{
    // BattleAreaオブジェクトに存在している敵の総数確認用リスト　(自動更新される)
    private List<BaseEnemy> enemies = new List<BaseEnemy>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // このBattleAreaゲームオブジェクト内にいる
            // 全Enemyに通知を出す。
            // 敵は戦闘体制になる。
            CallBack_AllEnemies();
        }

        if (collider.tag == "Enemy")
        {
            // BattleAreaゲームオブジェクト内に入っている敵を追加している
            // (が、同じ敵を入れる可能性がありそう...)
            // (一回入った敵がBattleAreaから出ることがないようにしておけば問題ない。)

            BaseEnemy currentEnemy = collider.gameObject.GetComponent<BaseEnemy>();
            currentEnemy.OnDeadEvent += (enemy, arg) => EraseList(enemy as BaseEnemy);

                enemies.Add(currentEnemy);

                Debug.Log("CurrentEnemyCount:" + InAreaEnemySize());
            
        }
    }

    void Update()
    {
        {
            // 中にいる敵の数を常に確認する。 (死んだ敵はOnTriggerExit()で感知できないので、この処理をしている)
            // 最後の削除後に、現在の戦闘に残っている敵の数がわかる


            var size = enemies.Count;
            var remove_indexes = new int[size];

            // listから削除する物を列挙
            for (int i = 0; i < size; ++i)
            {
                remove_indexes[i] = -1; // 初期化

                var enemy = enemies[i];

                if (enemy.IsDeath == false) continue;
                remove_indexes[i] = i; // 削除条件(敵が死んでcolliderが消えている)を満たしていてもすぐ消さない(イテレーターが逝かれそう)
            }

            // 削除
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


