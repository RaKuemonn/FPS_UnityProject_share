using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClearPerformanceController : MonoBehaviour
{
    [SerializeField] private GameObject ClearUIs;
    [SerializeField] private EnemyBossController boss;
    private InputAction Button_A;

    void Start()
    {
        Button_A = GetComponent<PlayerInput>().actions["XboxA"];
    }

    void Update()
    {
        if (boss == null) return;
        if (boss.GetDeathFlag() == false) return;
        if (boss.GetEndDeathAnimation() == false) return;

        Time.timeScale = 0.0f;
        ClearUIs.SetActive(true); 
        
        // Aƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚Ä‚¢‚½‚ç
        if (Button_A.triggered == false) return;
        Time.timeScale = 1.0f;
        GetComponent<ChangeScene>().Change();
    }
}
