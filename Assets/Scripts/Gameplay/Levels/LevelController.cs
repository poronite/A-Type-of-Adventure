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
    private List<int> wordKey;
    private List<LevelTemplate> levelValue;
    private Dictionary<int, LevelTemplate> choices = new Dictionary<int, LevelTemplate>();

    //events (for example recover player hp or trigger a cutscene etc...)
    private List<int> eventWordKey;
    private List<EncountersTemplate> eventValue;
    private Dictionary<int, EncountersTemplate> events = new Dictionary<int, EncountersTemplate>();


    //If level is of type Combat
    private EnemyTemplate enemy;

    private LevelTemplate nextLevelAfterCombat;


    //If level is of type Puzzle
    private string correctWord;

    private Sprite questionBoard;

    private LevelTemplate nextLevelAfterPuzzle;

    delegate void TriggerEncontersDelegate(EncountersTemplate eventToBeTriggered);
    TriggerEncontersDelegate TriggerEncounters;


    delegate IEnumerator LoadingScreenDelegate();
    LoadingScreenDelegate ShowLoadingScreen;

    delegate IEnumerator GraphicsDelegate(string levelType);
    GraphicsDelegate ChangeLevelGraphics;


    public void SetDelegatesLevel()
    {
        EncounterController encountersController = GameObject.FindGameObjectWithTag("EncountersController").GetComponent<EncounterController>();
        TriggerEncounters += encountersController.EncounterTriggered;

        ShowLoadingScreen += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateLoadingScreen;
        ChangeLevelGraphics += GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ChangeLevelGraphics;
    }


    public void ChangeLevel(LevelTemplate levelToLoad)
    {
        StartCoroutine(SetupLevel(levelToLoad));
    }


    IEnumerator SetupLevel(LevelTemplate levelToLoad)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Typing>().CurrentPlayerState = PlayerState.Loading;

        yield return StartCoroutine(ShowLoadingScreen.Invoke());

        DestroyGraphicsEventClones();

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

                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Adventure"));

                break;
            case LevelType.Combat:

                enemy = levelData.Enemy;
                nextLevelAfterCombat = levelData.NextLevelAfterCombat;

                //Start combat
                GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>().StartCombat(enemy, nextLevelAfterCombat);

                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Combat"));

                break;
            case LevelType.Puzzle:

                correctWord = levelData.CorrectWord;

                questionBoard = levelData.QuestionBoard;

                nextLevelAfterPuzzle = levelData.NextLevelAfterPuzzle;

                //Start puzzle

                GameObject.FindGameObjectWithTag("Player").GetComponent<Puzzle>().StartPuzzle(correctWord, questionBoard, nextLevelAfterPuzzle);

                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Puzzle"));

                break;
            default:
                break;
        }
    }


    private void CreateDictionaries()
    {
        choices = new Dictionary<int, LevelTemplate>();

        if (levelData.NumChoices >= 1)
        {
            wordKey = levelData.WordKey;
            levelValue = levelData.LevelValue;

            choices = new Dictionary<int, LevelTemplate>();

            for (int i = 0; i < wordKey.Count; i++)
            {
                choices.Add(wordKey[i], levelValue[i]);
            }
        }

        events = new Dictionary<int, EncountersTemplate>();

        if (levelData.NumEncounters >= 1)
        {
            eventWordKey = levelData.EncounterWordKey;
            eventValue = levelData.EncounterValue;

            events = new Dictionary<int, EncountersTemplate>();

            for (int i = 0; i < eventWordKey.Count; i++)
            {
                events.Add(eventWordKey[i], eventValue[i]);
            }
        }
    }


    ///<summary>Choose the next level based on the word typed by the player while adventuring.</summary>
    public void ChooseNextLevel(int word)
    {
        if (choices.ContainsKey(word))
        {
            ChangeLevel(choices[word]);
        }
    }


    ///<summary>Trigger a event based on the word typed by the player while adventuring.</summary>
    public void TriggerEncounter(int word)
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
