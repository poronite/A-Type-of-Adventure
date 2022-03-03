using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    Adventure,
    Combat,
    Puzzle
}

[CreateAssetMenu(fileName = "New_Level", menuName = "New Level", order = 51)]
public class LevelTemplate : ScriptableObject
{
    //Common to all levels
    public string LevelName;
    public LevelType LevelType;


    //Adventure
    [TextArea(3, 10)]
    public string TextToType;
    public LevelTemplate NextLevelAfterAdventure;

    //branching
    public List<string> PossibleChoices;
    public List<LevelTemplate> PossibleOutcomes;

    //encounters
    public int NumEncounters;
    public List<int> EncounterWordKey;
    public List<EncountersTemplate> EncounterValue;


    //Combat
    public EnemyTemplate Enemy;
    public LevelTemplate NextLevelAfterCombat;


    //Puzzle
    public string CorrectWord;
    public Sprite QuestionBoard;
    public LevelTemplate NextLevelAfterPuzzle;
}
