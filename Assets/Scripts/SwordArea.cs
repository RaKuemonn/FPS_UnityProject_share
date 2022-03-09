using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        var sword_area_pos = transform.position;
        Renderer renderer = null;
        float distance = float.MaxValue;

        foreach(GameObject enemy in enemies)
        {
            renderer = enemy.GetComponent<Renderer>();

            renderer.material.color = new Color(225f, 0f, 0f, 1f);

            distance = Vector3.Distance(sword_area_pos, enemy.transform.position);

            if (distance > 4.0f) continue;

            renderer.material.color = new Color(0f, 225f, 0f, 1f);

        }


    }
}
