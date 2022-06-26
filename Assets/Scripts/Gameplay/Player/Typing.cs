using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using ATOA;

//This class will receive input from PlayerInput class and will type or delete a letter on the OutputWord string.
//It will also contain the PlayerState so that it can decide what to do with the input received.
//It can be considered as the hub of the player's actions in the game.

public enum PlayerState
{
    Adventure,
    Combat,
    Puzzle,
    Challenge,
    Dead,
    CombatComplete,
    ChallengeComplete,
    Loading,
    Pause,
    Options,
    EndGame
}

public class Typing : MonoBehaviour
{
    //Variables
    public PlayerState CurrentPlayerState = PlayerState.Adventure;

    //when pausing use this to store last player state 
    private PlayerState lastPlayerState = PlayerState.Adventure;

    ///<summary>Is player setting their name for the first time.</summary>
    private bool isSettingName = false;

    //When on branching path
    private bool onBranching = false;

    ///<summary>Word that the player has to currently type.</summary>
    private string currentWord;

    ///<summary>Output of CurrentWord that the player has already typed.</summary>
    private StringBuilder outputWord = new StringBuilder();

    ///<summary>Index of the next character that needs to be typed by player.</summary>
    private int nextCharacterIndex = 0;

    [HideInInspector]
    public bool canEndGame = false;

    private GraphicsUIManager gfxUIManager;
    private AudioController audioController;
    private PostProcessVolume globalVolume;



    //Delegates
    delegate void GameplayStatsDelegate();
    GameplayStatsDelegate AddTime;
    GameplayStatsDelegate Mistake;

    delegate void SetPlayerName(string name);
    SetPlayerName SetName;

    delegate void ClearWord();
    ClearWord ResetName;
    ClearWord ResetAnswerPzl;
    ClearWord ClearCurrentWordAdv;


    //Invoke the respective state script to output character to UI
    delegate void OutputCharacterDelegate(string character);
    OutputCharacterDelegate OutputCharacterAdv;
    OutputCharacterDelegate OutputCharacterCmb;
    OutputCharacterDelegate OutputCharacterPzl;
    OutputCharacterDelegate OutputCharacterChl;

    //when player has to decide between attack and dodge or between branching words
    delegate bool DecideWord(string character);
    DecideWord DecideBranchingWordAdv;
    DecideWord DecideActionWordCmb;

    //When word is complete
    delegate void CompleteWordDelegate(string word);
    CompleteWordDelegate CompleteWordAdv;
    CompleteWordDelegate CompleteWordCmb;
    CompleteWordDelegate CompleteWordPzl;
    CompleteWordDelegate CompleteWordChl;

    delegate void UpdateHintDelegate(string hint);
    UpdateHintDelegate UpdateHintAdvUI;

    delegate void TriggerAudioDelegate(SFXName audioName);
    TriggerAudioDelegate TriggerSFX;

    delegate void MenuDelegate(bool display);
    MenuDelegate ResumePauseGame;
    MenuDelegate DisplayOptionsMenu;


    public void SetDelegatesTyping()
    {
        PlayerStats stats = gameObject.GetComponent<PlayerStats>();
        Adventure advController = gameObject.GetComponent<Adventure>();
        Combat cmbController = gameObject.GetComponent<Combat>();
        Puzzle pzlController = gameObject.GetComponent<Puzzle>();
        Challenge chlController = gameObject.GetComponent<Challenge>();

        AddTime = stats.AddTimeElapsed;
        Mistake = stats.AddMistake;

        SetName = stats.SetName;
        ResetName = advController.ResetName;
        ResetAnswerPzl = pzlController.ResetAnswerPzl;
        ClearCurrentWordAdv = advController.ClearCurrentWord;

        OutputCharacterAdv = advController.AddCharacterUI;
        OutputCharacterCmb = cmbController.AddCharacterUI;
        OutputCharacterPzl = pzlController.AddCharacterUI;
        OutputCharacterChl = chlController.AddCharacterUI;

        DecideBranchingWordAdv = advController.SetBranchingWord;
        DecideActionWordCmb = cmbController.SetChosenWordCmb;

        CompleteWordAdv = advController.CompleteWordAdv;
        CompleteWordCmb = cmbController.CompleteWordCmb;
        CompleteWordPzl = pzlController.CompleteWordPzl;
        CompleteWordChl = chlController.CompleteWordChl;

        CompleteWordAdv += stats.AddTypedWordNumber;
        CompleteWordCmb += stats.AddTypedWordNumber;
        CompleteWordPzl += stats.AddTypedWordNumber;
        CompleteWordChl += stats.AddTypedWordNumber;

        UpdateHintAdvUI = advController.UpdateHintUI;

        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
        TriggerSFX = audioController.TriggerSFX;

        gfxUIManager = GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>();
        ResumePauseGame = gfxUIManager.ResumePauseGame;
        DisplayOptionsMenu = gfxUIManager.DisplayOptionsMenu;

        globalVolume = FindObjectOfType<PostProcessVolume>();
    }


