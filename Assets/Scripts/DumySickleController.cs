using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumySickleController : MonoBehaviour
{
    [SerializeField]
    private GameObject boss;


    private bool a = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBossController enemyBoss = boss.GetComponent<EnemyBossController>();


        if (enemyBoss.weaponReflect)
        {
            SkinnedMeshRenderer mesh = transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
            if (mesh)
            {
                mesh.enabled = true;
                if (a == false)
                {
                    gameObject.GetComponent<DissolveTimer_ChangeTexture>()?.OnGenerate();
                    a = true;
                }
            }
        }
        else
        {
            SkinnedMeshRenderer mesh = transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
            if (mesh)
            {
                mesh.enabled = false;
                a = false;
            }
        }
    }
}
