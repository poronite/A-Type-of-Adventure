using System.Text;
using UnityEngine;

//This script will be used to manage everything involving Adventure: when typing the plot of the game.
//It will be able to add written words to writtenPlotText and send next word of remainingPlotText to Typing script's currentWord

public class Adventure : MonoBehaviour
{
    //Variables
    [SerializeField]
    private string[] separators;

    ///<summary>Already written plot text.</summary>
    private StringBuilder writtenPlotText = new StringBuilder();

    ///<summary>Still not written plot text.</summary>
    private string remainingPlotText = "Once, upon a time, the hero was doing nothing.";    


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
        //Every word has either a blank space or a separator + blank space
        //since blank spaces aren't going to be used for gameplay we ignore them when sending a word,
        //but because of that it needs to be added here.
        writtenPlotText.Append(word + " ");

        NextWordAdv();
    }


    private void NextWordAdv()
    {
        int nextWordEndIndex = 0;

        string newWord;

        for (int i = 0; i < remainingPlotText.Length; i++)
        {
            for (int j = 0; j < separators.Length; j++)
            {
                if (IsNextWordEnd(separators[j], i))
                {
                    //If blank separator, just get the word
                    if (IsNotBlankSeparator(separators[j]))
                    {
                        nextWordEndIndex = i + 1;
                    }
                    else //Else the word and the separator
                    {
                        nextWordEndIndex = i;
                    }

                    //To prevent unneeded loops
                    break;
                }
            }

            //To prevent gamebreaking loops
            if (nextWordEndIndex != 0)
            {
                break;
            }
        }

        newWord = remainingPlotText.Substring(0, nextWordEndIndex);

        remainingPlotText = remainingPlotText.Remove(0, nextWordEndIndex).TrimStart();

        Debug.Log($"|{newWord}| |{remainingPlotText}|");

        SendNextWord(newWord);
    }


    /// <summary>Verifies if current remainingPlotText index is the end of a word. |
    /// The end of a word is always a separator.</summary>
    private bool IsNextWordEnd(string separator, int index)
    {
        return remainingPlotText[index].ToString() == separator;
    }


    private bool IsNotBlankSeparator(string separator)
    {
        return separator != " ";
    }
}
