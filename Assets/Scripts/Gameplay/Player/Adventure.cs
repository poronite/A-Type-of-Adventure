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
    private string remainingPlotText;

    /// <summary>Next word to be written.</summary>
    private string nextWord;

    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordAdv;

    delegate void OutputUIDelegate(string output);
    OutputUIDelegate DisplayNewOutputWordAdv;
    OutputUIDelegate UpdateWrittenTextAdv;
    OutputUIDelegate UpdateRemainingTextAdv;
    OutputUIDelegate DisplayNewCurrentWordAdv;

    delegate void ClearUIDelegate();
    ClearUIDelegate ClearOutputWordAdv;

    delegate void ChangeLevelDelegate(string word);
    ChangeLevelDelegate ChangeToNewLevel;

    delegate void RefreshMovement();
    RefreshMovement RefreshPlayerMovement;



    ///<summary>Adventure starts when Graphics scene is loaded</summary>
    public void SetDelegatesAdv()
    {
        SendNextWordAdv += gameObject.GetComponent<Typing>().NewWord;
        ChangeToNewLevel += GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ChooseNextLevel;

        AdventureUI AdvUIController = GameObject.FindGameObjectWithTag("AdventureGfxUI").GetComponent<AdventureUI>();

        DisplayNewOutputWordAdv += AdvUIController.DisplayNewOutputWordUIAdv;
        ClearOutputWordAdv += AdvUIController.ClearOutputWordUIAdv;
        UpdateWrittenTextAdv += AdvUIController.UpdateWrittenTextUIAdv;
        UpdateRemainingTextAdv += AdvUIController.UpdateRemainingTextUIAdv;
        DisplayNewCurrentWordAdv += AdvUIController.DisplayNewCurrentWordUIAdv;

        RefreshPlayerMovement = GameObject.FindGameObjectWithTag("PlayerGfx").GetComponent<PlayerMovement>().RefreshPlayerMovementDuration;
    }

    public void StartAdventure(string textToType) //Start of a new game (Adventure)
    {
        //remove any white spaces that may cause problems
        textToType = textToType.Trim();

        //add a space at the end otherwise player won't be able to type the last word
        remainingPlotText = textToType + " ";

        //GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateAdventure();
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Adventure;
        NextWordAdv();
    }


    public void CompleteWordAdv()
    {
        //make the player sprite move for 5 seconds
        RefreshPlayerMovement.Invoke();

        //Every word ends with a blank space | Example: "time, " "hero "
        //since blank spaces aren't going to be used for gameplay we ignore them when sending a word,
        //but because of that it needs to be added here.
        writtenPlotText.Append(nextWord + " ");

        UpdateWrittenTextAdv.Invoke(writtenPlotText.ToString());

        ChangeToNewLevel.Invoke(nextWord);

        if (!IsPlotSegmentComplete())
        {
            NextWordAdv();
        }
        else
        {
            //check this after to fix bug
            DisplayNewCurrentWordAdv.Invoke(string.Empty);
            ClearOutputWordAdv.Invoke();

            Debug.Log("Plot Segment Complete.");

            SendNextWordAdv(string.Empty);

            //gameObject.GetComponent<Combat>().StartCombat();
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

        UpdateRemainingTextAdv.Invoke(remainingPlotText);
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
        DisplayNewOutputWordAdv.Invoke(character);
    }
}
