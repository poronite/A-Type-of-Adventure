﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private int enemyCurrentHP;

    public int Attack;

    //duration that the enemy takes to attack
    public float AttackSpeed;

    private bool isBoss;

    public bool IsEnemyDead;


    public void SetupEnemy(EnemyTemplate enemyData)
    {
        enemyCurrentHP = enemyData.MaxHP;
        Attack = enemyData.Attack;
        isBoss = enemyData.IsBoss;

        if (isBoss)
            AttackSpeed = 1.5f;
        else
            AttackSpeed = 3.5f;

        IsEnemyDead = false;
    }


    public void TakeDamage(int damage)
    {
        if (!IsEnemyDead)
        {
            enemyCurrentHP -= damage;
            if (enemyCurrentHP <= 0)
            {
                EnemyDies();
            }
            else
            {
                Debug.Log($"Enemy took {damage} damage | {enemyCurrentHP} HP left.");
            }
        }
    }


    private void EnemyDies()
    {
        enemyCurrentHP = 0;
        IsEnemyDead = true;
        Debug.Log("Enemy Died");
    }
}