using System;
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
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider voiceVolumeSlider;

    [SerializeField]
    private Slider barOpacitySlider;

    [SerializeField]
    private Image barOpacityTest;

    private GameObject lastSelectedUIOptionsMenu;


    private void Start()
    {
        lastSelectedUIOptionsMenu = fullscreenToggle.gameObject;
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));
        barOpacitySlider.value = PlayerPrefs.GetFloat("BarOpacity", 0.4f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.8f);
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


    public void OnFullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreenToggle.isOn));
    }

    public void OnBarOpacitySlider()
    {
        //maybe change an element on screen to show how it looks?
        barOpacityTest.color = new Color(0, 0, 0, barOpacitySlider.value);
        PlayerPrefs.SetFloat("BarOpacity", barOpacitySlider.value);
    }

    public void OnMusicVolumeSlider()
    {
        //change in FMOD
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void OnSFXVolumeSlider()
    {
        //change in FMOD
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void OnVoiceVolumeSlider()
    {
        //change in FMOD
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolumeSlider.value);
    }
}
