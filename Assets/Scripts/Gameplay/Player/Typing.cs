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

    ///<summary>Word that the player has to currently type.</summary>
    private string currentWord;

    ///<summary>Output of CurrentWord that the player has already typed.</summary>
    private StringBuilder outputWord = new StringBuilder();

    ///<summary>Index of the next character that needs to be typed by player.</summary>
    private int nextCharacterIndex = 0;

    /// <summary>First letter of the action (attack or dodge) that the player chose.</summary>
    private string firstLetterAction;


    //Unity events
    public UnityEvent AddTime;
    public UnityEvent Mistake;

    //Delegates
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
    //CompleteWordDelegate CompleteWordPzl;


    //TEMPORARY SFX
    [SerializeField]
    private AudioSource typeSFX, mistakeSFX, wordCompleteSFX;




    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        OutputCharacterAdv += gameObject.GetComponent<Adventure>().AddCharacterUIAdv;
        OutputCharacterCmb += gameObject.GetComponent<Combat>().AddCharacterUICmb;
        //OutputCharacterPzl += gameObject.GetComponent<Puzzle>().AddCharacterUIPzl;

        SendCharacterCmb += gameObject.GetComponent<Combat>().SetChosenWordCmb;

        CompleteWordAdv += gameObject.GetComponent<Adventure>().CompleteWordAdv;
        CompleteWordCmb += gameObject.GetComponent<Combat>().CompleteWordCmb;
        //CompleteWordPzl += gameObject.GetComponent<Puzzle>().CompleteWordPzl;
    }


    ///<summary>Invoked by Adventure, Combat and Puzzle scripts' SendNextWord delegate.</summary>
    public void NewWord(string word)
    {
        ClearWords();
        currentWord = word.Trim();

        Debug.Log(currentWord);

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
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (CurrentPlayerState)
            {
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
        //Verify if character is supposed to be displayed as upper case and if so make it upper case.
        if (IsCharacterUpperCase())
        {
            character = character.ToUpper();
        }

        outputWord.Append(character);
        nextCharacterIndex++;

        //TEMPORARY SOUND EFFECT
        typeSFX.Play();

        Debug.Log($"Character typed: {character} | {outputWord}");

        switch (CurrentPlayerState)
        {
            case PlayerState.Adventure:
                OutputCharacterAdv.Invoke(character);

                if (IsWordComplete())
                {
                    //Debug.Log($"outputWord: {outputWord}");

                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordAdv.Invoke();
                }
                break;
            case PlayerState.Combat:
                OutputCharacterCmb.Invoke(character);

                if (IsWordComplete())
                {
                    //Debug.Log("Word Completed");

                    //TEMPORARY SOUND EFFECT
                    wordCompleteSFX.Play();

                    CompleteWordCmb.Invoke();
                }

                break;
            case PlayerState.Puzzle:
                break;
            default:
                break;
        }
    }

    
    private bool IsCharacterUpperCase()
    {
        return char.IsUpper(currentWord[nextCharacterIndex]);
    }


    private bool IsWordComplete()
    {
        return outputWord.ToString() == currentWord;
    }


    /// <summary>Delete last character typed by player (Puzzle state only).</summary>
    public void DeleteLetter()
    {
        switch (CurrentPlayerState)
        {            
            case PlayerState.Puzzle:
                Debug.Log("Last character typed was deleted");
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
