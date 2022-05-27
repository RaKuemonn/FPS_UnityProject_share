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
        //EventSystem‚ª‰½‚à‘I‘ğ‚µ‚Ä‚¢‚È‚¢ê‡AfirstSelected‚ğ‘I‘ğ
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("‚­‚»€‚Ë");
            //if (firstSelected != null && firstSelected.gameObject.activeInHierarchy && firstSelected.interactable)
            //{
            //    EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
            //}
        }
        Debug.Log(EventSystem.current.currentSelectedGameObject);

    }
}
