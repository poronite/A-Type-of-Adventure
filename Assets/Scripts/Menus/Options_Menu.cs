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
    private Slider masterVolumeSlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider sfxVolumeSlider;

    [SerializeField]
    private Slider ambVolumeSlider;




    //FMOD Mixer Bus
    private Bus MasterBus;
    private Bus SFXBus;
    private Bus MusicBus;
    private Bus AMBBus;



    private void Start()
    {
        MasterBus = RuntimeManager.GetBus("bus:/");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        AMBBus = RuntimeManager.GetBus("bus:/AMB");

        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen", 1));

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        MasterBus.setVolume(masterVolumeSlider.value);

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        MusicBus.setVolume(musicVolumeSlider.value);

        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        SFXBus.setVolume(sfxVolumeSlider.value);

        ambVolumeSlider.value = PlayerPrefs.GetFloat("AMBVolume", 0.8f);
        AMBBus.setVolume(ambVolumeSlider.value);
    }


    public void Back()
    {
        Debug.Log("Return to Main/Pause Menu");
        Menu.SetActive(true);
        gameObject.SetActive(false);
    }


    public void OnFullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreenToggle.isOn));
    }

    public void OnMasterVolumeSlider()
    {
        //change in FMOD
        MasterBus.setVolume(masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
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

    public void OnAMBVolumeSlider()
    {
        //change in FMOD
        AMBBus.setVolume(ambVolumeSlider.value);
        PlayerPrefs.SetFloat("AMBVolume", ambVolumeSlider.value);
    }
}