    ///<summary>Invoked by Adventure, Combat and Puzzle scripts' SendNextWord delegate.</summary>
    public void NewWord(string word)
    {
        ClearWords();
        currentWord = word.TrimStart();

        Debug.Log(currentWord);

        switch (currentWord.Trim())
        {
            case "MCName":
                isSettingName = true;
                currentWord = string.Empty;
                ClearCurrentWordAdv.Invoke();
                UpdateHintAdvUI("Tip: Insert name of character and press Space Bar");
                Debug.Log("Setting name");
                break;
            case "*":
                onBranching = true;
                currentWord = string.Empty;
                ClearCurrentWordAdv.Invoke();
                Debug.Log("On branching");
                break;
            default:
                break;
        }
    }


    public void ClearWords()
    {
        currentWord = string.Empty;
        nextCharacterIndex = 0;
        outputWord.Clear();
    }


    ///<summary>Invoked by PlayerInput script's SendInput Delegate when player types a letter. | 
    ///Adventure and Combat: If correct AddCharacter, if not mistake += 1. |
    ///Puzzle: Always add character.</summary>
    public void TypeCharacter(string character)
    {
        //Debug.Log("Attempt to type character: " + character);

        if (CurrentWordExist()) //when current word is fixed
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Adventure:
                case PlayerState.Combat:
                case PlayerState.Challenge:
                    if (IsCharacterCorrect(character))
                    {
                        AddCharacter(character);
                    }
                    else
                    {
                        Mistake.Invoke();

                        TriggerSFX(SFXName.Mistake);
                    }
                    break;
                case PlayerState.Puzzle:
                    AddCharacter(character);
                    break;
                default:
                    break;
            }
        }
        else //when choosing a word
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Adventure:
                    if (isSettingName)
                    {
                        AddCharacter(character);
                    }
                    if (onBranching)
                    {
                        bool branchingDecisionComplete = false;
                        branchingDecisionComplete = DecideBranchingWordAdv.Invoke(character);

                        if (branchingDecisionComplete)
                        {
                            onBranching = false;
                            AddCharacter(character);
                        }
                        
                    }
                    break;
                case PlayerState.Combat:
                    bool actionDecisionComplete = false;
                    actionDecisionComplete = DecideActionWordCmb.Invoke(character);

                    if (actionDecisionComplete)
                    {
                        AddCharacter(character);
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }

    
    //Just to prevent unnecessary errors to the console.
    //Also used for combat when the player has to decide between 2 words: attack and dodge word.
    private bool CurrentWordExist()
    {
        return currentWord != string.Empty;
    }


    /// <summary>Verify if letter received from PlayerInput matches the one to be typed (Adventure and Combat state only).</summary>
    private bool IsCharacterCorrect(string character)
    {
        return character.ToUpper() == currentWord[nextCharacterIndex].ToString().ToUpper();
    }


    ///<summary>If letter is correct, add this letter to outputWord.</summary>
    private void AddCharacter(string character)
    {
        if (!isSettingName)
        {
            //don't do this if player is setting up their name
            //Verify if character is supposed to be displayed as lower case and if so make it upper case.
            if (IsCharacterLowerCase())
            {
                character = character.ToLower();
            }
            else
            {
                character = character.ToUpper();
            }
        }

        if (CanAppendCharacter(character))
        {
            outputWord.Append(character);
            nextCharacterIndex++;

            //add letter to word in UI
            switch (CurrentPlayerState)
            {
                case PlayerState.Adventure:
                    OutputCharacterAdv.Invoke(character);
                    break;
                case PlayerState.Combat:
                    OutputCharacterCmb.Invoke(character);
                    break;
                case PlayerState.Puzzle:
                    OutputCharacterPzl.Invoke(character);
                    break;
                case PlayerState.Challenge:
                    OutputCharacterChl.Invoke(character);
                    break;
                default:
                    break;
            }
        }

        TriggerSFX(SFXName.TypewriterKey);

        //Debug.Log($"Character typed: {character} | {outputWord}");

        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
                if (IsWordComplete(character))
                {
                    TriggerSFX(SFXName.CompleteWord);

                    CompleteWordAdv.Invoke(outputWord.ToString());
                }
                else if (HasSetupName(character))
                {
                    isSettingName = false;
                    string name = outputWord.ToString();

                    TriggerSFX(SFXName.CompleteWord);

                    CompleteWordAdv.Invoke(outputWord.ToString());

                    SetName.Invoke(name);
                }
                break;

            case PlayerState.Combat:
                if (IsWordComplete(character))
                {
                    TriggerSFX(SFXName.CompleteWord);

                    CompleteWordCmb.Invoke(outputWord.ToString());
                }

                break;

            case PlayerState.Puzzle:
                if (IsWordComplete(character))
                {
                    TriggerSFX(SFXName.PuzzleComplete);

                    CompleteWordPzl.Invoke(outputWord.ToString());
                }

                break;

            case PlayerState.Challenge:
                if (IsWordComplete(character))
                {
                    TriggerSFX(SFXName.CompleteWord);

                    CompleteWordChl.Invoke(outputWord.ToString());
                }
                break;
            default:
                break;
        }
    }

    
    private bool IsCharacterLowerCase()
    {
        return char.IsLower(currentWord[nextCharacterIndex]);
    }


    //return true when the player is typing normally or 
    //when player hasn't typed a " " when setting up their name
    //" " is used for the player to confirm their name
    private bool CanAppendCharacter(string character)
    {
        return !isSettingName || (isSettingName && character != " " && outputWord.Length < 8);
    }


    //return true when player completes a word or types their name
    private bool IsWordComplete(string character)
    {
        return outputWord.ToString() == currentWord && !isSettingName;
    }


    //check if player wants to set their name
    private bool HasSetupName(string character)
    {
        return isSettingName && character == " " && outputWord.Length >= 1;
    }


    ///<summary>Delete text typed by player (Setting Name and Puzzle only).</summary>
    public void DeleteWord()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure: //setting name only
                if (isSettingName && outputWord.Length >= 1)
                {
                    ClearWords();
                    ResetName.Invoke();
                }
                break;
            case PlayerState.Puzzle:
                //Debug.Log("Delete text typed.");
                nextCharacterIndex = 0;
                outputWord.Clear();
                ResetAnswerPzl.Invoke();
                break;
            default:
                break;
        }
    }


    private void CountTime()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
            case PlayerState.Combat:
            case PlayerState.Puzzle:
            case PlayerState.Challenge:
                AddTime?.Invoke();
                break;
            default:
                break;
        }
    }


    public void GameOver()
    {
        CurrentPlayerState = PlayerState.Dead;
        StartCoroutine(DisplayGameOverScreen());
    }

    private IEnumerator DisplayGameOverScreen()
    {
        yield return StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, false, 0f));
        yield return StartCoroutine(gfxUIManager.ActivateLoadingScreen());
        audioController.ChangeAMB(stopPlayingAll: true);
        audioController.ChangeSnapshot(SoundState.Normal);
        audioController.ChangeMusic(playOtherMusic: true, otherMusicName: OtherMusicName.GameOver);
        yield return StartCoroutine(gfxUIManager.ChangeLevelGraphics("GameOver"));
    }


    public void Victory()
    {
        CurrentPlayerState = PlayerState.CombatComplete;
    }

    public void PauseResumeGame()
    {
        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
            case PlayerState.Combat:
            case PlayerState.Puzzle:
            case PlayerState.Challenge: //resume to pause
                lastPlayerState = CurrentPlayerState;
                CurrentPlayerState = PlayerState.Pause;
                Time.timeScale = 0.0f;
                ResumePauseGame.Invoke(true);
                break;
            case PlayerState.Pause: //pause to resume
                CurrentPlayerState = lastPlayerState;
                Time.timeScale = 1.0f;
                ResumePauseGame.Invoke(false);
                break;
            case PlayerState.Options: //options to pause
                CurrentPlayerState = PlayerState.Pause;
                DisplayOptionsMenu.Invoke(false);
                break;
            default:
                break;
        }
    }

    public void DisplayOptions()
    {
        CurrentPlayerState = PlayerState.Options;
        DisplayOptionsMenu.Invoke(true);
    }

    public void EndGame()
    {
        switch (CurrentPlayerState)
        {            
            case PlayerState.EndGame:
                StartCoroutine(GameObject.FindGameObjectWithTag("EndGame").GetComponent<EndGameScreen>().LoadMainMenu());
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        CountTime();
    }
}
