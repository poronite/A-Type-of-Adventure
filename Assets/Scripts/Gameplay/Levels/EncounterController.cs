using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EncounterController : MonoBehaviour
{
    private Image spriteToDisplay;

    [SerializeField]
    private Sprite forestBackground;
    [SerializeField]
    private Sprite magicForestBackground;
    [SerializeField]
    private Sprite castleBackground;

    delegate IEnumerator PlayVideoDelegate(VideoClip video, VideoClip videoloop);
    PlayVideoDelegate PlayVideo;

    delegate IEnumerator StopVideoDelegate();
    StopVideoDelegate StopVideo;

    delegate void AudioDelegate(AudioName name);
    AudioDelegate TriggerSFX;


    public void SetDelegatesEncounters()
    {
        CutsceneManager cutsceneManager = GameObject.FindGameObjectWithTag("CutsceneManager").GetComponent<CutsceneManager>();
        PlayVideo = cutsceneManager.PlayVideo;
        StopVideo = cutsceneManager.StopVideo;

        AudioController audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
        TriggerSFX = audioController.TriggerSFX;
    }


    public void EncounterTriggered(EncountersTemplate encounter)
    {
        switch (encounter.EncounterType)
        {
            case EncounterType.Image:
                if (spriteToDisplay == null)
                    spriteToDisplay = GameObject.FindGameObjectWithTag("ImageToDisplay").GetComponent<Image>();

                if (encounter.DisplayImage)
                {
                    spriteToDisplay.sprite = encounter.ImageToDisplay;

                    spriteToDisplay.enabled = true;
                }
                else
                {
                    spriteToDisplay.enabled = false;

                    spriteToDisplay.sprite = null;
                }
                break;
            case EncounterType.Cutscene:

                

                if (encounter.EndOfCutscene)
                {
                    TriggerSFX.Invoke(AudioName.CutsceneEnd);

                    StartCoroutine(StopVideo.Invoke());
                    break;
                }
                else
                {
                    TriggerSFX.Invoke(AudioName.CutsceneChange);

                    StartCoroutine(PlayVideo.Invoke(encounter.cutsceneVideo, encounter.cutsceneVideoLoop));
                }

                break;
            default:
                break;
        }
    }
}
