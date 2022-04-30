using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Click()
    {
        SceneManager.LoadScene(SceneName);
    }

    void OnTriggerEnter(Collider collider)
    {
        var player = collider.transform.gameObject;
        if (player.name != "Player") return;

        SceneManager.LoadScene(SceneName);
    }
}
