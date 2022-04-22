using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main_Menu_Buttons : MonoBehaviour
{
    private GameObject lastSelectedButton;

    
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        }
    }
}
