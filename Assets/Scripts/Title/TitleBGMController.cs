using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGMController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("SoundManager")?.GetComponent<S_SoundManager>()?.PlayBGM(clip, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
