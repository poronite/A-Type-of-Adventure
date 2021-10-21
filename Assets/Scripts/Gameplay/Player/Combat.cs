using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will be used to manage everything involving Combat: when fighting against an enemy.
//

public class Combat : MonoBehaviour
{
    private string attackWordText = "Attack";
    private string dodgeWordText = "Dodge";


    //Delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordCmb;

    delegate void AttackDelegate(int damage);
    AttackDelegate AttackEnemy;


    public void SetDelegatesCmb()
    {
        SendNextWordCmb += gameObject.GetComponent<Typing>().NewWord;
    }


    public void SetWordCmb(string character)
    {
        string decidedWord = DecideWord(character);

        SendNextWordCmb.Invoke(decidedWord);
    }

    /// <summary>Verify if character is equal to the words first letter.</summary>
    private string DecideWord(string character)
    {
        if (character == attackWordText[0].ToString().ToLower())
            return attackWordText;
        else if (character == dodgeWordText[0].ToString().ToLower())
            return dodgeWordText;

        return "None";
    }

    public void CompleteWordCmb(string word)
    {
        //attack enemy
    }
}
