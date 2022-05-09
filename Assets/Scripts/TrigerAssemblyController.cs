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
            GameObject[] dragons = GameObject.FindGameObjectsWithTag("Dragon");

            foreach(GameObject dragon in dragons)
            {
                EnemyDragonController dController = dragon.GetComponent<EnemyDragonController>();
                dController.SetAssemblyFlag(true);
            }

            // 削除
            Destroy(gameObject);
        }

    }
}
