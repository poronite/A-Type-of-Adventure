using System.Text;
using UnityEngine;
using UnityEngine.Events;

//This class will receive input from PlayerInput class and will type or delete a letter on the OutputWord string.
//It will also contain the PlayerState so that it can decide what to do with the input received.
//It can be called the hub of the player's actions in the game.

public enum PlayerState
{
    Adventure,
    Combat,
    Puzzle,
    Dead,
    CombatWon,
    Loading
}

public class Typing : MonoBehaviour
{
    //Variables
    public PlayerState CurrentPlayerState = PlayerState.Adventure;

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


    //Unity events
    public UnityEvent AddTime;
    public UnityEvent Mistake;

    //Delegates

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

    //when player has to decide between attack and dodge or between branching words
    delegate bool DecideWord(string character);
    DecideWord DecideBranchingWordAdv;
    DecideWord DecideActionWordCmb;

    //When word is complete
    delegate void CompleteWordDelegate(string word);
    CompleteWordDelegate CompleteWordAdv;
    CompleteWordDelegate CompleteWordCmb;
    CompleteWordDelegate CompleteWordPzl;

    delegate void UpdateHint(string hint);
    UpdateHint UpdateHintAdvUI;



    //TEMPORARY SFX
    [SerializeField]
    private AudioSource typeSFX, mistakeSFX, wordCompleteSFX;




    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        PlayerStats stats = gameObject.GetComponent<PlayerStats>();
        Adventure advController = gameObject.GetComponent<Adventure>();
        Combat cmbController = gameObject.GetComponent<Combat>();
        Puzzle pzlController = gameObject.GetComponent<Puzzle>();


        SetName = stats.SetName;
        ResetName = advController.ResetName;
        ResetAnswerPzl = pzlController.ResetAnswerPzl;
        ClearCurrentWordAdv = advController.ClearCurrentWord;

        OutputCharacterAdv = advController.AddCharacterUI;
        OutputCharacterCmb = cmbController.AddCharacterUI;
        OutputCharacterPzl = pzlController.AddCharacterUI;

        DecideBranchingWordAdv = advController.SetBranchingWord;
        DecideActionWordCmb = cmbController.SetChosenWordCmb;

        CompleteWordAdv = advController.CompleteWordAdv;
        CompleteWordCmb = cmbController.CompleteWordCmb;
        CompleteWordPzl = pzlController.CompleteWordPzl;

        UpdateHintAdvUI = advController.UpdateHintUI;
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

        if (CurrentWordExist())
        {
            switch (CurrentPlayerState)
            {
                case PlayerState.Adventure:
                case PlayerState.Combat:
                    if (IsCharacterCorrect(character))
                    {
                        AddCharacter(character);
                    }
                    else
                    {
                        Mistake.Invoke();

                        //TEMPORARY SOUND EFFECT
                        mistakeSFX.Play();
                    }
                    break;
                case PlayerState.Puzzle:
                    AddCharacter(character);
                    break;
                default:
                    break;
            }
        }
        else
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
        return character == currentWord[nextCharacterIndex].ToString().ToLower(); //Always lower case
    }


    ///<summary>If letter is correct, add this letter to outputWord.</summary>
    private void AddCharacter(string character)
    {
        if (!isSettingName)
        {
            //don't do this if player is setting up their name or doing a puzzle
            //Verify if character is supposed to be displayed as upper case and if so make it upper case.
            if (IsCharacterUpperCase())
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
                default:
                    break;
            }
        }

        //TEMPORARY SOUND EFFECT
        typeSFX.Play();

        //Debug.Log($"Character typed: {character} | {outputWord}");

        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordAdv.Invoke(outputWord.ToString());
                }
                else if (HasSetupName(character))
                {
                    isSettingName = false;
                    string name = outputWord.ToString();

                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordAdv.Invoke(outputWord.ToString());

                    SetName.Invoke(name);
                }
                break;
            case PlayerState.Combat:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordCmb.Invoke(outputWord.ToString());
                }

                break;
            case PlayerState.Puzzle:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordPzl.Invoke(outputWord.ToString());
                }

                break;
            default:
                break;
        }
    }

    
    private bool IsCharacterUpperCase()
    {
        return char.IsUpper(currentWord[nextCharacterIndex]);
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
            case PlayerState.Adventure:
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
                AddTime.Invoke();
                break;
            default:
                break;
        }
    }


    public void GameOver()
    {
        CurrentPlayerState = PlayerState.Dead;
    }


    public void Victory()
    {
        CurrentPlayerState = PlayerState.CombatWon;
    }


    private void Update()
    {
        CountTime();
    }
}
