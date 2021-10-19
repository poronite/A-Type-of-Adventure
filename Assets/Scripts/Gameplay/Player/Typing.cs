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
    Puzzle
}

public class Typing : MonoBehaviour
{
    //Variables
    [SerializeField]
    private PlayerState currentPlayerState = PlayerState.Adventure;

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
    //Invoke the respective state script to output character to UI
    delegate void OutputCharacterDelegate(string character);
    OutputCharacterDelegate OutputCharacterAdv;
    OutputCharacterDelegate OutputCharacterCmb;
    OutputCharacterDelegate OutputCharacterPzl;

    //When word is complete
    delegate void CompleteWordDelegate(string input);
    CompleteWordDelegate CompleteWordAdv;
    CompleteWordDelegate CompleteWordCmb;
    CompleteWordDelegate CompleteWordPzl;

    


    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        OutputCharacterAdv += gameObject.GetComponent<Adventure>().AddCharacterUIAdv;
        //OutputCharacterCmb += gameObject.GetComponent<Combat>().AddCharacterUICmb;
        //OutputCharacterPzl += gameObject.GetComponent<Puzzle>().AddCharacterUIPzl;

        CompleteWordAdv += gameObject.GetComponent<Adventure>().CompleteWordAdv;
        //CompleteWordCmb += gameObject.GetComponent<Combat>().CompleteWordCmb;
        //CompleteWordPzl += gameObject.GetComponent<Puzzle>().CompleteWordPzl;
    }


    ///<summary>Invoked by Adventure, Combat and Puzzle scripts' SendNextWord delegate.</summary>
    public void NewWord(string word)
    {
        currentWord = word;
        nextCharacterIndex = 0;
        outputWord.Clear();
    }


    ///<summary>Invoked by PlayerInput script's SendInput Delegate when player types a letter. | 
    ///Adventure and Combat: If correct AddCharacter, if not mistake += 1. |
    ///Puzzle: Always add character.</summary>
    public void TypeCharacter(string character)
    {
        if (CurrentWordExist())
        {
            switch (currentPlayerState)
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
                    }
                    break;
                case PlayerState.Puzzle:
                    break;
                default:
                    break;
            }
        }
    }

    
    //Just to prevent unnecessary errors to the console
    private bool CurrentWordExist()
    {
        return currentWord != "";
    }


    /// <summary>Verify if letter received from PlayerInput matches the one to be typed (Adventure and Combat state only).</summary>
    private bool IsCharacterCorrect(string character)
    {
        return character == currentWord[nextCharacterIndex].ToString().ToLower(); //Always lower case
    }


    /// <summary>If letter is correct, add this letter to outputWord.</summary>
    private void AddCharacter(string character)
    {
        //Verify if character is supposed to be displayed as upper case and if so make it upper case.
        if (IsCharacterUpperCase())
        {
            character = character.ToUpper();
        }

        outputWord.Append(character);
        nextCharacterIndex++;

        Debug.Log($"Character typed: {character}");

        switch (currentPlayerState)
        {
            case PlayerState.Adventure:
                OutputCharacterAdv.Invoke(character);

                if (IsWordComplete())
                {
                    Debug.Log($"outputWord: {outputWord}");
                    CompleteWordAdv.Invoke(currentWord.ToString());
                }
                break;
            case PlayerState.Combat:
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
        switch (currentPlayerState)
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
        switch (currentPlayerState)
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


    private void Update()
    {
        CountTime();
    }
}
