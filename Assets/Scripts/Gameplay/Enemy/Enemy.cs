using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will manage the enemy actions

public class Enemy : MonoBehaviour
{
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
        timeSinceLastAttack += Time.deltaTime;
        IsAttackReady();
    }


    private void Update()
    {
        ReadyAttack();
    }
}
