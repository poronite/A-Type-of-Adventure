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
    CutsceneChange,
    CutsceneEnd
}

public enum SnapshotName
{
    Normal,
    LowHealth
}


public class AudioController : MonoBehaviour
{
    //Events
    //Audio references
    [SerializeField] 
    private EventReference typewriterKey_eventReference;

    [SerializeField]
    private EventReference completeWord_eventReference;

    [SerializeField]
    private EventReference mistake_eventReference;

    [SerializeField]
    private EventReference cutsceneChange_eventReference;

    [SerializeField]
    private EventReference cutsceneEnd_eventReference;


    //Audio event instances
    EventInstance typewriterKey_instance;
    EventInstance completeWord_instance;
    EventInstance mistake_instance;
    EventInstance cutsceneChange_instance;
    EventInstance cutsceneEnd_instance;



    //Snapshots
    //References
    [SerializeField]
    private EventReference normalStateSnapshotReference;

    [SerializeField]
    private EventReference lowHealthStateSnapshotReference;

    //Instances
    EventInstance normalStateSnapshotInstance;
    EventInstance lowHealthStateSnapshotInstance;



    private void Awake()
    {
        //set instances
        typewriterKey_instance = RuntimeManager.CreateInstance(typewriterKey_eventReference);
        completeWord_instance = RuntimeManager.CreateInstance(completeWord_eventReference);
        mistake_instance = RuntimeManager.CreateInstance(mistake_eventReference);
        cutsceneChange_instance = RuntimeManager.CreateInstance(cutsceneChange_eventReference);
        cutsceneEnd_instance = RuntimeManager.CreateInstance(cutsceneEnd_eventReference);

        normalStateSnapshotInstance = RuntimeManager.CreateInstance(normalStateSnapshotReference);
        lowHealthStateSnapshotInstance = RuntimeManager.CreateInstance(lowHealthStateSnapshotReference);
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
            case AudioName.CutsceneEnd:
                cutsceneEnd_instance.start();
                break;
            default:
                break;
        }
    }

    public void ChangeSnapshot(SnapshotName snapshotName)
    {
        switch (snapshotName)
        {
            case SnapshotName.Normal:
                normalStateSnapshotInstance.start();
                break;
            case SnapshotName.LowHealth:
                lowHealthStateSnapshotInstance.start();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        //stop snapshots
        normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        lowHealthStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        //free instances from memory
        typewriterKey_instance.release();
        completeWord_instance.release();
        mistake_instance.release();
        cutsceneChange_instance.release();
        cutsceneEnd_instance.release();
       
        normalStateSnapshotInstance.release();
        lowHealthStateSnapshotInstance.release();
    }
}
