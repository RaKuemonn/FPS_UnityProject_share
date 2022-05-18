using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumySickleController : MonoBehaviour
{
    [SerializeField]
    private GameObject boss;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBossController enemyBoss = boss.GetComponent<EnemyBossController>();

        GameObject[] child = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            child[i] = transform.GetChild(i).gameObject;
        }

        if (enemyBoss.weaponReflect)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                MeshRenderer mesh = child[i].GetComponent<MeshRenderer>();
                if (mesh) mesh.enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                MeshRenderer mesh = child[i].GetComponent<MeshRenderer>();
                if (mesh) mesh.enabled = false;
            }
        }
    }
}
