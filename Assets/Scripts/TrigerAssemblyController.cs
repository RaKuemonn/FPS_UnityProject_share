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
            // �w�肵���^�O������GameObject�� �S�� �擾����B
            GameObject[] dragons = GameObject.FindGameObjectsWithTag("Dragon");

            foreach(GameObject dragon in dragons)
            {
                EnemyDragonController dController = dragon.GetComponent<EnemyDragonController>();
                dController.SetAssemblyFlag(true);
            }

            // �폜
            Destroy(gameObject);
        }

    }
}
