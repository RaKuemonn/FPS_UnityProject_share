using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSickle : MonoBehaviour
{
    private float easingTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var parentDra = transform.parent.GetComponent<DeathDragon>();

        if (!parentDra.checkFlag) return;

        transform.eulerAngles = Easing.SineInOut(easingTimer, 0.5f, new Vector3(0, 0, 0), new Vector3(0, 0, 100));
        transform.localPosition = Easing.SineInOut(easingTimer, 0.5f, new Vector3(0, 0.2f, -0.5f), new Vector3(0.0f, 0.2f, -0.2f));


        easingTimer += Time.deltaTime;

        if (easingTimer > 0.5f) easingTimer = 0.5f;
    }
}
