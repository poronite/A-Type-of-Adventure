using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script will be used to manage everything involving Combat: when fighting against an enemy.
//

public enum Actions
{
    None,
    Attack,
    Dodge
}

public class Combat : MonoBehaviour
{
    private PlayerStats stats;

    public EnemyTemplate enemy;

    private Actions actionChosen;

    private string attackWordText = "Attack";
    private string dodgeWordText = "Dodge";


    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordCmb;

    delegate void AttackDelegate(int damage);
    AttackDelegate AttackEnemy;

    [SerializeField]
    private UnityEvent ActivateDodge; 


    private void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }


    public void SetDelegatesCmb()
    {
        SendNextWordCmb += gameObject.GetComponent<Typing>().NewWord;
        AttackEnemy += GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().TakeDamage;
    }


    public void StartCombat()
    {
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Combat;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().SetupEnemy(enemy);
        Debug.Log("Started combat.");
    }


    /// <summary>Send to Typing script the word that the player chose.</summary>
    public void SetChosenWordCmb(string character)
    {
        actionChosen = Actions.None;

        string decidedWord = DecideAction(character);

        SendNextWordCmb.Invoke(decidedWord);
    }


    /// <summary>Verify if character is equal to the words first letter.
    /// Then decide player's action based on word chosen.</summary>
    private string DecideAction(string character)
    {
        if (character == attackWordText[0].ToString().ToLower())
        {
            Debug.Log("Chose to attack.");
            actionChosen = Actions.Attack;
            return attackWordText;
        }    
        else if (character == dodgeWordText[0].ToString().ToLower())
        {
            Debug.Log("Chose to dodge.");
            actionChosen = Actions.Dodge;
            return dodgeWordText;
        }

        //in case player doesn't type a letter equal to the
        //first letter of one of the words displayed
        Debug.Log("Didn't choose anything.");
        return string.Empty;
    }


    public void CompleteWordCmb()
    {
        switch (actionChosen)
        {
            case Actions.Attack:
                Attack();
                break;
            case Actions.Dodge:
                Dodge();
                break;
            default:
                break;
        }

        //clear everything so the player can choose the next action
        SendNextWordCmb.Invoke(string.Empty);
        Debug.Log("Choose the next action.");
    }


    private void Attack()
    {
        Debug.Log("Attacked");
        AttackEnemy.Invoke(stats.PlayerAttack);
    }


    private void Dodge()
    {
        Debug.Log("Dodged");
        ActivateDodge.Invoke();
    }
}
