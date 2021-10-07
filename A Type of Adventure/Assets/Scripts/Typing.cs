using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typing : MonoBehaviour
{   
    public Text currentWordOutput, writtenWordsOutput, notWrittenWordsOutput, numMistakesOutput;

    private string remainingWord, currentWord;

    private char wordSplitter;

    private int numMistakes;


    public void SetText(string text)
    {
        notWrittenWordsOutput.text = text;

        SetCurrentWord();
    }

    private void SetCurrentWord()
    {
        char[] wordSpiltters = {' ', '.', '!', '?', ':', ',', ';'};

        int wordEnd = 0;

        //find end of the next word
        for (int i = 0; i < notWrittenWordsOutput.text.Length; i++)
        {
            for (int j = 0; j < wordSpiltters.Length; j++)
            {
                if (notWrittenWordsOutput.text[i] == wordSpiltters[j])
                {
                    wordSplitter = wordSpiltters[j];

                    if (wordSplitter == ' ')
                    {
                        wordEnd = i; //get word without spiltter
                    }
                    else
                    {
                        wordEnd = i + 1; //get word with spiltter
                    }

                    break;
                }
            }

            if (wordEnd != 0)
            {
                break;
            }
        }

        currentWord = notWrittenWordsOutput.text.Substring(0, wordEnd);

        SetRemainingWord(currentWord);

        notWrittenWordsOutput.text = notWrittenWordsOutput.text.Remove(0, wordEnd).TrimStart();
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
        if (remainingWord.ToLower().IndexOf(typedletter) == 0) //check if typedletter is correct | always lower case
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
            numMistakesOutput.text = $"Mistakes: {numMistakes}";
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
