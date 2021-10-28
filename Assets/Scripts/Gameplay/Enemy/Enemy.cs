using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will manage the enemy actions

public class Enemy : MonoBehaviour
{
    public Typing TypingController;

    private EnemyStats stats;

    private float timeSinceLastAttack;


    delegate void DealDamageDelegate(int damage);
    DealDamageDelegate DealDamage;

    delegate void EnemyAttackWordFill(float time, float attackspeed);
    EnemyAttackWordFill UpdateEnemyAttackWordFill;



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
            UpdateEnemyAttackWordFill.Invoke(timeSinceLastAttack, stats.AttackSpeed);
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
}
