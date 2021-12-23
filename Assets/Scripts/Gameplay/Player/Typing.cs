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
    CombatWon
}

public class Typing : MonoBehaviour
{
    //Variables
    public PlayerState CurrentPlayerState = PlayerState.Adventure;

    ///<summary>Is player setting their name for the first time.</summary>
    private bool isSettingName = false;

    ///<summary>Word that the player has to currently type.</summary>
    private string currentWord;

    ///<summary>Output of CurrentWord that the player has already typed.</summary>
    private StringBuilder outputWord = new StringBuilder();

    ///<summary>Index of the next character that needs to be typed by player.</summary>
    private int nextCharacterIndex = 0;

    ///<summary>First letter of the action (attack or dodge) that the player chose.</summary>
    private string firstLetterAction;


    //Unity events
    public UnityEvent AddTime;
    public UnityEvent Mistake;

    //Delegates

    delegate void SetPlayerName(string name);
    SetPlayerName SetName;

    delegate void ClearWord();
    ClearWord ResetName;
    ClearWord ResetAnswerPzl;


    //Invoke the respective state script to output character to UI
    delegate void OutputCharacterDelegate(string character);
    OutputCharacterDelegate OutputCharacterAdv;
    OutputCharacterDelegate OutputCharacterCmb;
    OutputCharacterDelegate OutputCharacterPzl;

    //when player has to decide between attack and dodge
    delegate void DecideAction(string character);
    DecideAction SendCharacterCmb;

    //When word is complete
    delegate void CompleteWordDelegate();
    CompleteWordDelegate CompleteWordAdv;
    CompleteWordDelegate CompleteWordCmb;
    CompleteWordDelegate CompleteWordPzl;



    //TEMPORARY SFX
    [SerializeField]
    private AudioSource typeSFX, mistakeSFX, wordCompleteSFX;




    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        SetName = gameObject.GetComponent<PlayerStats>().SetName;
        ResetName = gameObject.GetComponent<Adventure>().ResetName;
        ResetAnswerPzl = gameObject.GetComponent<Puzzle>().ResetAnswerPzl;
        

        OutputCharacterAdv += gameObject.GetComponent<Adventure>().AddCharacterUI;
        OutputCharacterCmb += gameObject.GetComponent<Combat>().AddCharacterUI;
        OutputCharacterPzl += gameObject.GetComponent<Puzzle>().AddCharacterUI;

        SendCharacterCmb += gameObject.GetComponent<Combat>().SetChosenWordCmb;

        CompleteWordAdv += gameObject.GetComponent<Adventure>().CompleteWordAdv;
        CompleteWordCmb += gameObject.GetComponent<Combat>().CompleteWordCmb;
        CompleteWordPzl += gameObject.GetComponent<Puzzle>().CompleteWordPzl;
    }


    ///<summary>Invoked by Adventure, Combat and Puzzle scripts' SendNextWord delegate.</summary>
    public void NewWord(string word)
    {
        ClearWords();
        currentWord = word.TrimStart();

        Debug.Log(currentWord);

        if (currentWord.TrimEnd() == "MCName")
        {
            isSettingName = true;
            currentWord = string.Empty;
            Debug.Log("Setting name");
        }

        //This is so that the player doesn't have to type
        //the first letter of the action twice when typing the action chosen.
        if (CurrentPlayerState == PlayerState.Combat)
        {
            AddCharacter(firstLetterAction);
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
        Debug.Log("Attempt to type character: " + character);

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
                    break;
                case PlayerState.Combat:
                    //store the first letter of the word of the action chosen
                    //in order to re use it so that the player doesn't have to type it again
                    firstLetterAction = character;
                    SendCharacterCmb.Invoke(character);
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

        Debug.Log($"Character typed: {character} | {outputWord}");

        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordAdv.Invoke();
                }
                else if (HasSetupName(character))
                {
                    isSettingName = false;
                    string name = outputWord.ToString();

                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordAdv.Invoke();

                    SetName.Invoke(name);
                }
                break;
            case PlayerState.Combat:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordCmb.Invoke();
                }

                break;
            case PlayerState.Puzzle:
                if (IsWordComplete(character))
                {
                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordPzl.Invoke();
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
                Debug.Log("Delete text typed.");
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
