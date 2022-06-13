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
    //SFX
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


    //Music
    //Audio references
    [SerializeField]
    private EventReference magicForest_Music_reference;

    //Audio event instances
    EventInstance magicForest_Music_instance;


    //AMB
    //Audio references
    [SerializeField]
    private EventReference plains_AMB_eventReference;

    [SerializeField]
    private EventReference magicForest_AMB_eventReference;

    [SerializeField]
    private EventReference citadel_AMB_eventReference;

    //Audio event instances
    EventInstance plains_AMB_instance;
    EventInstance magicForest_AMB_instance;
    EventInstance citadel_AMB_instance;


    //Snapshots
    //References
    [SerializeField]
    private EventReference normalStateSnapshotReference;

    [SerializeField]
    private EventReference lowHealthStateSnapshotReference;

    //Instances
    EventInstance normalStateSnapshotInstance;
    EventInstance lowHealthStateSnapshotInstance;


    public void SetInstances()
    {
        //set instances
        //SFX
        typewriterKey_instance = RuntimeManager.CreateInstance(typewriterKey_eventReference);
        completeWord_instance = RuntimeManager.CreateInstance(completeWord_eventReference);
        mistake_instance = RuntimeManager.CreateInstance(mistake_eventReference);
        cutsceneChange_instance = RuntimeManager.CreateInstance(cutsceneChange_eventReference);
        cutsceneEnd_instance = RuntimeManager.CreateInstance(cutsceneEnd_eventReference);

        //Music
        magicForest_Music_instance = RuntimeManager.CreateInstance(magicForest_Music_reference);

        //AMB
        plains_AMB_instance = RuntimeManager.CreateInstance(plains_AMB_eventReference);
        magicForest_AMB_instance = RuntimeManager.CreateInstance(magicForest_AMB_eventReference);
        citadel_AMB_instance = RuntimeManager.CreateInstance(citadel_AMB_eventReference);

        //snapshots
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

    public void ChangeMusic(FieldType field)
    {
        magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


        switch (field)
        {
            case FieldType.Plains:
                break;
            case FieldType.MagicForest:
                magicForest_Music_instance.start();
                break;
            case FieldType.Citadel:
                break;
            default:
                break;
        }
    }

    public void ChangeAMB(FieldType field)
    {
        plains_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        magicForest_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        citadel_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


        switch (field)
        {
            case FieldType.Plains:
                plains_AMB_instance.start();
                break;
            case FieldType.MagicForest:
                magicForest_AMB_instance.start();
                break;
            case FieldType.Citadel:
                citadel_AMB_instance.start();
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
                lowHealthStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                normalStateSnapshotInstance.start();
                break;
            case SnapshotName.LowHealth:
                normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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

        plains_AMB_instance.release();
        magicForest_AMB_instance.release();
        citadel_AMB_instance.release();
       
        normalStateSnapshotInstance.release();
        lowHealthStateSnapshotInstance.release();
    }
}
