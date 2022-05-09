using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public enum AudioName
{
    TypewriterKey,
    CompleteWord,
    Mistake,
    CutsceneChange
}


public class AudioController : MonoBehaviour
{
    //Audio references
    [SerializeField] 
    private EventReference typewriterKey_eventReference;

    [SerializeField]
    private EventReference completeWord_eventReference;

    [SerializeField]
    private EventReference mistake_eventReference;

    [SerializeField]
    private EventReference cutsceneChange_eventReference;


    //Audio event instances
    EventInstance typewriterKey_instance;
    EventInstance completeWord_instance;
    EventInstance mistake_instance;
    EventInstance cutsceneChange_instance;



    private void Awake()
    {
        //set instances
        typewriterKey_instance = RuntimeManager.CreateInstance(typewriterKey_eventReference);
        completeWord_instance = RuntimeManager.CreateInstance(completeWord_eventReference);
        mistake_instance = RuntimeManager.CreateInstance(mistake_eventReference);
        cutsceneChange_instance = RuntimeManager.CreateInstance(cutsceneChange_eventReference);
    }


    public void TriggerSFX(AudioName audioName)
    {
        switch (audioName)
        {
            case AudioName.TypewriterKey:
                typewriterKey_instance.start();
                break;
            case AudioName.CompleteWord:
                completeWord_instance.start();
                break;
            case AudioName.Mistake:
                mistake_instance.start();
                break;
            case AudioName.CutsceneChange:
                cutsceneChange_instance.start();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        //free instances from memory
        typewriterKey_instance.release();
        completeWord_instance.release();
        mistake_instance.release();
        cutsceneChange_instance.release();
    }
}
