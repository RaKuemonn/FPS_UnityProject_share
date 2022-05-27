using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleButtonController : MonoBehaviour
{
    public GameObject uiBlocker;
    public GameObject titleUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�{�^�������������̏���
    public void StartButton()
    {
        SceneManager.LoadScene("scene_tutorial");

        var se = GameObject.Find("SE");
        var bgm = GameObject.Find("BGM");
        var soundManager = GameObject.Find("SoundManager");

        DontDestroyOnLoad(se);
        DontDestroyOnLoad(bgm);
        DontDestroyOnLoad(soundManager);
    }

    public void ExitButton()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif

    }

    public void SettingButton()
    {
        //var se = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        //se.firstSelectedGameObject = uiBlocker.transform.GetChild(1).gameObject;

        EventSystem.current.SetSelectedGameObject(uiBlocker.transform.GetChild(2).gameObject);

        //var se = GameObject.Find("UIblocker");
        uiBlocker.SetActive(true);

        titleUi.SetActive(false);
    }

    public void SettingExitButton()
    {
        EventSystem.current.SetSelectedGameObject(titleUi.transform.GetChild(1).gameObject);

        uiBlocker.SetActive(false);

        titleUi.SetActive(true);
    }
}
