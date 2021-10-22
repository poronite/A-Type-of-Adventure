using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int CurrentHP;

    public int Attack;

    //duration that the enemy takes to attack
    public float AttackSpeed;

    public bool IsBoss;


    public void SetupEnemy(EnemyTemplate enemyData)
    {
        CurrentHP = enemyData.MaxHP;
        Attack = enemyData.Attack;
        IsBoss = enemyData.IsBoss;

        if (IsBoss)
            AttackSpeed = 1.5f;
        else
            AttackSpeed = 2.0f;
    }


    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
