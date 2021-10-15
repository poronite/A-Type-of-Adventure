using System.Text;
using UnityEngine;

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


    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
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

        if (!IsPlotSegmentComplete())
        {
            NextWordAdv();
        }
        else
        {
            Debug.Log("Plot Segment Complete.");
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
}
