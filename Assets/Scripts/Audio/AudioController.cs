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
    Block,
    EnemyAttackComplete,
    PuzzleComplete,
    PuzzleWrong
}

public enum OtherMusicName
{
    Combat,
    GameOver
}

public enum SoundState
{
    Normal,
    LowHealth,
    Pause
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

    [SerializeField]
    private EventReference enemyAttackComplete_eventReference;

    [SerializeField]
    private EventReference puzzleComplete_eventReference;

    [SerializeField]
    private EventReference puzzleWrong_eventReference;

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
    EventInstance enemyAttackComplete_instance;
    EventInstance puzzleComplete_instance;
    EventInstance puzzleWrong_instance;


    //Music
    //Audio references
    [SerializeField]
    private EventReference plains_Music_reference;

    [SerializeField]
    private EventReference magicForest_Music_reference;

    [SerializeField]
    private EventReference citadel_Music_reference;

    [SerializeField]
    private EventReference combat_Music_reference;

    [SerializeField]
    private EventReference gameOver_Music_reference;

    //Audio event instances
    EventInstance plains_Music_instance;
    EventInstance magicForest_Music_instance;
    EventInstance citadel_Music_instance;
    EventInstance combat_Music_instance;
    EventInstance gameOver_Music_instance;


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

    [SerializeField]
    private EventReference pauseStateSnapshotReference;



    //Instances
    EventInstance normalStateSnapshotInstance;
    EventInstance lowHealthStateSnapshotInstance;
    EventInstance pauseStateSnapshotInstance;

    //mixer
    private Bus SFXBus;


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
        enemyAttackComplete_instance = RuntimeManager.CreateInstance(enemyAttackComplete_eventReference);
        puzzleComplete_instance = RuntimeManager.CreateInstance(puzzleComplete_eventReference);
        puzzleWrong_instance = RuntimeManager.CreateInstance(puzzleWrong_eventReference);

        //Music
        plains_Music_instance = RuntimeManager.CreateInstance(plains_Music_reference);
        magicForest_Music_instance = RuntimeManager.CreateInstance(magicForest_Music_reference);
        citadel_Music_instance = RuntimeManager.CreateInstance(citadel_Music_reference);
        combat_Music_instance = RuntimeManager.CreateInstance(combat_Music_reference);
        gameOver_Music_instance = RuntimeManager.CreateInstance(gameOver_Music_reference);

        //AMB
        plains_AMB_instance = RuntimeManager.CreateInstance(plains_AMB_eventReference);
        magicForest_AMB_instance = RuntimeManager.CreateInstance(magicForest_AMB_eventReference);
        citadel_AMB_instance = RuntimeManager.CreateInstance(citadel_AMB_eventReference);

        //snapshots
        normalStateSnapshotInstance = RuntimeManager.CreateInstance(normalStateSnapshotReference);
        lowHealthStateSnapshotInstance = RuntimeManager.CreateInstance(lowHealthStateSnapshotReference);
        pauseStateSnapshotInstance = RuntimeManager.CreateInstance(pauseStateSnapshotReference);

        SFXBus = RuntimeManager.GetBus("bus:/SFX");

        ChangeSnapshot(SoundState.Normal);
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
            case SFXName.EnemyAttackComplete:
                enemyAttackComplete_instance.start();
                break;
            case SFXName.PuzzleComplete:
                puzzleComplete_instance.start();
                break;
            case SFXName.PuzzleWrong:
                puzzleWrong_instance.start();
                break;
            default:
                break;
        }
    }

    public void ChangeMusic(FieldType field = FieldType.Plains, bool playOtherMusic = false, OtherMusicName otherMusicName = OtherMusicName.Combat, bool stopPlayingAll = false)
    {
        if (stopPlayingAll)
        {
            plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameOver_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            return;
        }

        EventInstance musicToPlay = new EventInstance();

        if (!playOtherMusic)
        {
            switch (field)
            {
                case FieldType.Plains:
                    magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    gameOver_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = plains_Music_instance;
                    break;
                case FieldType.MagicForest:
                    plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    gameOver_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = magicForest_Music_instance;
                    break;
                case FieldType.Citadel:
                    plains_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    magicForest_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    gameOver_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = citadel_Music_instance;
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
                    citadel_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = combat_Music_instance;
                    break;
                case OtherMusicName.GameOver:
                    combat_Music_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    musicToPlay = gameOver_Music_instance;
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

    public void ChangeAMB(FieldType field = FieldType.Plains, bool stopPlayingAll = false)
    {
        if (stopPlayingAll)
        {
            plains_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            magicForest_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            citadel_AMB_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            return;
        }

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

    public void ChangeSnapshot(SoundState snapshotName = SoundState.Normal, bool stopPlayingAll = false)
    {
        if (stopPlayingAll)
        {
            normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            pauseStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            lowHealthStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            return;
        }

        switch (snapshotName)
        {
            case SoundState.Normal:
                pauseStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                lowHealthStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                normalStateSnapshotInstance.start();
                break;
            case SoundState.LowHealth:
                normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                pauseStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                lowHealthStateSnapshotInstance.start();
                break;
            case SoundState.Pause:
                normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                lowHealthStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                pauseStateSnapshotInstance.start();
                break;
            default:
                break;
        }
    }

    public void ResumePauseSounds(bool pause)
    {
        SFXBus.setPaused(pause);

        if (pause)
        {
            ChangeSnapshot(SoundState.Pause);
        }
        else
        {
            ChangeSnapshot(SoundState.Normal);
        }
    }

    private void OnDestroy()
    {
        //stop snapshots
        ChangeSnapshot(stopPlayingAll: true);

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
        enemyAttackComplete_instance.release();
        puzzleComplete_instance.release();
        puzzleWrong_instance.release();

        //Music
        ChangeMusic(stopPlayingAll: true);
        plains_Music_instance.release();
        magicForest_Music_instance.release();
        citadel_Music_instance.release();
        combat_Music_instance.release();
        gameOver_Music_instance.release();


        //AMB
        ChangeAMB(stopPlayingAll: true);
        plains_AMB_instance.release();
        magicForest_AMB_instance.release();
        citadel_AMB_instance.release();
        
        //Snapshots
        lowHealthStateSnapshotInstance.release();
        pauseStateSnapshotInstance.release();
    }
}
