using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThrowingStartAreaControl : MonoBehaviour
{
    [SerializeField] private Transform[] throwing_start_position;
    [SerializeField] private GameObject RabbitThrowingPrefab;
    [SerializeField] private PlayerAutoControl playerAutoControl;
    [SerializeField] private Image Caution;
    [SerializeField] private Vector3 end_position;
    [SerializeField] private GameObject parent;
    [SerializeField] private float default_spawn_time;
    [SerializeField] private float count_down_size;
    private float timer;
    private float counter;
    private bool is_start;
    private Tween CautionRendering1;
    private Tween CautionRendering2;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if(is_start == false) return;

        timer += Time.deltaTime;

        Check();
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") == false) return;
        if(is_start == true) return;

        is_start = true;
        Caution.enabled = true;
        Invoke("SetCautionDisable", 1.5f);
        CautionRendering1 = DOTween.To(
            () => 255.0f,
            x =>
            {
                Caution.color = new Color(Caution.color.a, Caution.color.g, Caution.color.b, x);
                if (x <= 0.0f)
                {
                    CautionRendering2 = DOTween.To(
                        () => 255.0f,
                        x =>
                        {
                            Caution.color = new Color(Caution.color.a, Caution.color.g, Caution.color.b, x);
                            
                        },
                        0f,
                        0.75f);
                }

            },
            0f,
            0.75f);
    }

    void Check()
    {
        
        if (playerAutoControl.gameObject.transform.position.z > end_position.z)
        {
            Destroy(gameObject);
        }


        if(counter >= count_down_size) return;

        var rate = 1.0f - (counter / count_down_size + 3 /* rate??0????????????????offset */ );

        if (timer < Mathf.Abs(rate * default_spawn_time)) return;

        timer = 0f;
        counter++;

        CreateRabbitThrowing();

    }

    void CreateRabbitThrowing()
    {
        var rand = UnityEngine.Random.Range(0, throwing_start_position.Length);

        var vector3 = throwing_start_position[rand].position;
        var RabbitThrowing = Instantiate(RabbitThrowingPrefab, new Vector3(vector3.x, vector3.y, vector3.z), Quaternion.identity);

        var controller = RabbitThrowing.GetComponent<RabbitThrowingController>();
        controller.PlayerAutoControl = playerAutoControl;
        controller.StartPosition = throwing_start_position[rand].position;
        controller.arrive_time = 5f;

        RabbitThrowing.transform.SetParent(parent.transform);
    }

    void SetCautionDisable()
    {
        Caution.enabled = false;
    }
}
