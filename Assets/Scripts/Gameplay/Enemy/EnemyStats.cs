using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script that manages enemy stats.
//When battle starts it gets the enemy's stats from the enemy scriptable object

public class EnemyStats : MonoBehaviour
{
    //Variables
    private int enemyMaxHP = 4;

    private int enemyCurrentHP;

    public float AttackSpeed; //duration that the enemy takes to attack

    public bool IsBoss;

    public bool IsEnemyDead;




    //delegates
    delegate void EnemyHPBarFill(string id, float fillAmount);
    EnemyHPBarFill UpdateEnemyHPBarFill;

    delegate void BossPhase(string word);
    BossPhase ActivateBossPhase;

    delegate void ActivateWords();
    ActivateWords ActivateKillSpareChoice;

    delegate IEnumerator EndCombat();
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
        enemyCurrentHP = enemyMaxHP;
        IsBoss = enemyData.IsBoss;

        GameObject.FindGameObjectWithTag("EnemyGfx").GetComponent<SpriteRenderer>().sprite = enemyData.Sprite;
        GameObject.FindGameObjectWithTag("EnemyIcon").GetComponent<Image>().sprite = enemyData.Icon;

        if (IsBoss)
            AttackSpeed = 3f;
        else
            AttackSpeed = 4f;

        IsEnemyDead = false;

        UpdateHPBar();
    }


    public void TakeDamage()
    {
        if (!IsEnemyDead)
        {
            enemyCurrentHP -= 1;

            if (IsBoss)
            {
                //trigger boss special phase
                if (enemyCurrentHP == enemyMaxHP / 2)
                {
                    ActivateBossPhase.Invoke(gameObject.GetComponent<Enemy>().StartBossPhase());
                }
            }
            

            if (enemyCurrentHP <= 0)
            {
                EnemyDies();
            }
            else
            {
                Debug.Log($"Enemy took 1 damage | {enemyCurrentHP} HP left.");
                UpdateHPBar();
            }
        }
    }


    private void EnemyDies()
    {
        enemyCurrentHP = 0;
        UpdateHPBar();
        IsEnemyDead = true;

        if (!IsBoss) //player win
        {
            StartCoroutine(TriggerVictory.Invoke());
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
