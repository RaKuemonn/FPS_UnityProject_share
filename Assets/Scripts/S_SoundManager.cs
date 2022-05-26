using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundManager : MonoBehaviour
{
    public AudioSource SEAudioSource;            //SE用AudioSource
    public AudioSource BGMAudioSource;

    public AudioClip[] SEClip,BGMClip;

    public enum SE      //効果音のdefine
    {
        TEST_SLASH,
    }

    public enum BGM
    {
        //TITLE,
        //GAME1,
        //MAINGAME,
        //TUTORIAL,
        //CHALLENGE,
    }


    void Start()
    {
        SEAudioSource = GameObject.Find("SE").GetComponent<AudioSource>();
        BGMAudioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
    }


    void Update()
    {
        D_CheckNullAudioSource();
    }


    //効果音呼び出し関数
    public void PlaySE(SE index)
    {
        D_CheckNullAudioClip((int)index);
        SEAudioSource.PlayOneShot(SEClip[(int)index]);
    }

    public void PlayBGM(BGM index)
    {
        BGMAudioSource.PlayOneShot(BGMClip[(int)index]);
    }

    //AudioSourceがnullの場合警告を出す
    private void D_CheckNullAudioSource()
    {
        if (SEAudioSource == null)
        {
           // Debug.LogError("エラー！SEオーディオが未設定やで！");
        }
    }

    //AudioClipがnullかarray外の場合警告を出す
    private void D_CheckNullAudioClip(int index)
    {
        if (index>SEClip.Length||SEClip.Length==0)
        {
            Debug.LogError("エラー！SEクリップの要素数を超えてるで！");
        }
        if (SEClip[index]==null)
        {
            Debug.LogError("エラー！SEクリップの"+index+"番は未設定やで！");
        }
    }

}
