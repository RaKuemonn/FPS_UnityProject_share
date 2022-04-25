using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class change_scene : MonoBehaviour
{
    private InputAction input_;

    // Start is called before the first frame update
    void Start()
    {
        input_ = GetComponent<PlayerInput>().actions["Select"];

    }

    // Update is called once per frame
    void Update()
    {
        if (input_ == null) return;

        if (input_.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Playground");
        }
    }
}
