using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Enemy_Stats", menuName = "New Enemy", order = 51)]
public class EnemyTemplate : ScriptableObject
{
    [SerializeField]
    private int maxHP;

    [SerializeField]
    private int attack;
}
