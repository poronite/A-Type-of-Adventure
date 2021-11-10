using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that creates the current level based on the stats from the scriptable object.

public class LevelController : MonoBehaviour
{
    //Variables
    //Common stats
    private LevelTemplate levelData;

    private string levelName;

    private LevelType levelType;


    //If level is of type Adventure
    private string textToType;

    private List<string> wordKey;

    private List<LevelTemplate> levelValue;

    private Dictionary<string, LevelTemplate> choices = new Dictionary<string, LevelTemplate>();


    //If level is of type Combat
    private EnemyTemplate enemy;

    private LevelTemplate nextLevelAfterCombat;


    //If level is of type Puzzle
    private string correctWord;

    private LevelTemplate nextLevelAfterPuzzle;


    delegate void GraphicsDelegate();
    GraphicsDelegate ShowLoadingScreen;
    GraphicsDelegate ShowAdventure;
    GraphicsDelegate ShowCombat;
    //UIManagerDelegate ShowPuzzle;


    public void SetDelegatesLevel()
    {
        ShowLoadingScreen += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateLoadingScreen;
        ShowAdventure += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateAdventure;
        ShowCombat += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateCombat;
    }


    public void SetupLevel(LevelTemplate levelToLoad)
    {
        ShowLoadingScreen.Invoke();

        levelData = levelToLoad;

        levelName = levelData.LevelName;

        levelType = levelData.LevelType;


        switch (levelType)
        {
            case LevelType.Adventure:

                textToType = levelData.TextToType;
                
                if (levelData.NumChoices >= 1)
                {
                    wordKey = levelData.WordKey;
                    levelValue = levelData.LevelValue;
                    CreateChoicesDictionary();
                }

                //Start adventure
                GameObject.FindGameObjectWithTag("Player").GetComponent<Adventure>().StartAdventure(textToType);

                ShowAdventure.Invoke();

                break;
            case LevelType.Combat:

                enemy = levelData.Enemy;
                nextLevelAfterCombat = levelData.NextLevelAfterCombat;

                //Start combat
                GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>().StartCombat(enemy, nextLevelAfterCombat);

                ShowCombat.Invoke();

                break;
            case LevelType.Puzzle:

                correctWord = levelData.CorrectWord;
                nextLevelAfterPuzzle = levelData.NextLevelAfterPuzzle;

                //Start puzzle

                break;
            default:
                break;
        }
    }


    private void CreateChoicesDictionary()
    {
        for (int i = 0; i < wordKey.Count; i++)
        {
            choices.Add(wordKey[i], levelValue[i]);
        }
    }


    /// <summary>Choose the next level based on word typed by the player while adventuring.</summary>
    public void ChooseNextLevel(string word)
    {
        if (choices.ContainsKey(word))
        {
            SetupLevel(choices[word]);
        }
    }
}
