using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlashDirectionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var sickle = transform.parent.GetComponent<EnemyAttackPrudir>().m_gameObject;
        if (!sickle) return;

        var parent = transform.parent.GetComponent<Image>();
        if (!parent.enabled)
        {
            var image = GetComponent<Image>();
            image.enabled = false;
            return;
        }
        else
        {
            var image = GetComponent<Image>();
            image.enabled = true;
        }


        var throwing = sickle.GetComponent<SickleThrowingController>();
        if (throwing)
        {
            float angleZ = throwing.GetSlashAbgle();
            transform.eulerAngles = new Vector3(0, 0, angleZ);
            return;
        }

        var sickleController = sickle.GetComponent<SickleController>();
        if (sickleController)
        {
            float angleZ = sickleController.GetSlashAngle();
            transform.eulerAngles = new Vector3(0, 0, angleZ);
            return;
        }

        var boss = sickle.GetComponent<EnemyBossController>();
        if (boss)
        {
            float angleZ = boss.GetSlashAngle();
            transform.eulerAngles = new Vector3(0, 0, angleZ);
            return;
        }
    }
}
