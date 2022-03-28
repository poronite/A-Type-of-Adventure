using System.Text;
using System.Collections.Generic;
using UnityEngine;

//This script will be used to manage everything involving Adventure: when typing the plot of the game.
//It will be able to add written words to writtenPlotText and send next word of remainingPlotText to Typing script's currentWord

public class Adventure : MonoBehaviour
{
    private string playerName = string.Empty;

    ///<summary>Already written plot text.</summary>
    private StringBuilder writtenPlotText = new StringBuilder();

    ///<summary>Still not written plot text.</summary>
    private string remainingPlotText;

    //number of words written in current level
    private int numWordsWritten = 0;

    ///<summary>Next word to be written.</summary>
    private string nextWord;

    //words used in branching for current adventure level
    private List<string> branchingWords = new List<string>();

    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordAdv;

    delegate void OutputUIDelegate(string output);
    OutputUIDelegate AddCharacterAdv;
    OutputUIDelegate UpdateWrittenTextAdv;
    OutputUIDelegate UpdateRemainingTextAdv;
    OutputUIDelegate DisplayNewCurrentWordAdv;
    OutputUIDelegate DisplayNewHintAdv;
    OutputUIDelegate DisplayChosenBranchingWordAdv;

    delegate void ClearUIDelegate();
    ClearUIDelegate ClearOutputWordAdv;
    ClearUIDelegate ClearCurrentWordAdv;

    delegate void LevelDelegate(string word);
    LevelDelegate ChangeToNewLevel;

    delegate void EncounterDelegate(int word);
    EncounterDelegate TriggerEncounter;

    delegate void BranchingDelegate(List<string> words);
    BranchingDelegate DisplayBranchingWordsAdv;

    delegate void RefreshMovement();
    RefreshMovement RefreshPlayerMovement;



    ///<summary>Adventure starts when Graphics scene is loaded.</summary>
    public void SetDelegatesAdv()
    {
        SendNextWordAdv = gameObject.GetComponent<Typing>().NewWord;

        LevelController LevelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        ChangeToNewLevel = LevelController.ChooseNextLevel;
        TriggerEncounter = LevelController.TriggerEncounter;

        AdventureUI AdvUIController = GameObject.FindGameObjectWithTag("AdventureGfxUI").GetComponent<AdventureUI>();
        AddCharacterAdv = AdvUIController.AddCharacterUIAdv;
        ClearOutputWordAdv = AdvUIController.ClearOutputWordUIAdv;
        ClearCurrentWordAdv = AdvUIController.ClearCurrentWordUIAdv;
        UpdateWrittenTextAdv = AdvUIController.UpdateWrittenTextUIAdv;
        UpdateRemainingTextAdv = AdvUIController.UpdateRemainingTextUIAdv;
        DisplayNewCurrentWordAdv = AdvUIController.DisplayNewCurrentWordUIAdv;
        DisplayNewHintAdv = AdvUIController.DisplayNewHintUIAdv;
        DisplayBranchingWordsAdv = AdvUIController.DisplayBranchingWordsUIAdv;
        DisplayChosenBranchingWordAdv = AdvUIController.DisplayChosenBranchingWordUIAdv;

        RefreshPlayerMovement = GameObject.FindGameObjectWithTag("PlayerGfx").GetComponent<PlayerMovementAdv>().RefreshPlayerMovementDuration;
    }


    public void StartAdventure(string textToType, List<string> words) //Start of a new game (Adventure)
    {
        numWordsWritten = 0;
        TriggerEncounter.Invoke(numWordsWritten);

        //remove any white spaces that may cause problems
        textToType = textToType.Trim();

        //array for easy access to the branching words in case current adventure level has branching
        branchingWords = words;

        //add a space at the end otherwise player won't be able to type the last word
        remainingPlotText = textToType;

        if (playerName != string.Empty)
        {
            InsertNameIntoText();
        }

        //GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateAdventure();
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Adventure;
        NextWordAdv();
    }


    public void SetPlayerName(string name)
    {
        playerName = name;
        UpdateHintUI(string.Empty);
        Debug.Log("replacing name");
        InsertNameIntoText();
    }


    public void ResetName()
    {
        ClearOutputWordAdv.Invoke();
    }
    

    public void InsertNameIntoText()
    {
        writtenPlotText = writtenPlotText.Replace("MCName", playerName);
        UpdateWrittenTextAdv.Invoke(writtenPlotText.ToString());

        remainingPlotText = remainingPlotText.Replace("MCName", playerName);
        UpdateRemainingTextAdv.Invoke(remainingPlotText);
    }


    public void CompleteWordAdv(string word)
    {
        numWordsWritten += 1;

        //make the player sprite move for a few seconds
        RefreshPlayerMovement.Invoke();

        //this is to add a space to the end of the last word in a level
        nextWord = word.Trim() + " ";

        //Every word ends with a blank space | Example: "time, " "hero "
        writtenPlotText.Append(nextWord);

        UpdateWrittenTextAdv.Invoke(writtenPlotText.ToString());

        TriggerEncounter.Invoke(numWordsWritten);

        if (!IsPlotSegmentComplete())
        {
            NextWordAdv();
        }
        else
        {
            Debug.Log("Plot Segment Complete.");

            DisplayNewCurrentWordAdv.Invoke(string.Empty);
            ClearOutputWordAdv.Invoke();
            SendNextWordAdv(string.Empty);

            ChangeToNewLevel.Invoke(word);
        }
    }


    private void NextWordAdv()
    {
        int nextWordEndIndex = 0;

        nextWord = string.Empty;

        for (int i = 0; i <= remainingPlotText.Length; i++)
        {
            if (i == remainingPlotText.Length) //if player reaches end of level
            {
                nextWordEndIndex = i;
                break; //To prevent gamebreaking loops
            }

            if (IsNextWordEnd(i)) //if player reaches end of word
            {
                nextWordEndIndex = i + 1; //include space bar
                break; //To prevent gamebreaking loops
            } 
        }

        nextWord = remainingPlotText.Substring(0, nextWordEndIndex);

        remainingPlotText = remainingPlotText.Remove(0, nextWordEndIndex).TrimStart();

        Debug.Log($"|{nextWord}| |{remainingPlotText}|");

        //verify if next word contains a branching path
        if (nextWord.Trim() == "*")
        {
            DisplayBranchingWordsAdv.Invoke(branchingWords);
        }

        UpdateRemainingTextAdv.Invoke(remainingPlotText);
        DisplayNewCurrentWordAdv.Invoke(nextWord);

        SendNextWordAdv(nextWord);        
        
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
    public void AddCharacterUI(string character)
    {
        AddCharacterAdv.Invoke(character);
    }


    public void UpdateHintUI(string hint)
    {
        DisplayNewHintAdv.Invoke(hint);
    }


    public void ClearCurrentWord()
    {
        ClearCurrentWordAdv.Invoke();
    }


    //branching
    public bool SetBranchingWord(string character)
    {
        foreach (string word in branchingWords)
        {
            if (word[0].ToString() == character)
            {
                SendNextWordAdv(word);
                DisplayChosenBranchingWordAdv.Invoke(word);
                return true;
            }
        }

        return false;
    }
}
