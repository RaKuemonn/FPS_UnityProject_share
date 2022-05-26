using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SliderMover : MonoBehaviour
{
    private Slider mySlider;
    private GameObject thisSlider;
    private float sliderChange;
    private float maxSliderValue;
    private float minSliderValue;
    private float sliderRange;
    private const float SLIDERSTEP = 100.0f; //used to detrime how fine to change value

    private InputAction moveR;
    private InputAction moveL;

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponentInParent<Slider>();
        thisSlider = gameObject; //used to deterine when slider has 'focus'
        maxSliderValue = mySlider.maxValue;
        minSliderValue = mySlider.minValue;
        sliderRange = maxSliderValue - minSliderValue;
        moveR = GetComponent<PlayerInput>().actions["MoveR"];
        //moveL = GetComponent<PlayerInput>().actions["MoveR"];
        mySlider.value = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        //If slider has 'focus'
        if (thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange =moveR.ReadValue<Vector2>().normalized.x * sliderRange / SLIDERSTEP;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            if (tempValue <= maxSliderValue && tempValue >= minSliderValue)
            {
                if (moveR.ReadValue<Vector2>().x > 0.3f || moveR.ReadValue<Vector2>().x < -0.3f)
                {
                    GameObject.Find("SoundManager").GetComponent<S_SoundManager>().PlaySE((int)S_SoundManager.SE.TEST_SLASH);
                    Debug.Log("SE");
                    sliderValue = tempValue;
                }
               
            }
            else if (sliderValue > 0.99f && moveR.ReadValue<Vector2>().x > 0.3f)
            {
                GameObject.Find("SoundManager").GetComponent<S_SoundManager>().PlaySE((int)S_SoundManager.SE.TEST_SLASH);
                Debug.Log("MAX");
            }

            mySlider.value = sliderValue;
            GameObject.Find("SoundManager").GetComponent<S_SoundManager>().SEAudioSource.volume = mySlider.value;

        }
    }
}
