using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerAssemblyController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // 指定したタグを持つGameObjectを 全て 取得する。
            GameObject[] dragons = GameObject.FindGameObjectsWithTag("Enemy");

            foreach(GameObject dragon in dragons)
            {
                EnemyDragonController dController = dragon.GetComponent<EnemyDragonController>();
                if (dController)
                {
                    dController.SetAssemblyFlag(true);
                }
                EnemyBatController bController = dragon.GetComponent<EnemyBatController>();
                if (bController)
                {
                    bController.SetAssemblyFlag(true);
                }
                EnemySnakeController sController = dragon.GetComponent<EnemySnakeController>();
                if (sController)
                {
                    sController.SetAssemblyFlag(true);
                }

            }

            // 削除
            Destroy(gameObject);
        }

    }
}
