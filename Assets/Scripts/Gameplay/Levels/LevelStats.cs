using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that creates the current level based on the stats from the scriptable object.

public class LevelStats : MonoBehaviour
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

    private Dictionary<string, LevelTemplate> choices;


    //If level is of type Combat
    private EnemyTemplate enemy;

    private LevelTemplate nextLevelAfterCombat;


    //If level is of type Puzzle
    private string correctWord;

    private LevelTemplate nextLevelAfterPuzzle;



    public void SetupLevel(LevelTemplate levelToLoad)
    {
        levelData = levelToLoad;

        levelName = levelData.LevelName;

        levelType = levelData.LevelType;


        switch (levelType)
        {
            case LevelType.Adventure:

                textToType = levelData.TextToType;
                wordKey = levelData.WordKey;
                levelValue = levelData.LevelValue;

                CreateChoicesDictionary();

                //Start adventure
                GameObject.FindGameObjectWithTag("Player").GetComponent<Adventure>().StartAdventure(textToType);

                break;
            case LevelType.Combat:

                enemy = levelData.Enemy;
                nextLevelAfterCombat = levelData.NextLevelAfterCombat;

                //Start combat

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
}
