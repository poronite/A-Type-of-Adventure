using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

//This script will be used to manage everything involving Adventure: when typing the plot of the game.
//It will be able to add written words to writtenPlotText and send next word of remainingPlotText to Typing script's currentWord

public class Adventure : MonoBehaviour
{
    //Variables
    [SerializeField]
    private string[] separators;

    ///<summary>Still not written plot text.</summary>
    private StringBuilder remainingPlotText;

    ///<summary>Already written plot text.</summary>
    private StringBuilder writtenPlotText;

    ///<summary>Index of the separator after next word to be sent to Typing script.</summary>
    private int nextWordEndIndex;


    //Delegates
    delegate void WordDelegate(string input);
    WordDelegate SendNextWord;


    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        SendNextWord += gameObject.GetComponent<Typing>().NewWord;
    }


    public void CompleteWordAdv(string word)
    {
        writtenPlotText.Append(word);

        NextWordAdv();
    }


    private void NextWordAdv()
    {
        string newWord;

        for (int i = 0; i < remainingPlotText.Length; i++)
        {
            for (int j = 0; j < separators.Length; j++)
            {
                if (IsNextWordEnd(separators[j]))
                {
                    //remove from remaining word end
                }
            }
        }

        //newWord = writtenPlotText.Remove(0, );
    }

    private bool IsNextWordEnd(string separator)
    {
        return remainingPlotText[i].ToString() == separator;
    }
}
