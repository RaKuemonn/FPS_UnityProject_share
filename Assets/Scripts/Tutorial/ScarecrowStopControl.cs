using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowStopControl : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {

        // 当たり判定の生成
        CreateCollideOnCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCollideOnCanvas()
    {
        // 斬られたときに生成されたオブジェクトでなければ（SetCutPerformance()で、変更されていなければ）
        //if (is_create_collide == false) return;

        // 当たり判定用のオブジェクトをCanvas下に生成
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }
}
