using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownCircle : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private float coolTime;
    private float coolDownTimer = 0.0f;

    void Update()
    {
        if (coolDownTimer < 0.0f)
        {
            coolDownTimer = 0.0f;
        }

        circle.fillAmount = coolDownTimer / coolTime;

        coolDownTimer += -Time.deltaTime;
    }

    public void SetCoolDownTimer()
    {
        coolDownTimer = coolTime;
    }
}
