using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Template to create enemy assets that contain information for each enemy.

[CreateAssetMenu(fileName = "New_Enemy_Stats", menuName = "New Enemy", order = 52)]
public class EnemyTemplate : ScriptableObject
{
    public string Name;

    public Sprite Sprite;

    public Sprite Icon;

    public bool IsBoss;
}
