using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{
    //Text bank

    public Text wordOutput, numMistakesOutput;

    private string remainingWord, currentWord = "testing";

    private int numMistakes;


    void Start()
    {
        SetCurrentWord();
    }

    private void SetCurrentWord()
    {
        // get text bank
        SetRemainingWord(currentWord);
    }

    private void SetRemainingWord(string word)
    {
        remainingWord = word;
        wordOutput.text = remainingWord.ToUpper();
    }

    void Update()
    {
        CheckInput();
    }


    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            string key = Input.inputString;

            if (key.Length == 1)
            {
                EnterLetter(key);
            }
        }
    }

    private void EnterLetter(string typedletter)
    {
        if (remainingWord.IndexOf(typedletter) == 0) //check if typedletter is correct
        {
            RemoveLetter();

            if (remainingWord.Length == 0) //is word complete?
            {
                SetCurrentWord();
            }
        }
        else //wrong letter typed
        {
            numMistakes++;
            numMistakesOutput.text = $"Number of Mistakes: {numMistakes}";
        }
    }

    private void RemoveLetter()
    {
        string newWord = remainingWord.Remove(0, 1);
        SetRemainingWord(newWord);
    }   
}
