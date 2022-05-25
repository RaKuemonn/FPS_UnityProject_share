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
        if (gameObject.tag == "Player") return;

        var player = collider.transform.gameObject;
        if (player.tag != "Player") return;

        SceneManager.LoadScene(SceneName);
    }

    public void Change()
    {
        SceneManager.LoadScene(SceneName);
    }
}
