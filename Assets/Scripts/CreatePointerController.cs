using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePointerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateTargetPointer(GameObject gameObject)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load("EffectCircle"));
        var canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);// = //transform.parent;//.SetParent(transform.parent);
        Debug.Log(obj.transform.parent);

        var enemyAttack = obj.GetComponent<EnemyAttackPrudir>();
        enemyAttack.m_gameObject = gameObject;
    }
}
