using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public enum SFXName
{
    TypewriterKey,
    CompleteWord,
    Mistake,
    CutsceneChange,
    CutsceneEnd,
    Slash,
    Hit,
    Punch,
    Crouch,
    Dash,
    Roll,
    Block
}

public enum OtherMusicName
{
    Combat,
    GameOver
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

    [SerializeField]
    private EventReference slash_eventReference;

    [SerializeField]
    private EventReference hit_eventReference;

    [SerializeField]
    private EventReference punch_eventReference;

    [SerializeField]
    private EventReference crouch_eventReference;

    [SerializeField]
    private EventReference dash_eventReference;

    [SerializeField]
    private EventReference roll_eventReference;

    [SerializeField]
    private EventReference block_eventReference;

    //Audio event instances
    EventInstance typewriterKey_instance;
    EventInstance completeWord_instance;
    EventInstance mistake_instance;
    EventInstance cutsceneChange_instance;
    EventInstance cutsceneEnd_instance;
    EventInstance slash_instance;
    EventInstance hit_instance;
    EventInstance punch_instance;
    EventInstance crouch_instance;
    EventInstance dash_instance;
    EventInstance roll_instance;
    EventInstance block_instance;


    //Music
    //Audio references
    [SerializeField]
    private EventReference plains_Music_reference;

    [SerializeField]
    private EventReference magicForest_Music_reference;

    //[SerializeField]
    //private EventReference citadel_Music_reference;

    [SerializeField]
    private EventReference combat_Music_reference;

    //[SerializeField]
    //private EventReference gameOver_Music_reference;

    //Audio event instances
    EventInstance plains_Music_instance;
    EventInstance magicForest_Music_instance;
    //EventInstance citadel_Music_instance;
    EventInstance combat_Music_instance;
    //EventInstance gameOver_Music_instance;


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
        slash_instance = RuntimeManager.CreateInstance(slash_eventReference);
        hit_instance = RuntimeManager.CreateInstance(hit_eventReference);
        punch_instance = RuntimeManager.CreateInstance(punch_eventReference);
        crouch_instance = RuntimeManager.CreateInstance(crouch_eventReference);
        dash_instance = RuntimeManager.CreateInstance(dash_eventReference);
        roll_instance = RuntimeManager.CreateInstance(roll_eventReference);
        block_instance = RuntimeManager.CreateInstance(block_eventReference);

        //Music
        plains_Music_instance = RuntimeManager.CreateInstance(plains_Music_reference);
        magicForest_Music_instance = RuntimeManager.CreateInstance(magicForest_Music_reference);
        //citadel_Music_instance = RuntimeManager.CreateInstance(citadel_Music_reference);
        combat_Music_instance = RuntimeManager.CreateInstance(combat_Music_reference);
        //gameOver_Music_instance = RuntimeManager.CreateInstance(gameOver_Music_reference);

        //AMB
        plains_AMB_instance = RuntimeManager.CreateInstance(plains_AMB_eventReference);
        magicForest_AMB_instance = RuntimeManager.CreateInstance(magicForest_AMB_eventReference);
        citadel_AMB_instance = RuntimeManager.CreateInstance(citadel_AMB_eventReference);

        //snapshots
        normalStateSnapshotInstance = RuntimeManager.CreateInstance(normalStateSnapshotReference);
        lowHealthStateSnapshotInstance = RuntimeManager.CreateInstance(lowHealthStateSnapshotReference);
    }


    public void TriggerSFX(SFXName sfxName)
    {
        switch (sfxName)
        {
            case SFXName.TypewriterKey:
                typewriterKey_instance.start();
                break;
            case SFXName.CompleteWord:
                completeWord_instance.start();
                break;
            case SFXName.Mistake:
                mistake_instance.start();
                break;
            case SFXName.CutsceneChange:
                cutsceneChange_instance.start();
                break;
            case SFXName.CutsceneEnd:
                cutsceneEnd_instance.start();
                break;
            case SFXName.Slash:
                slash_instance.start();
                break;
            case SFXName.Hit:
                hit_instance.start();
                break;
            case SFXName.Punch:
                punch_instance.start();
                break;
            case SFXName.Crouch:
                crouch_instance.start();
                break;
            case SFXName.Dash:
                dash_instance.start();
                break;
            case SFXName.Roll:
                roll_instance.start();
                break;
            case SFXName.Block:
                block_instance.start();
                break;
            default:
                break;
        }
    }

    public void ChangeMusic(FieldType field = FieldType.Plains, bool playOtherMusic = false, OtherMusicName otherMusicName = OtherMusicName.Combat)
    {
        EventInstance musicToPlay = new EventInstance();

        if (!playOtherMusic)
        {
            switch (field)
            {
                case FieldType.Plains:
                    magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    //citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    Debug.Log("Plains");

                    musicToPlay = plains_Music_instance;
                    break;
                case FieldType.MagicForest:
                    plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    //citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = magicForest_Music_instance;
                    break;
                case FieldType.Citadel:
                    plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    //musicToPlay = citadel_Music_instance;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (otherMusicName)
            {
                case OtherMusicName.Combat:
                    plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    //citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = combat_Music_instance;
                    break;
                case OtherMusicName.GameOver:
                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    //musicToPlay = gameOver_Music_instance;
                    break;
                default:
                    break;
            }
        }

        musicToPlay.getPlaybackState(out PLAYBACK_STATE state);

        musicToPlay.getDescription(out EventDescription description);

        description.getPath(out string path);

        Debug.Log($"{path}, {state}");

        if (state != PLAYBACK_STATE.PLAYING)
            musicToPlay.start();
    }

    public void ChangeAMB(FieldType field = FieldType.Plains, bool stopPlaying = false)
    {
        if (!stopPlaying)
        {
            switch (field)
            {
                case FieldType.Plains:
                    magicForest_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    citadel_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    plains_AMB_instance.start();
                    break;
                case FieldType.MagicForest:
                    plains_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    citadel_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    magicForest_AMB_instance.start();
                    break;
                case FieldType.Citadel:
                    plains_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    magicForest_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    citadel_AMB_instance.start();
                    break;
                default:
                    break;
            }
        }
        else
        {
            plains_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            magicForest_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            citadel_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
        //SFX
        typewriterKey_instance.release();
        completeWord_instance.release();
        mistake_instance.release();
        cutsceneChange_instance.release();
        cutsceneEnd_instance.release();
        slash_instance.release();
        hit_instance.release();
        punch_instance.release();
        crouch_instance.release();
        dash_instance.release();
        roll_instance.release();
        block_instance.release();

        //Music
        plains_Music_instance.release();
        magicForest_Music_instance.release();
        //citadel_Music_instance.release();
        combat_Music_instance.release();
        //gameOver_Music_instance.release();


        //AMB
        plains_AMB_instance.release();
        magicForest_AMB_instance.release();
        citadel_AMB_instance.release();
        
        //Snapshots
        normalStateSnapshotInstance.release();
        lowHealthStateSnapshotInstance.release();
    }
}
