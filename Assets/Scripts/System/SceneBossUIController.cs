using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBossUIController : MonoBehaviour
{
    //ボタンを押した時の処理
    public void ExitButton()
    {
        SceneManager.LoadScene("scene_title");
    }
}
