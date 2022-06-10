using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    private Combat player;

    private PlayerStats playerStats;

    private Enemy enemy;

    private EnemyStats enemyStats;

    [SerializeField]
    private Animator mcAnimator;

    [SerializeField]
    private Animator enemyAnimator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>();
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        enemyStats = enemy.gameObject.GetComponent<EnemyStats>();
    }

    public void TriggerAttack(string attacker)
    {
        switch (attacker)
        {
            case "MC":
                player.Attack();
                enemyAnimator.SetTrigger("TakeDamage");

                if (!enemyStats.IsBoss)
                {
                    enemyAnimator.SetBool("Death", enemyStats.IsEnemyDead);
                }
                else
                {
                    enemyAnimator.SetBool("Death", false); //boss doesn't have death animation
                }
                break;
            case "Enemy":
                if (!playerStats.IsPlayerDodging)
                {
                    enemy.Attack();
                    mcAnimator.SetTrigger("TakeDamage");

                    if (playerStats.PlayerCurrentHP == 0)
                    {
                        mcAnimator.SetBool("Death", true);
                    }
                    else
                    {
                        mcAnimator.SetBool("Death", false);
                    }
                }
                else
                {
                    playerStats.IsPlayerDodging = false;
                    mcAnimator.SetTrigger(player.LastDodgeWord);
                }
                break;
            default:
                break;
        }
    }

    public void ResetPlayerPosition()
    {
        mcAnimator.gameObject.transform.GetChild(0).localPosition = Vector3.zero;
        mcAnimator.Play("Idle", 0);
    }

    public void ResetEnemyPosition()
    {
        enemyAnimator.gameObject.transform.GetChild(0).localPosition = Vector3.zero;
        enemyAnimator.Play("Idle", 0);
    }
}
