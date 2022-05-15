using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BattleAreaTutorial : MonoBehaviour
{
    private List<Collider> enemyColliders = new List<Collider>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            CallBack_Scarecrow();
        }

        if (collider.tag == "Enemy")
        {
            // 入っている敵を追加している (が、同じ敵を入れる可能性がありそう...)


            enemyColliders.Add(collider);
        }
    }

    void Update()
    {
        // 中にいる敵の数を常に確認する。 (死んだ敵はOnTriggerExit()で感知できないので、この処理をしている)
        // 最後の削除後に、現在の戦闘に残っている敵の数がわかる


        var size = enemyColliders.Count;
        var remove_indexes = new int[size];

        // listから削除する物を列挙
        for (int i = 0; i < size; ++i)
        {
            remove_indexes[i] = -1; // 初期化

            var enemyCollider = enemyColliders[i];

            if (enemyCollider) continue;
            remove_indexes[i] = i;  // 削除条件(敵が死んでcolliderが消えている)を満たしていてもすぐ消さない(イテレーターが逝かれそう)
        }

        // 削除
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

    private void CallBack_Scarecrow()
    {
        foreach (var collider in enemyColliders)
        {
            var control = collider?.gameObject.GetComponent<ScarecrowControl>();

            if (!control) continue;

            control.OnEnterBattleArea();
        }
    }
}


