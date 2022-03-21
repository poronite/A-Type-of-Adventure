using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that manages enemy stats.
//When battle starts it gets the enemy's stats from the enemy scriptable object

public class EnemyStats : MonoBehaviour
{
    //Variables
    private int enemyMaxHP;

    private int enemyCurrentHP;

    public int Attack;

    public float AttackSpeed; //duration that the enemy takes to attack

    private bool isBoss;

    public bool BossPhaseHappened;

    public bool IsEnemyDead;


    //delegates
    delegate void EnemyHPBarFill(string id, float fillAmount);
    EnemyHPBarFill UpdateEnemyHPBarFill;

    delegate void BossPhase(string word);
    BossPhase ActivateBossPhase;

    delegate void EndCombat();
    EndCombat ActivateKillSpareChoice;
    EndCombat TriggerVictory;


    public void SetDelegatesEnemyStats()
    {
        UpdateEnemyHPBarFill = GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateHealthBarFillUI;

        Combat cmbController = GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>();
        ActivateBossPhase = cmbController.ActivateBossPhase;
        ActivateKillSpareChoice = cmbController.ActivateKillSpareChoice;
        TriggerVictory = cmbController.WinCombat;
    }


    public void SetupEnemy(EnemyTemplate enemyData)
    {
        enemyMaxHP = enemyData.MaxHP;
        enemyCurrentHP = enemyMaxHP;
        Attack = enemyData.Attack;
        isBoss = enemyData.IsBoss;

        GameObject.FindGameObjectWithTag("EnemyGfx").GetComponent<SpriteRenderer>().sprite = enemyData.Sprite;

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

            if (isBoss)
            {
                //trigger boss special phase
                if (enemyCurrentHP <= enemyMaxHP / 2 && !BossPhaseHappened)
                {
                    BossPhaseHappened = true;
                    ActivateBossPhase.Invoke(gameObject.GetComponent<Enemy>().StartBossPhase());
                }
            }
            

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

        if (!isBoss) //player win
        {
            TriggerVictory.Invoke();
            //Debug.Log("Enemy Died");
        }
        else //trigger kill/spare choice
        {
            ActivateKillSpareChoice.Invoke();
        }
    }


    private void UpdateHPBar()
    {
        float fillAmount = (float)enemyCurrentHP / (float)enemyMaxHP;
        UpdateEnemyHPBarFill.Invoke("Enemy", fillAmount);
    }
}
