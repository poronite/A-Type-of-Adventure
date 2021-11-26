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

    public int NumEvents;

    public List<string> WordKey;

    public List<LevelTemplate> LevelValue;


    //Combat
    public EnemyTemplate Enemy;

    public LevelTemplate NextLevelAfterCombat;


    //Puzzle
    public string CorrectWord;

    public LevelTemplate NextLevelAfterPuzzle;
}
