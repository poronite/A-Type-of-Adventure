using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ATOA;

public class Options_Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject OptionsMenu;

    [SerializeField]
    private Toggle fullscreenToggle;

    [SerializeField]
    private Slider barOpacitySlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider voiceVolumeSlider;

    private GameObject lastSelectedUIOptionsMenu;


    private void Start()
    {
        lastSelectedUIOptionsMenu = fullscreenToggle.gameObject;
    }


    public void Back()
    {
        Debug.Log("Return to Main Menu");
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        lastSelectedUIOptionsMenu = fullscreenToggle.gameObject;
    }


    void Update()
    {
        if (OptionsMenu.activeInHierarchy == true)
        {
            //Debug.Log(EventSystem.current.currentSelectedGameObject);
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                lastSelectedUIOptionsMenu = EventSystem.current.currentSelectedGameObject;
                
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedUIOptionsMenu);
            }
        }
    }
}
