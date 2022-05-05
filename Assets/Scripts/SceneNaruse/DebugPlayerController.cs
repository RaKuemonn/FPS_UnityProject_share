using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugPlayerController : MonoBehaviour
{
    private InputAction moveL;
    private PlayerAutoControl autoControl;

    [SerializeField]
    public float move_speed = 5.0f;

#if UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {
        moveL = GetComponent<PlayerInput>().actions["MoveL"];
        autoControl = GetComponent<PlayerAutoControl>();
    }

    // Update is called once per frame
    void Update()
    {
        var _input = moveL.ReadValue<Vector2>();
        if (_input.magnitude < 0) { return; }

        autoControl.speed_rate = 0.0f;

        var displacement = new Vector3(_input.x, 0f, _input.y).normalized * move_speed * Time.deltaTime;
        gameObject.transform.Translate(displacement);
    }
#endif
}
