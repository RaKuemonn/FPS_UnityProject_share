using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstSelectButton : MonoBehaviour
{

    public Button FirstSelectedButton;
    // Start is called before the first frame update
    void Start()
    {
        FirstSelectedButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
