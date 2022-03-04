using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that creates the current level based on the stats from the scriptable object.
//Note: choices dictionary is pretty much used when there's only 1 path
//which makes the name choices sound wierd but I rather not change the name now

public class LevelController : MonoBehaviour
{
    //Variables
    private LevelTemplate levelData;

    //Common stats to all levels
    private string levelName;
    private LevelType levelType;


    //If level is of type Adventure
    private string textToType;
    private LevelTemplate nextLevelAfterAdventure;

    //branching
    private bool hasBranching;
    private List<string> possibleChoices = new List<string>();
    private List<LevelTemplate> possibleOutcomes = new List<LevelTemplate>();
    private Dictionary<string, LevelTemplate> branches = new Dictionary<string, LevelTemplate>();

    //encounters (for example recover player hp or trigger a cutscene etc...)
    private List<int> encounterWordKey;
    private List<EncountersTemplate> encounterValue;
    private Dictionary<int, EncountersTemplate> encounters = new Dictionary<int, EncountersTemplate>();


    //If level is of type Combat
    private EnemyTemplate enemy;
    private LevelTemplate nextLevelAfterCombat;


    //If level is of type Puzzle
    private string correctWord;
    private Sprite questionBoard;
    private LevelTemplate nextLevelAfterPuzzle;



    //Delegates
    delegate void TriggerEncountersDelegate(EncountersTemplate eventToBeTriggered);
    TriggerEncountersDelegate TriggerEncounters;

    delegate IEnumerator LoadingScreenDelegate();
    LoadingScreenDelegate ShowLoadingScreen;

    delegate IEnumerator GraphicsDelegate(string levelType);
    GraphicsDelegate ChangeLevelGraphics;


    //Functions
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

                //branching
                CreateBranching();

                if (!hasBranching)
                {
                    nextLevelAfterAdventure = levelData.NextLevelAfterAdventure;
                }

                //create branching and encounter dictionaries
                CreateDictionaries();

                //Start adventure
                GameObject.FindGameObjectWithTag("Player").GetComponent<Adventure>().StartAdventure(textToType, possibleChoices);
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

    private void CreateBranching()
    {
        if (levelData.PossibleChoices.Count >= 1 && levelData.PossibleOutcomes.Count >= 1)
        {
            hasBranching = true;
            possibleChoices = levelData.PossibleChoices;
            possibleOutcomes = levelData.PossibleOutcomes;
        }
        else if (levelData.PossibleChoices.Count == 0 && levelData.PossibleOutcomes.Count == 0)
        {
            hasBranching = false;
            possibleChoices = new List<string>();
            possibleOutcomes = new List<LevelTemplate>();
        }

        Debug.Log($"Branching = {hasBranching}");
    }

    private void CreateDictionaries()
    {
        branches = new Dictionary<string, LevelTemplate>();

        if (hasBranching)
        {
            for (int i = 0; i < possibleChoices.Count; i++)
            {
                branches.Add(possibleChoices[i], possibleOutcomes[i]);
            }
        }

        encounters = new Dictionary<int, EncountersTemplate>();

        if (levelData.NumEncounters >= 1)
        {
            encounterWordKey = levelData.EncounterWordKey;
            encounterValue = levelData.EncounterValue;

            for (int i = 0; i < encounterWordKey.Count; i++)
            {
                encounters.Add(encounterWordKey[i], encounterValue[i]);
            }
        }
    }


    ///<summary>Choose the next level based on the word typed by the player while adventuring.</summary>
    public void ChooseNextLevel(string word)
    {        
        if (!hasBranching)
        {
            Debug.Log("One Path");
            ChangeLevel(nextLevelAfterAdventure);
        }
        else if (hasBranching)
        {
            Debug.Log("Branched");
            if (branches.ContainsKey(word))
            {
                ChangeLevel(branches[word]);
            }
        }
    }


    ///<summary>Trigger a event based on the word typed by the player while adventuring.</summary>
    public void TriggerEncounter(int word)
    {
        if (encounters.ContainsKey(word))
        {
            TriggerEncounters.Invoke(encounters[word]);
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
