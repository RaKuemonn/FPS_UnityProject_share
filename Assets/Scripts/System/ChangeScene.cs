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
        var se = GameObject.Find("SE");
        var bgm = GameObject.Find("BGM");
        var soundManager = GameObject.Find("SoundManager");

        if (SceneName == "scene_title")
        {
            Destroy(se);
            Destroy(bgm);
            Destroy(soundManager);
            SceneManager.LoadScene(SceneName);
            return;
        }


        if(se){DontDestroyOnLoad(se);}
        if(bgm){DontDestroyOnLoad(bgm);}
        if(soundManager){DontDestroyOnLoad(soundManager);}

        SceneManager.LoadScene(SceneName);
    }
}
