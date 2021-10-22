using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Template to create enemy assets that contain information for each enemy.

[CreateAssetMenu(fileName = "New_Enemy_Stats", menuName = "New Enemy", order = 51)]
public class EnemyTemplate : ScriptableObject
{
    [SerializeField]
    private int maxHP;

    [SerializeField]
    private int attack;

    [SerializeField]
    private bool isBoss;

    public int MaxHP
    {
        get => maxHP;
    }

    public int Attack
    {
        get => attack;
    }

    public bool IsBoss
    {
        get => isBoss;
    }
}
