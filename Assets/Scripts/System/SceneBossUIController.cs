using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBossUIController : MonoBehaviour
{
    //ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Ìˆ—
    public void ExitButton()
    {
        SceneManager.LoadScene("scene_title");
    }
}
