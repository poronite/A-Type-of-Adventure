using System.Text;
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

    /// <summary>Next word to be written.</summary>
    private string nextWord;

    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordAdv;

    delegate void OutputUIDelegate(string output);
    OutputUIDelegate DisplayCharacterAdv;
    OutputUIDelegate UpdateWrittenTextUIAdv;
    OutputUIDelegate UpdateRemainingTextUIAdv;
    OutputUIDelegate DisplayNewCurrentWordAdv;

    delegate void ClearOutputUIDelegate();
    ClearOutputUIDelegate ClearOutputWordAdv;



    ///<summary>Adventure starts when Graphics scene is loaded</summary>
    public void SetDelegatesAdv()
    {
        AdventureUI AdvUIController = GameObject.FindGameObjectWithTag("AdventureGfxUI").GetComponent<AdventureUI>();

        DisplayCharacterAdv += AdvUIController.DisplayNewOutputWordUIAdv;
        UpdateWrittenTextUIAdv += AdvUIController.UpdateWrittenTextUIAdv;
        UpdateRemainingTextUIAdv += AdvUIController.UpdateRemainingTextUIAdv;
        DisplayNewCurrentWordAdv += AdvUIController.DisplayNewCurrentWordUIAdv;
        ClearOutputWordAdv += AdvUIController.ClearOutputWordUIAdv;

        SendNextWordAdv += gameObject.GetComponent<Typing>().NewWord;
    }

    public void StartAdventure()
    {
        //Start of a new game (Adventure)
        NextWordAdv();
    }


    public void CompleteWordAdv()
    {
        //Every word ends with a blank space | Example: "time, " "hero "
        //since blank spaces aren't going to be used for gameplay we ignore them when sending a word,
        //but because of that it needs to be added here.
        writtenPlotText.Append(nextWord + " ");

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

            SendNextWordAdv(string.Empty);
        }
    }


    private void NextWordAdv()
    {
        int nextWordEndIndex = 0;

        nextWord = string.Empty;

        for (int i = 0; i < remainingPlotText.Length; i++)
        {
            if (IsNextWordEnd(i))
            {
                nextWordEndIndex = i;
                break; //To prevent gamebreaking loops
            }
        }

        nextWord = remainingPlotText.Substring(0, nextWordEndIndex);

        remainingPlotText = remainingPlotText.Remove(0, nextWordEndIndex).TrimStart();

        //Debug.Log($"|{newWord}| |{remainingPlotText}|");

        SendNextWordAdv(nextWord);

        UpdateRemainingTextUIAdv.Invoke(remainingPlotText);
        DisplayNewCurrentWordAdv.Invoke(nextWord);
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
