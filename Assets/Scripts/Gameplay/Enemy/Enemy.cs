using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will manage the enemy actions

public class Enemy : MonoBehaviour
{
    //references and variables
    public Typing TypingController;

    private EnemyStats stats;

    private float timeSinceLastAttack;

    private string letters = "abcdefghijklmnopqrstuvwxyz";

    [SerializeField]
    private int numberCharBossWord;


    //delegates
    delegate void DealDamageDelegate();
    DealDamageDelegate DealDamage;

    delegate void EnemyAttackWordFill(float fillAmount);
    EnemyAttackWordFill UpdateEnemyAttackWordFill;

    private Animator enemyAnimator;

    private CombatAnimations combatAnimation;

    private string playerDodgeWord;

    

    //functions
    private void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
    }


    public void SetDelegatesEnemy()
    {
        DealDamage += GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().TakeDamage;
        UpdateEnemyAttackWordFill += GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateEnemyAttackWordFillUI;

        enemyAnimator = GameObject.Find("Enemy_Combat").GetComponent<Animator>();

        combatAnimation = enemyAnimator.gameObject.GetComponent<CombatAnimations>();
    }


    public void Attack()
    {
        if (!IsEnemyDead())
        {
            DealDamage.Invoke(); //enemy only deals 1 damage now
        }
    }
    // ↑
    private void IsAttackReady()
    {
        if (timeSinceLastAttack >= stats.AttackSpeed)
        {
            //trigger the attack before the player dodge
            if (playerDodgeWord != "Block")
            {
                enemyAnimator.SetTrigger("Attack");
            }
            else //trigger the dodge instantly if word is Block
            {
                combatAnimation.TriggerAttack("Enemy");
            }
            
            ResetAttackProgress();
        }
    }
    // ↑
    ///<summary>Function that readies the enemy attack over a fixed duration.</summary>
    private void ReadyAttack()
    {
        if (IsOnCombat() && !IsEnemyDead())
        {
            timeSinceLastAttack += Time.deltaTime;
            UpdateEnemyAttackWordFill.Invoke(timeSinceLastAttack / stats.AttackSpeed);
            IsAttackReady();
        }
    }


    private bool IsEnemyDead()
    {
        return stats.IsEnemyDead;
    }


    private bool IsOnCombat()
    {
        return TypingController.CurrentPlayerState == PlayerState.Combat;
    }

    
    //trigger the dodge when the player dodges
    public void ForceDodge(string dodgeWord)
    {
        playerDodgeWord = dodgeWord;
        timeSinceLastAttack = stats.AttackSpeed;
    }

    public void ResetAttackProgress()
    {
        timeSinceLastAttack = 0;
    }


    private void Update()
    {
        ReadyAttack();
    }


    //Trigger boss special phase
    public string StartBossPhase()
    {
        string finalWord = string.Empty;

        for (int i = 0; i < numberCharBossWord; i++)
        {
            finalWord += letters[Random.Range(0, letters.Length)];
        }

        return finalWord;
    }
}
