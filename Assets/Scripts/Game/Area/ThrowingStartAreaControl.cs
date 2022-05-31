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

    private float timer;
    private float count_downer = 10f;
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
        

        if(count_downer <= 0f) return;
        if (timer < count_downer) return;

        timer = 0f;
        count_downer--;

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
