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



    private void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
    }


    public void SetDelegatesEnemy()
    {
        DealDamage += GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().TakeDamage;
    }


    private void Attack()
    {
        DealDamage.Invoke(stats.Attack);
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
        if (IsOnCombat())
        {
            timeSinceLastAttack += Time.deltaTime;
            IsAttackReady();
        }
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
