using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            //// �w�肵���^�O������GameObject�� �S�� �擾����B
            //GameObject[] dragons = GameObject.FindGameObjectsWithTag("Enemy");
            //
            //foreach(GameObject dragon in dragons)
            //{
            //    EnemyDragonController dController = dragon.GetComponent<EnemyDragonController>();
            //    if (dController)
            //    {
            //        dController.SetAssemblyFlag(true);
            //    }
            //    EnemyBatController bController = dragon.GetComponent<EnemyBatController>();
            //    if (bController)
            //    {
            //        bController.SetAssemblyFlag(true);
            //    }
            //    EnemySnakeController sController = dragon.GetComponent<EnemySnakeController>();
            //    if (sController)
            //    {
            //        sController.SetAssemblyFlag(true);
            //    }
            //
            //}

            var battleAreas = GameObject.FindGameObjectsWithTag("BattleArea");
            GameObject nearest_battleArea = null;
            foreach (var battleArea in battleAreas)
            {
                
                // �v���C���[�����Ȃ�continue
                if(other.transform.position.z >
                   battleArea.transform.position.z) continue;
                
                // null�Ȃ�
                if (nearest_battleArea == null)
                {
                    nearest_battleArea = battleArea;
                }

                // �b��̈�ԋ߂�battleArea���z���Ⴂ�Ȃ�continue
                if (battleArea.transform.position.z >
                   nearest_battleArea.transform.position.z) continue;

                nearest_battleArea = battleArea;
            }

            nearest_battleArea
                .GetComponent<BattleAreaTutorial>()
                .AssemblyEnemies();

            // �폜
            Destroy(gameObject);
        }

    }
}
