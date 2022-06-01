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

    private float oldSlider;
    private float interval = 0.5f;
    private float timer = 0.0f;

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
        moveL = GetComponent<PlayerInput>().actions["MoveL"];

        //moveL = GetComponent<PlayerInput>().actions["MoveR"];
        mySlider.value = 0.5f;
        timer = interval;
    }

    // Update is called once per frame
    void Update()
    {
        oldSlider = mySlider.value;

        //If slider has 'focus'
        if (thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange = moveR.ReadValue<Vector2>().normalized.x * sliderRange / SLIDERSTEP;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            if (tempValue <= maxSliderValue && tempValue >= minSliderValue)
            {
                    sliderValue = tempValue;
            }
            mySlider.value = sliderValue;
            GameObject.Find("SoundManager").GetComponent<S_SoundManager>().SEAudioSource.volume = mySlider.value;
        }
        if (thisSlider == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange = moveL.ReadValue<Vector2>().normalized.x * sliderRange / SLIDERSTEP;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            if (tempValue <= maxSliderValue && tempValue >= minSliderValue)
            {
                sliderValue = tempValue;
            }
            mySlider.value = sliderValue;
            GameObject.Find("SoundManager").GetComponent<S_SoundManager>().SEAudioSource.volume = mySlider.value;
        }


        float length = mySlider.value - oldSlider;
        length = Mathf.Sqrt(length * length);
        if (length > 0.008f && timer < 0.0f)
        {
            GameObject.Find("SoundManager").GetComponent<S_SoundManager>().PlaySE((int)S_SoundManager.SE.TEST_SLASH);
            Debug.Log("SE");
            oldSlider = mySlider.value;
            timer = interval;
        }
        Debug.Log(length);

        timer -= Time.deltaTime;
    }

    public void OnClick()
    {
        GameObject.Find("SoundManager").GetComponent<S_SoundManager>().PlaySE((int)S_SoundManager.SE.TEST_SLASH);
    }
}
