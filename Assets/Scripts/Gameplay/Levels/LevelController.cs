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

    private GameObject player;

    //Common stats to all levels
    private string levelName;
    private LevelType levelType;
    private FieldType fieldType;

    //branching
    private bool hasBranching;
    private List<string> possibleChoices = new List<string>();
    private List<LevelTemplate> possibleOutcomes = new List<LevelTemplate>();
    private Dictionary<string, LevelTemplate> branches = new Dictionary<string, LevelTemplate>();

    //encounters (for example recover player hp or trigger a cutscene etc...)
    private List<int> encounterWordKey;
    private List<EncountersTemplate> encounterValue;
    private Dictionary<int, EncountersTemplate> encounters = new Dictionary<int, EncountersTemplate>();



    //Delegates
    delegate void TriggerEncountersDelegate(EncountersTemplate eventToBeTriggered);
    TriggerEncountersDelegate TriggerEncounters;

    delegate IEnumerator LoadingScreenDelegate();
    LoadingScreenDelegate ShowLoadingScreen;

    delegate IEnumerator GraphicsDelegate(string levelType);
    GraphicsDelegate ChangeLevelGraphics;

    delegate void FieldDelegate(FieldType type);
    FieldDelegate ChangeFieldNonParalax;
    FieldDelegate ChangeFieldParalax;


    //Functions
    public void SetDelegatesLevel()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        EncounterController encountersController = GameObject.FindGameObjectWithTag("EncountersController").GetComponent<EncounterController>();
        TriggerEncounters = encountersController.EncounterTriggered;

        ShowLoadingScreen = GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ActivateLoadingScreen;
        ChangeLevelGraphics = GameObject.FindGameObjectWithTag("GfxUIManager").GetComponent<GraphicsUIManager>().ChangeLevelGraphics;

        BackgroundManager backgroundManager = FindObjectOfType<BackgroundManager>().GetComponent<BackgroundManager>();
        ChangeFieldNonParalax = backgroundManager.ChangeFieldNonParalax;
        ChangeFieldParalax = backgroundManager.ChangeFieldParalax;
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
        fieldType = levelData.FieldType;

        switch (levelType)
        {
            case LevelType.Adventure:

                //branching
                CreateBranching();

                //create branching and encounter dictionaries
                CreateDictionaries();

                ChangeFieldParalax.Invoke(fieldType);

                //Start adventure
                player.GetComponent<Adventure>().StartAdventure(levelData);
                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Adventure"));

                break;
            case LevelType.Combat:

                ChangeFieldNonParalax.Invoke(fieldType);

                //Start combat
                player.GetComponent<Combat>().StartCombat(levelData);
                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Combat"));

                break;
            case LevelType.Puzzle:

                ChangeFieldNonParalax.Invoke(fieldType);

                //Start puzzle
                player.GetComponent<Puzzle>().StartPuzzle(levelData);
                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Puzzle"));

                break;
            case LevelType.Challenge:

                ChangeFieldNonParalax.Invoke(fieldType);

                //Start challenge
                player.GetComponent<Challenge>().StartChallenge(levelData);
                yield return StartCoroutine(ChangeLevelGraphics.Invoke("Challenge"));

                break;
            default:
                break;
        }
    }

    private void CreateBranching()
    {
        hasBranching = levelData.HasBranching;

        if (hasBranching)
        {
            possibleChoices = levelData.PossibleChoices;
            possibleOutcomes = levelData.PossibleOutcomes;
        }
        else
        {
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
            ChangeLevel(levelData.NextLevelAfterAdventure);
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
