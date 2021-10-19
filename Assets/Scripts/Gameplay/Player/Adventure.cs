﻿using System.Text;
using UnityEngine;
using UnityEngine.Events;

//This script will be used to manage everything involving Adventure: when typing the plot of the game.
//It will be able to add written words to writtenPlotText and send next word of remainingPlotText to Typing script's currentWord

public class Adventure : MonoBehaviour
{
    ///<summary>Already written plot text.</summary>
    private StringBuilder writtenPlotText = new StringBuilder();

    ///<summary>Still not written plot text.</summary>
    private string remainingPlotText = "Once, upon a time, the hero was doing nothing. ";


    //Delegates
    delegate void WordDelegate(string input);
    WordDelegate SendNextWord;

    delegate void OutputUIDelegate(string output);
    OutputUIDelegate DisplayCharacterAdv;
    OutputUIDelegate UpdateWrittenTextUIAdv;
    OutputUIDelegate UpdateRemainingTextUIAdv;
    OutputUIDelegate DisplayNewCurrentWordAdv;

    delegate void ClearOutputUIDelegate();
    ClearOutputUIDelegate ClearOutputWordAdv;



    /// <summary>Adventure starts when Graphics scene is loaded</summary>
    public void StartAdventure()
    {
        AdventureUI AdvUIController = GameObject.FindGameObjectWithTag("AdventureGfxUI").GetComponent<AdventureUI>();

        DisplayCharacterAdv += AdvUIController.DisplayNewOutputWordUIAdv;
        UpdateWrittenTextUIAdv += AdvUIController.UpdateWrittenTextUIAdv;
        UpdateRemainingTextUIAdv += AdvUIController.UpdateRemainingTextUIAdv;
        DisplayNewCurrentWordAdv += AdvUIController.DisplayNewCurrentWordUIAdv;
        ClearOutputWordAdv += AdvUIController.ClearOutputWordUIAdv;

        SendNextWord += gameObject.GetComponent<Typing>().NewWord;

        //Start of a new game (Adventure)
        NextWordAdv();
    }


    public void CompleteWordAdv(string word)
    {
        //Every word ends with a blank space | Example: "time, " "hero "
        //since blank spaces aren't going to be used for gameplay we ignore them when sending a word,
        //but because of that it needs to be added here.
        writtenPlotText.Append(word + " ");

        UpdateWrittenTextUIAdv.Invoke(writtenPlotText.ToString());

        if (!IsPlotSegmentComplete())
        {
            NextWordAdv();
        }
        else
        {
            DisplayNewCurrentWordAdv.Invoke(string.Empty);
            ClearOutputWordAdv.Invoke();

            Debug.Log("Plot Segment Complete.");

            SendNextWord(string.Empty);
        }
    }


    private void NextWordAdv()
    {
        int nextWordEndIndex = 0;

        string newWord;

        for (int i = 0; i < remainingPlotText.Length; i++)
        {
            if (IsNextWordEnd(i))
            {
                nextWordEndIndex = i;
                break; //To prevent gamebreaking loops
            }
        }

        newWord = remainingPlotText.Substring(0, nextWordEndIndex);

        remainingPlotText = remainingPlotText.Remove(0, nextWordEndIndex).TrimStart();

        Debug.Log($"|{newWord}| |{remainingPlotText}|");

        SendNextWord(newWord);

        UpdateRemainingTextUIAdv.Invoke(remainingPlotText);
        DisplayNewCurrentWordAdv.Invoke(newWord);
        ClearOutputWordAdv.Invoke();
    }


    /// <summary>Verifies if current remainingPlotText index is the end of a word. |
    /// The end of a word is always a blank space.</summary>
    private bool IsNextWordEnd(int index)
    {
        return remainingPlotText[index].ToString() == " ";
    }


    private bool IsPlotSegmentComplete()
    {
        return remainingPlotText == string.Empty;
    }


    ///<summary>Send character to AdventureUI script to be added to the word being displayed</summary>
    public void AddCharacterUIAdv(string character)
    {
        DisplayCharacterAdv.Invoke(character);
    }

    
}
