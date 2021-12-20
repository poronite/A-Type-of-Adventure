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

    //choices
    private List<string> wordKey;
    private List<LevelTemplate> levelValue;
    private Dictionary<string, LevelTemplate> choices = new Dictionary<string, LevelTemplate>();

    //events (for example recover player hp or trigger an animation etc...)
    private List<string> eventWordKey;
    private List<EncountersTemplate> eventValue;
    private Dictionary<string, EncountersTemplate> events = new Dictionary<string, EncountersTemplate>();


    //If level is of type Combat
    private EnemyTemplate enemy;

    private LevelTemplate nextLevelAfterCombat;


    //If level is of type Puzzle
    private string correctWord;

    private Sprite questionBoard;

    private LevelTemplate nextLevelAfterPuzzle;

    delegate void TriggerEncontersDelegate(EncountersTemplate eventToBeTriggered);
    TriggerEncontersDelegate TriggerEncounters;

    delegate void SetPlayerMovement(bool movementState);
    SetPlayerMovement AllowPlayerMovement;


    delegate void LoadingScreenDelegate();
    LoadingScreenDelegate ShowLoadingScreen;

    delegate void GraphicsDelegate(string levelType);
    GraphicsDelegate ChangeLevelGraphics;


    public void SetDelegatesLevel()
    {
        EncounterController encountersController = GameObject.FindGameObjectWithTag("EncountersController").GetComponent<EncounterController>();
        TriggerEncounters += encountersController.EnconterTriggered;
        AllowPlayerMovement += encountersController.SetPlayerMovement;

        ShowLoadingScreen += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateLoadingScreen;
        ChangeLevelGraphics += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ChangeLevelGraphics;
    }


    public void SetupLevel(LevelTemplate levelToLoad)
    {
        ShowLoadingScreen.Invoke();

        DestroyGraphicsEventClones();
        AllowPlayerMovement(false);

        levelData = levelToLoad;

        levelName = levelData.LevelName;

        levelType = levelData.LevelType;


        switch (levelType)
        {
            case LevelType.Adventure:

                textToType = levelData.TextToType;

                CreateDictionaries();

                //Start adventure
                GameObject.FindGameObjectWithTag("Player").GetComponent<Adventure>().StartAdventure(textToType);

                ChangeLevelGraphics.Invoke("Adventure");

                break;
            case LevelType.Combat:

                enemy = levelData.Enemy;
                nextLevelAfterCombat = levelData.NextLevelAfterCombat;

                //Start combat
                GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>().StartCombat(enemy, nextLevelAfterCombat);

                ChangeLevelGraphics.Invoke("Combat");

                break;
            case LevelType.Puzzle:

                correctWord = levelData.CorrectWord;

                questionBoard = levelData.QuestionBoard;

                nextLevelAfterPuzzle = levelData.NextLevelAfterPuzzle;

                //Start puzzle

                GameObject.FindGameObjectWithTag("Player").GetComponent<Puzzle>().StartPuzzle(correctWord, questionBoard, nextLevelAfterPuzzle);

                ChangeLevelGraphics.Invoke("Puzzle");

                break;
            default:
                break;
        }
    }


    private void CreateDictionaries()
    {
        if (levelData.NumChoices >= 1)
        {
            wordKey = levelData.WordKey;
            levelValue = levelData.LevelValue;

            choices = new Dictionary<string, LevelTemplate>();

            for (int i = 0; i < wordKey.Count; i++)
            {
                choices.Add(wordKey[i], levelValue[i]);
            }
        }

        if (levelData.NumEvents >= 1)
        {
            eventWordKey = levelData.EventWordKey;
            eventValue = levelData.EventValue;

            events = new Dictionary<string, EncountersTemplate>();

            for (int i = 0; i < eventWordKey.Count; i++)
            {
                events.Add(eventWordKey[i], eventValue[i]);
            }
        }
    }


    ///<summary>Choose the next level based on the word typed by the player while adventuring.</summary>
    public void ChooseNextLevel(string word)
    {
        if (choices.ContainsKey(word))
        {
            SetupLevel(choices[word]);
        }
    }


    ///<summary>Trigger a event based on the word typed by the player while adventuring.</summary>
    public void TriggerEnconter(string word)
    {
        if (events.ContainsKey(word))
        {
            TriggerEncounters.Invoke(events[word]);
        }
    }

    
    //destroy the objects such as sprites that are spawned at certain points of the game
    private void DestroyGraphicsEventClones()
    {
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("GraphicsEventClone");

        foreach (GameObject item in objectsToDelete)
        {
            Destroy(item);
        }
    }
}
