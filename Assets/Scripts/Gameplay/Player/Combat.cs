using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script will be used to manage everything involving Combat: when fighting against an enemy.

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

    private List<string> attackWordsList = new List<string> { "Punch", "Kick", "Tackle", "Slash"};
    private string attackWordText = string.Empty;
    private string previousAttackWord = string.Empty; //so that the same word doesn't appear twice in a row

    private List<string> dodgeWordsList = new List<string> { "Roll", "Dash", "Crouch", "Jump"};
    private string dodgeWordText = string.Empty;
    private string previousDodgeWord = string.Empty; //so that the same word doesn't appear twice in a row


    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordCmb;

    delegate void AttackDelegate(int damage);
    AttackDelegate AttackEnemy;

    //UI Delegates
    delegate void ClearDelegate();
    ClearDelegate ClearCurrentOutputWordCmb;
    ClearDelegate ClearAttackDodgeWordsCmb;

    delegate void DisplayOutputWordDelegate(string character);
    DisplayOutputWordDelegate DisplayNewOutputWordCmb;

    delegate void DisplayActionWordsDelegate(string attackWord, string dodgeWord);
    DisplayActionWordsDelegate DisplayNewAttackDodgeWordsCmb;

    delegate void ActionBasedDelegate(Actions action);
    ActionBasedDelegate DefineWordColorCmb;
    ActionBasedDelegate DisplayNewCurrentWordCmb;


    //Unity Events
    [SerializeField]
    private UnityEvent ActivateDodge;

    [SerializeField]
    private UnityEvent ClearWord;


    private void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }


    public void SetDelegatesCmb()
    {
        SendNextWordCmb += gameObject.GetComponent<Typing>().NewWord;
        AttackEnemy += GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().TakeDamage;

        CombatUI CmbUIController = GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>();

        ClearCurrentOutputWordCmb += CmbUIController.ClearCurrentOutputWordUICmb;
        ClearAttackDodgeWordsCmb += CmbUIController.ClearAttackDodgeWordsUICmb;
        DisplayNewOutputWordCmb += CmbUIController.DisplayNewOutputWordUICmb;
        DisplayNewAttackDodgeWordsCmb += CmbUIController.DisplayNewAttackDodgeWordsUICmb;
        DefineWordColorCmb += CmbUIController.DefineWordColorUICmb;
        DisplayNewCurrentWordCmb += CmbUIController.DisplayNewCurrentWordUICmb;
    }


    public void StartCombat()
    {
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Combat;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().SetupEnemy(enemy);
        SetWords();
        Debug.Log("Started combat.");
    }


    /// <summary>Set the next attack and dodge word for the player to choose.
    /// The same word can't appear twice.</summary>
    private void SetWords()
    {
        //clear everything so the player can choose the next action
        ClearWord.Invoke();
        ClearCurrentOutputWordCmb.Invoke();


        int index;

        //attack
        //choose the new attack word
        index = Random.Range(0, attackWordsList.Count - 1);
        attackWordText = attackWordsList[index];

        //add previous word back to the list
        if (previousAttackWord != string.Empty)
            attackWordsList.Add(previousAttackWord);

        //remove the new attack word from the list 
        previousAttackWord = attackWordText;
        attackWordsList.Remove(previousAttackWord);


        //dodge
        //choose the new dodge word
        index = Random.Range(0, dodgeWordsList.Count - 1);
        dodgeWordText = dodgeWordsList[index];

        //add previous word back to the list
        if (previousDodgeWord != string.Empty)
            dodgeWordsList.Add(previousDodgeWord);

        //remove the new dodge word from the list 
        previousDodgeWord = dodgeWordText;
        dodgeWordsList.Remove(previousDodgeWord);


        DisplayNewAttackDodgeWordsCmb.Invoke(attackWordText, dodgeWordText);
        Debug.Log($"Choose the next word: {attackWordText} or {dodgeWordText}");
    }


    /// <summary>Send to Typing script the word that the player chose.</summary>
    public void SetChosenWordCmb(string character)
    {
        actionChosen = Actions.None;

        string decidedWord = DecideAction(character);

        ClearAttackDodgeWordsCmb.Invoke();

        DefineWordColorCmb.Invoke(actionChosen);
        DisplayNewCurrentWordCmb.Invoke(actionChosen);

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
        Debug.Log("Didn't choose anything. Try Again.");
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


        SetWords();
    }

    public void AddCharacterUICmb(string character)
    {
        DisplayNewOutputWordCmb.Invoke(character);
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
