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
    public string levelName;

    public LevelType levelType;


    //Adventure
    public string textToType;

    [SerializeField]
    private List<string> wordKey;

    [SerializeField]
    private List<LevelTemplate> levelValue;


    //Combat
    [SerializeField]
    private EnemyTemplate enemy;

    [SerializeField]
    private LevelTemplate nextLevelAfterCombat;


    //Puzzle
    [SerializeField]
    private string correctWord;

    [SerializeField]
    private LevelTemplate nextLevelAfterPuzzle;
}
