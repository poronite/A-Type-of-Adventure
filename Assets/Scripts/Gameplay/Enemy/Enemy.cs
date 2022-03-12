﻿using System.Collections;
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
    delegate void DealDamageDelegate(int damage);
    DealDamageDelegate DealDamage;

    delegate void EnemyAttackWordFill(float fillAmount);
    EnemyAttackWordFill UpdateEnemyAttackWordFill;

    

    //functions
    private void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
    }


    public void SetDelegatesEnemy()
    {
        DealDamage += GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().TakeDamage;
        UpdateEnemyAttackWordFill += GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateEnemyAttackWordFillUI;
    }


    private void Attack()
    {
        if (!IsEnemyDead())
        {
            DealDamage.Invoke(stats.Attack);
        }
    }
    // ↑
    private void IsAttackReady()
    {
        if (timeSinceLastAttack >= stats.AttackSpeed)
        {
            Attack();
            timeSinceLastAttack = 0;
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
