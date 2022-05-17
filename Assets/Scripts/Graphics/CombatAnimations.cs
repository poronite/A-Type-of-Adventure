using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    private Combat player;

    private Enemy enemy;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    public void TriggerAttack(string attacker)
    {
        switch (attacker)
        {
            case "MC":
                player.Attack();
                break;
            case "Enemy":
                enemy.Attack();
                break;
            default:
                break;
        }
    }
}
