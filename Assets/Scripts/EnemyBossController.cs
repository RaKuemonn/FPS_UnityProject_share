using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    private float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
    }    
    

    // Update is called once per frame
    void Update()
    {
        angle += Mathf.PI * Time.deltaTime;
        //angle = //Mathf.Repeat(angle, 360);
        angle = Mathf.Sin(angle);




        transform.position = new Vector3(transform.position.x, transform.position.y + angle, transform.position.z);
        
    }
}
