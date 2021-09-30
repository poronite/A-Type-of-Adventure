using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{   
    public Text currentWordOutput, writtenWordsOutput, notWrittenWordsOutput, numMistakesOutput;

    private List<string> words = new List<string>();

    private string remainingWord, currentWord;

    private int numMistakes;

    [SerializeField]
    private string mainText;


    void Start()
    {
        SetText();
    }

    private void SetText()
    {
        notWrittenWordsOutput.text = mainText;

        SetCurrentWord();
    }

    private void SetCurrentWord()
    {
        char[] separators = {' ', '.'};

        int wordEnd = notWrittenWordsOutput.text.IndexOf(' ');

        currentWord = notWrittenWordsOutput.text.Substring(0, wordEnd);

        SetRemainingWord(currentWord);

        notWrittenWordsOutput.text = notWrittenWordsOutput.text.Remove(0, wordEnd + 1);
    }

    private void SetRemainingWord(string word)
    {
        remainingWord = word;
        currentWordOutput.text = remainingWord;
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
                NextWord();
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
    
    private void NextWord()
    {
        writtenWordsOutput.text += $"{currentWord} ";

        SetCurrentWord();
    }
}
