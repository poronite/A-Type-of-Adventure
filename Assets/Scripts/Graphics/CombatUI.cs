﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    //Variables
    [SerializeField]
    private Color attackWordColor, dodgeWordColor;

    [SerializeField]
    private Color attackWordIncompleteColor, dodgeWordIncompleteColor;


    //References
    [SerializeField]
    private Text attackWordTextUICmb;

    [SerializeField]
    private Text dodgeWordTextUICmb;

    [SerializeField]
    private Text currentTextUICmb;

    [SerializeField]
    private Text outputTextUICmb;

    [SerializeField]
    private Image enemyAttackWordFill;



    //Player
    ///<summary>Display output with new character added.</summary>
    public void DisplayNewOutputWordUICmb(string character)
    {
        outputTextUICmb.text += character;
    }


    ///<summary>Clear current and output words after player has finished typing the word chosen.</summary>
    public void ClearCurrentOutputWordUICmb()
    {
        currentTextUICmb.text = string.Empty;
        outputTextUICmb.text = string.Empty;
    }


    ///<summary>Display on screen the words that the player can choose from.</summary>
    public void DisplayNewAttackDodgeWordsUICmb(string attackWord, string dodgeWord)
    {
        attackWordTextUICmb.text = attackWord;
        dodgeWordTextUICmb.text = dodgeWord;
    }


    ///<summary>Clear attack and dodge words after player made a decision.</summary>
    public void ClearAttackDodgeWordsUICmb()
    {
        attackWordTextUICmb.text = string.Empty;
        dodgeWordTextUICmb.text = string.Empty;
    }


    ///<summary>Change current and output words color based on action chosen by player.</summary>
    public void DefineWordColorUICmb(Actions action)
    {
        switch (action)
        {
            case Actions.Attack:
                currentTextUICmb.color = attackWordIncompleteColor;
                outputTextUICmb.color = attackWordColor;
                break;
            case Actions.Dodge:
                currentTextUICmb.color = dodgeWordIncompleteColor;
                outputTextUICmb.color = dodgeWordColor;
                break;
            default:
                break;
        }
    }


    ///<summary> Display new current word based on action chosen by player.</summary>
    public void DisplayNewCurrentWordUICmb(Actions action)
    {
        switch (action)
        {
            case Actions.Attack:
                currentTextUICmb.text = attackWordTextUICmb.text;
                break;
            case Actions.Dodge:
                currentTextUICmb.text = dodgeWordTextUICmb.text;
                break;
            default:
                break;
        }
    }


    //Enemy
    /// <summary>Increase enemy's attack word fill based on time elapsed since last attack.</summary>
    public void UpdateEnemyAttackWordFillUI(float time, float attackSpeed)
    {
        enemyAttackWordFill.fillAmount = time / attackSpeed;
    }
}
