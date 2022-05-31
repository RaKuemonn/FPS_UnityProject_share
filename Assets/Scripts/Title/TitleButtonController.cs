using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleButtonController : MonoBehaviour
{
    public GameObject uiBlocker;
    public GameObject titleUi;
    public GameObject start;
    private bool is_can_submit = false;

    void Start()
    {
        Invoke("ChangeCanSubmitBoolean", 0.8f);
    }

    //ボタンを押した時の処理
    public void StartButton()
    {
        if (is_can_submit == false) return;

        GameObject.Find("SoundManager")?.GetComponent<S_SoundManager>()?.StopBGM();

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
        if (is_can_submit == false) return;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
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


        is_can_submit = false;
    }

    public void SettingExitButton()
    {
        EventSystem.current.SetSelectedGameObject(titleUi.transform.GetChild(1).gameObject);

        uiBlocker.SetActive(false);

        titleUi.SetActive(true);

        Invoke("ChangeCanSubmitBoolean", 0.2f);
    }

    private void ChangeCanSubmitBoolean()
    {
        is_can_submit = true;
    }
}
