using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugEventSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //EventSystemが何も選択していない場合、firstSelectedを選択
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("くそ死ね");
            //if (firstSelected != null && firstSelected.gameObject.activeInHierarchy && firstSelected.interactable)
            //{
            //    EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
            //}
        }
        Debug.Log(EventSystem.current.currentSelectedGameObject);

    }
}
