using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingStartAreaControl : MonoBehaviour
{
    [SerializeField] private Transform[] throwing_start_position;
    [SerializeField] private GameObject RabbitThrowingPrefab;
    [SerializeField] private PlayerAutoControl playerAutoControl;
    [SerializeField] private Vector3 end_position;
    [SerializeField] private GameObject parent;
    [SerializeField] private float default_spawn_time;
    [SerializeField] private float count_down_size;
    private float timer;
    private float counter;
    private bool is_start;



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

        is_start = true;
    }

    void Check()
    {
        
        if (playerAutoControl.gameObject.transform.position.z > end_position.z)
        {
            Destroy(gameObject);
        }


        if(counter >= count_down_size) return;
        var rate = 1.0f - (counter / count_down_size + 3 /* rate‚ª0‚É‚È‚ç‚È‚¢‚æ‚¤‚Éoffset */ );
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
}
