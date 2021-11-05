using System.Collections;
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
    [SerializeField]
    private new string name;

    [SerializeField]
    private LevelType type;

    [SerializeField]
    private List<string> wordKey;

    [SerializeField]
    private List<LevelTemplate> levelValue;

    [SerializeField]
    private EnemyTemplate enemy;
}
