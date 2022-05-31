using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SettingController : MonoBehaviour
{
    private Slider mySlider;

    private float oldValue;

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponentInParent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //oldValue 
    }
}