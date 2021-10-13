using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This class will receive input from PlayerInput class and will type or delete a letter on the OutputWord string.
//It will also contain the PlayerState so that it can decide what to do with the input received.

public enum PlayerState
{
    Adventure,
    Combat,
    Puzzle
}

public class Typing : MonoBehaviour
{
    [SerializeField]
    private PlayerState currentPlayerState = PlayerState.Adventure;

    ///<summary>Word that the player has to currently type.</summary>
    private string currentWord = "word";

    ///<summary>Output of CurrentWord that the player has already typed.</summary>
    private string outputWord;

    /// <summary>Index of the next letter that needs to be typed by player.</summary>
    private int nextLetterIndex = 0;


    //Unity events
    public UnityEvent Mistake;
    public UnityEvent AddTime;

    public void TypeLetter(string letter)
    {
        switch (currentPlayerState)
        {
            case PlayerState.Adventure:
                if (IsLetterCorrect(letter))
                {
                    Debug.Log($"Letter typed: {letter}");
                }
                else
                {
                    Mistake.Invoke();
                }
                break;
            case PlayerState.Combat:
                if (IsLetterCorrect(letter))
                {
                    Debug.Log($"Letter typed: {letter}");
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

    /// <summary>Verify if letter received from PlayerInput matches the one to be typed (Adventure and Combat state only).</summary>
    private bool IsLetterCorrect(string letter)
    {
        return letter == currentWord[nextLetterIndex].ToString();
    }

    /// <summary>Delete last letter typed by player (Puzzle state only).</summary>
    public void DeleteLetter()
    {
        switch (currentPlayerState)
        {            
            case PlayerState.Puzzle:
                Debug.Log("Last Letter typed was deleted");
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
