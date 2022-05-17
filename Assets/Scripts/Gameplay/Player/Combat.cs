using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATOA;

//This script will be used to manage everything involving Combat: when fighting against an enemy.

public enum Actions
{
    None,
    Attack,
    Dodge
}

public enum Phase
{
    Normal,
    BossPhase,
    KillSparePhase
}

public class Combat : MonoBehaviour
{
    private PlayerStats stats;

    private Actions actionChosen;

    private Phase currentPhase = Phase.Normal;

    private List<string> attackWordsList = new List<string> { "Punch", "Kick", "Tackle", "Slash"};
    private string attackWordText = string.Empty;

    private List<string> dodgeWordsList = new List<string> { "Roll", "Dash", "Crouch", "Jump"};
    private string dodgeWordText = string.Empty;

    //normal enemy
    private LevelTemplate nextLevel; //next level after player wins the combat

    //boss
    private LevelTemplate nextLevelAfterKillingBoss;
    private LevelTemplate nextLevelAfterSparingBoss;


    //Delegates
    delegate void ClearDelegate();
    ClearDelegate ClearWord;

    delegate void WordDelegate(string word);
    WordDelegate SendNextWordCmb;
    WordDelegate DisplayCurrentBossPhaseWordCmb;

    delegate void AttackDelegate(int damage);
    AttackDelegate AttackEnemy;

    delegate void DodgeDelegate();
    DodgeDelegate ActivateDodge;

    delegate void CombatEndDelegate();
    CombatEndDelegate WinFight;


    //UI Delegates
    delegate void ClearUIDelegate();
    ClearUIDelegate ClearCurrentOutputWordCmb;
    ClearUIDelegate ClearAttackDodgeWordsCmb;

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

    private Animator playerAnimator;



    private void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }


    public void SetDelegatesCmb()
    {
        AttackEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().TakeDamage;
        ActivateDodge = gameObject.GetComponent<PlayerStats>().ActivateDodge;
        GoToNextLevel = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ChangeLevel;

        Typing typingController = gameObject.GetComponent<Typing>();
        SendNextWordCmb = typingController.NewWord;
        ClearWord = typingController.ClearWords;
        WinFight = typingController.Victory;

        CombatUI CmbUIController = GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>();
        ClearCurrentOutputWordCmb = CmbUIController.ClearCurrentOutputWordUICmb;
        ClearAttackDodgeWordsCmb = CmbUIController.ClearAttackDodgeWordsUICmb;
        AddCharacterCmb = CmbUIController.AddCharacterUICmb;
        AddCharacterBossPhaseCmb = CmbUIController.AddCharacterBossPhaseUICmb;
        DisplayNewAttackDodgeWordsCmb = CmbUIController.DisplayNewAttackDodgeWordsUICmb;
        DefineWordColorCmb = CmbUIController.DefineWordColorUICmb;
        DisplayNewCurrentWordCmb = CmbUIController.DisplayNewCurrentWordUICmb;
        DisplayCurrentBossPhaseWordCmb = CmbUIController.DisplayCurrentBossPhaseWordUICmb;

        playerAnimator = GameObject.Find("PlayerAnimation").GetComponent<Animator>();
    }


    public void StartCombat(LevelTemplate currentLevel) //Start of a new game (Combat)
    {
        stats.RecoverFullHP();
        EnemyTemplate enemy = currentLevel.Enemy;

        if (!enemy.IsBoss) //normal enemy
        {
            nextLevel = currentLevel.NextLevelAfterCombat;
        }
        else //boss
        {
            nextLevelAfterKillingBoss = currentLevel.NextLevelAfterKillingBoss;
            nextLevelAfterSparingBoss = currentLevel.NextLevelAfterSparingBoss;
        }

        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Combat;
        currentPhase = Phase.Normal;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStats>().SetupEnemy(enemy);
        GenerateActionWordsCmb();
        Debug.Log("Started combat.");
    }


    ///<summary>Generate the next attack and dodge word for the player to choose.
    ///The same word can't appear twice.</summary>
    private void GenerateActionWordsCmb()
    {
        string attackWord;
        string dodgeWord;

        //attack
        attackWord = ATOA_Utilities.GenerateWord(attackWordsList, attackWordText);

        //dodge
        dodgeWord = ATOA_Utilities.GenerateWord(dodgeWordsList, dodgeWordText);


        SetWords(attackWord, dodgeWord);
    }


    //split the SetWords function into 2
    //so that I can use this function to manually set kill/spare words
    private void SetWords(string attackWord, string dodgeWord)
    {
        //clear everything so the player can choose the next action
        ClearWord.Invoke();
        ClearCurrentOutputWordCmb.Invoke();

        attackWordText = attackWord;
        dodgeWordText = dodgeWord;

        DisplayNewAttackDodgeWordsCmb.Invoke(attackWord, dodgeWord);
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
        GenerateActionWordsCmb();

        switch (currentPhase)
        {
            case Phase.Normal:
                switch (actionChosen)
                {
                    case Actions.Attack:
                        //Attack(); //trigger animation
                        playerAnimator.SetTrigger("Attack");
                        break;
                    case Actions.Dodge:
                        Dodge(); //trigger animation
                        break;
                    default:
                        break;
                }
                break;
            case Phase.BossPhase:
                DeactivateBossPhase();
                break;
            case Phase.KillSparePhase:
                WinCombat();
                break;
            default:
                break;
        }        
    }

    public void AddCharacterUI(string character)
    {
        switch (currentPhase)
        {
            case Phase.Normal:
            case Phase.KillSparePhase:
                AddCharacterCmb.Invoke(character);
                break;
            case Phase.BossPhase:
                AddCharacterBossPhaseCmb.Invoke(character);
                break;
            default:
                break;
        }
    }


    public void Attack()
    {
        //Debug.Log("Attacked");
        AttackEnemy.Invoke(1);
    }


    private void Dodge()
    {
        //Debug.Log("Dodged");
        ActivateDodge.Invoke();
    }


    public void ActivateBossPhase(string word)
    {
        currentPhase = Phase.BossPhase;
        SendNextWordCmb.Invoke(word);
        DisplayCurrentBossPhaseWordCmb.Invoke(word);
    }


    private void DeactivateBossPhase()
    {
        currentPhase = Phase.Normal;
        GenerateActionWordsCmb();
    }


    public void ActivateKillSpareChoice()
    {
        currentPhase = Phase.KillSparePhase;
        SetWords("Kill", "Spare");
    }


    public void WinCombat()
    {
        WinFight.Invoke();

        switch (currentPhase)
        {
            case Phase.Normal: //win against normal enemy

                GoToNextLevel.Invoke(nextLevel);

                break;
            case Phase.KillSparePhase: //after choosing to kill/spare boss

                switch (actionChosen)
                {
                    case Actions.Attack: //kill
                        GoToNextLevel.Invoke(nextLevelAfterKillingBoss);
                        break;
                    case Actions.Dodge: //spare
                        GoToNextLevel.Invoke(nextLevelAfterSparingBoss);
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }
    }
}
