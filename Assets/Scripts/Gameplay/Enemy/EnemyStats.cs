using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that manages enemy stats.
//When battle starts it gets the enemy's stats from the enemy scriptable object

public class EnemyStats : MonoBehaviour
{
    //variables
    private int enemyMaxHP;

    private int enemyCurrentHP;

    public int Attack;

    //duration that the enemy takes to attack
    public float AttackSpeed;

    private bool isBoss;

    public bool IsEnemyDead;


    //delegates
    delegate void EnemyHPBarFill(string id, float fillAmount);
    EnemyHPBarFill UpdateEnemyHPBarFill;


    public void SetDelegatesEnemyStats()
    {
        UpdateEnemyHPBarFill += GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateHealthBarFillUI;
    }


    public void SetupEnemy(EnemyTemplate enemyData)
    {
        enemyMaxHP = enemyData.MaxHP;
        enemyCurrentHP = enemyMaxHP;
        Attack = enemyData.Attack;
        isBoss = enemyData.IsBoss;

        if (isBoss)
            AttackSpeed = 1.5f;
        else
            AttackSpeed = 3.5f;

        IsEnemyDead = false;

        UpdateHPBar();
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
                UpdateHPBar();
            }
        }
    }


    private void EnemyDies()
    {
        enemyCurrentHP = 0;
        UpdateHPBar();
        IsEnemyDead = true;
        Debug.Log("Enemy Died");
    }


    private void UpdateHPBar()
    {
        float fillAmount = (float)enemyCurrentHP / (float)enemyMaxHP;
        UpdateEnemyHPBarFill.Invoke("Enemy", fillAmount);
    }
}
