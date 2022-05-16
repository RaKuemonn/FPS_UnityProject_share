using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowStopControl : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {

        // �����蔻��̐���
        CreateCollideOnCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCollideOnCanvas()
    {
        // �a��ꂽ�Ƃ��ɐ������ꂽ�I�u�W�F�N�g�łȂ���΁iSetCutPerformance()�ŁA�ύX����Ă��Ȃ���΁j
        //if (is_create_collide == false) return;

        // �����蔻��p�̃I�u�W�F�N�g��Canvas���ɐ���
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }
}
