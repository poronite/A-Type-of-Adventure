using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMOD.Studio;
using FMODUnity;

public class Options_Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;

    [SerializeField]
    private Toggle fullscreenToggle;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider voiceVolumeSlider;

    private GameObject lastSelectedUIOptionsMenu;

    //FMOD Mixer Bus
    private Bus SFXBus;
    private Bus MusicBus;
    private Bus VoiceBus;



    private void Start()
    {
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        VoiceBus = RuntimeManager.GetBus("bus:/Voice");


        lastSelectedUIOptionsMenu = fullscreenToggle.gameObject;

        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        MusicBus.setVolume(musicVolumeSlider.value);

        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        SFXBus.setVolume(sfxVolumeSlider.value);

        voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.8f);
        VoiceBus.setVolume(voiceVolumeSlider.value);
    }


    public void Back()
    {
        Debug.Log("Return to Main/Pause Menu");
        Menu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        lastSelectedUIOptionsMenu = fullscreenToggle.gameObject;
        gameObject.SetActive(false);
    }


    void Update()
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


    public void OnFullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreenToggle.isOn));
    }

    public void OnMusicVolumeSlider()
    {
        //change in FMOD
        MusicBus.setVolume(musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void OnSFXVolumeSlider()
    {
        //change in FMOD
        SFXBus.setVolume(sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void OnVoiceVolumeSlider()
    {
        //change in FMOD
        VoiceBus.setVolume(voiceVolumeSlider.value);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolumeSlider.value);
    }
}
