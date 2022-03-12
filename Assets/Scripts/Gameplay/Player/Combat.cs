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

    private Actions actionChosen;

    private bool onBossPhase = false;

    private List<string> attackWordsList = new List<string> { "Punch", "Kick", "Tackle", "Slash"};
    private string attackWordText = string.Empty;
    private string previousAttackWord = string.Empty; //so that the same word doesn't appear twice in a row

    private List<string> dodgeWordsList = new List<string> { "Roll", "Dash", "Crouch", "Jump"};
    private string dodgeWordText = string.Empty;
    private string previousDodgeWord = string.Empty; //so that the same word doesn't appear twice in a row

    private LevelTemplate nextLevel; //next level after player wins the combat


    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordCmb;
    WordDelegate DisplayCurrentBossPhaseWordCmb;

    delegate void AttackDelegate(int damage);
    AttackDelegate AttackEnemy;

    //UI Delegates
    delegate void ClearDelegate();
    ClearDelegate ClearCurrentOutputWordCmb;
    ClearDelegate ClearAttackDodgeWordsCmb;

    delegate void OutputUIDelegate(string character);
    OutputUIDelegate AddCharacterCmb;
    OutputUIDelegate AddCharacterBossPhaseCmb;

    delegate void DisplayActionWordsDelegate(string attackWord, string dodgeWord);
    DisplayActionWordsDelegate DisplayNewAttackDodgeWordsCmb;

    delegate void ActionBasedDelegate(Actions action);
    ActionBasedDelegate DefineWordColorCmb;
    ActionBasedDelegate DisplayNewCurrentWordCmb;

    delegate void ChangeLevelDelegate(LevelTemplate level);
    ChangeLevelDelegate GoToNextLevel;


    //Unity Events
    [SerializeField]
    private UnityEvent ActivateDodge;

    [SerializeField]
    private UnityEvent ClearWord;

    [SerializeField]
    private UnityEvent WinFight;


    private void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }


    public void SetDelegatesCmb()
    {
        SendNextWordCmb = gameObject.GetComponent<Typing>().NewWord;
        AttackEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().TakeDamage;
        GoToNextLevel = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ChangeLevel;

        CombatUI CmbUIController = GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>();

        ClearCurrentOutputWordCmb = CmbUIController.ClearCurrentOutputWordUICmb;
        ClearAttackDodgeWordsCmb = CmbUIController.ClearAttackDodgeWordsUICmb;
        AddCharacterCmb = CmbUIController.AddCharacterUICmb;
        AddCharacterBossPhaseCmb = CmbUIController.AddCharacterBossPhaseUICmb;
        DisplayNewAttackDodgeWordsCmb = CmbUIController.DisplayNewAttackDodgeWordsUICmb;
        DefineWordColorCmb = CmbUIController.DefineWordColorUICmb;
        DisplayNewCurrentWordCmb = CmbUIController.DisplayNewCurrentWordUICmb;
        DisplayCurrentBossPhaseWordCmb = CmbUIController.DisplayCurrentBossPhaseWordUICmb;
    }


    public void StartCombat(EnemyTemplate enemy, LevelTemplate nextLevelAfterCombat) //Start of a new game (Combat)
    {
        nextLevel = nextLevelAfterCombat;
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Combat;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().SetupEnemy(enemy);
        SetWords();
        Debug.Log("Started combat.");
    }


    ///<summary>Set the next attack and dodge word for the player to choose.
    ///The same word can't appear twice.</summary>
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
        //Debug.Log($"Choose the next word: {attackWordText} or {dodgeWordText}");
    }


    ///<summary>Send to Typing script the word that the player chose.</summary>
    public bool SetChosenWordCmb(string character)
    {
        actionChosen = Actions.None;

        string decidedWord = DecideAction(character);

        if (!(actionChosen == Actions.None))
        {
            DefineWordColorCmb.Invoke(actionChosen);
            DisplayNewCurrentWordCmb.Invoke(actionChosen);

            ClearAttackDodgeWordsCmb.Invoke();
            SendNextWordCmb.Invoke(decidedWord);
            return true;
        }

        return false;
    }


    ///<summary>Verify if character is equal to the words first letter.
    ///Then decide player's action based on word chosen.</summary>
    private string DecideAction(string character)
    {
        if (character == attackWordText[0].ToString().ToLower())
        {
            //Debug.Log("Chose to attack.");
            actionChosen = Actions.Attack;
            return attackWordText;
        }    
        else if (character == dodgeWordText[0].ToString().ToLower())
        {
            //Debug.Log("Chose to dodge.");
            actionChosen = Actions.Dodge;
            return dodgeWordText;
        }

        //in case player doesn't type a letter equal to the
        //first letter of one of the words displayed
        Debug.Log("Didn't choose anything. Try Again.");
        return string.Empty;
    }


    public void CompleteWordCmb(string word)
    {
        //this is here because after defeating the enemy 
        //it was clearing the word that the player had to type
        //making it impossible to continue the game
        SetWords();

        if (!onBossPhase)
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
        }
        else
        {
            DeactivateBossPhase();
        }
        
    }

    public void AddCharacterUI(string character)
    {
        if (!onBossPhase)
        {
            AddCharacterCmb.Invoke(character);
        }
        else
        {
            AddCharacterBossPhaseCmb.Invoke(character);
        }
    }


    private void Attack()
    {
        //Debug.Log("Attacked");
        AttackEnemy.Invoke(stats.PlayerAttack);
    }


    private void Dodge()
    {
        //Debug.Log("Dodged");
        ActivateDodge.Invoke();
    }


    public void ActivateBossPhase(string word)
    {
        onBossPhase = true;
        SendNextWordCmb.Invoke(word);
        DisplayCurrentBossPhaseWordCmb.Invoke(word);
    }


    private void DeactivateBossPhase()
    {
        onBossPhase = false;
        SetWords();
    }


    public void WinCombat()
    {
        WinFight.Invoke();
        GoToNextLevel.Invoke(nextLevel);
    }
}
