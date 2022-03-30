using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    Adventure,
    Combat,
    Puzzle,
    Challenge
}

public enum FieldType
{
    Plains,
    Castle,
    MagicForest
}

[CreateAssetMenu(fileName = "New_Level", menuName = "New Level", order = 51)]
public class LevelTemplate : ScriptableObject
{
    //Common to all levels
    public string LevelName;
    public LevelType LevelType;
    public FieldType FieldType;


    //Adventure
    [TextArea(3, 10)]
    public string TextToType;
    public LevelTemplate NextLevelAfterAdventure;
    public bool HasBranching;

    //branching
    public List<string> PossibleChoices;
    public List<LevelTemplate> PossibleOutcomes;

    //encounters
    public int NumEncounters;
    public List<int> EncounterWordKey;
    public List<EncountersTemplate> EncounterValue;


    //Combat
    public EnemyTemplate Enemy;

    //only for common enemies
    public LevelTemplate NextLevelAfterCombat;

    //if enemy is a Boss
    public LevelTemplate NextLevelAfterKillingBoss;
    public LevelTemplate NextLevelAfterSparingBoss;


    //Puzzle
    public string CorrectWord;
    public Sprite QuestionBoard;
    public LevelTemplate NextLevelAfterPuzzle;


    //Challenge
    public float StartingEnergy;
    public float EnergyLostPerSecond;
    public float EnergyGainedPerWord;
    public LevelTemplate NextLevelAfterChallenge;
}
