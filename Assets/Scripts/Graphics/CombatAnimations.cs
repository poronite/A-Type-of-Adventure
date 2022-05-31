using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    private Combat player;

    private PlayerStats playerStats;

    private Enemy enemy;

    [SerializeField]
    private Animator mcAnimator;

    [SerializeField]
    private Animator enemyAnimator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>();
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    public void TriggerAttack(string attacker)
    {
        switch (attacker)
        {
            case "MC":
                player.Attack();
                enemyAnimator.SetTrigger("TakeDamage");
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
}
