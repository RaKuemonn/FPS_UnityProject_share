using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBossUIController : MonoBehaviour
{
    //�{�^�������������̏���
    public void ExitButton()
    {
        SceneManager.LoadScene("scene_title");
    }
}
