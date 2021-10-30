﻿using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    //Variables
    [SerializeField]
    private int playerMaxHP, playerAttack;

    private int playerCurrentHP;

    private bool isPlayerDodging;

    private bool isPlayerDead;

    ///<summary>Number of mistakes player does while typing (Adventure and Combat states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat and Puzzle states only).</summary>
    private float timeElapsedSeconds;


    //Delegates
    delegate void UpdateInfoUIDelegate(int info);
    UpdateInfoUIDelegate UpdateTimeElapsedUI;
    UpdateInfoUIDelegate UpdateMistakesUI;

    delegate void PlayerHPBarFill(string id, float fillAmount);
    PlayerHPBarFill UpdatePlayerHPBarFill;


    //Unity events
    [SerializeField]
    private UnityEvent GameOver;


    public int PlayerAttack
    {
        get => playerAttack;
    }

    public bool IsPlayerDead
    {
        get => isPlayerDead;
    }


    private void Start()
    {
        RecoverFullHP();
    }


    public void SetGeneralUIDelegates()
    {
        GeneralUI UIUpdater = GameObject.FindGameObjectWithTag("GeneralUI").GetComponent<GeneralUI>();

        UpdateTimeElapsedUI += UIUpdater.SetTimeElapsedUI;
        UpdateMistakesUI += UIUpdater.SetMistakesUI;

        UpdatePlayerHPBarFill += GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateHealthBarFillUI;
    }


    //Invoked by UnityEvent in Typing script when player does a mistake
    public void AddMistake()
    {
        numMistakes++;

        UpdateMistakesUI(numMistakes);
    }


    //Invoked by UnityEvent in Typing script every Update
    public void AddTimeElapsed()
    {
        timeElapsedSeconds += Time.deltaTime;

        UpdateTimeElapsedUI((int)timeElapsedSeconds);
    }


    public void TakeDamage(int damage)
    {
        if (!isPlayerDead)
        {
            if (!isPlayerDodging)
            {
                playerCurrentHP -= damage;
                if (playerCurrentHP <= 0)
                {
                    PlayerDies();
                }
                else
                {
                    Debug.Log($"Player took {damage} damage | {playerCurrentHP} HP left | {playerMaxHP} Max HP.");
                    UpdateHPBar();
                }

            }
            else
            {
                Debug.Log("Dodged the enemy attack.");
                isPlayerDodging = false;
            }
        }
    }


    private void PlayerDies()
    {
        playerCurrentHP = 0;
        UpdateHPBar();
        isPlayerDead = true;
        GameOver.Invoke();
        Debug.Log("Player died.");
    }


    public void ActivateDodge()
    {
        isPlayerDodging = true;
    }


    public void RecoverFullHP()
    {
        playerCurrentHP = playerMaxHP;
        UpdateHPBar();
    }


    private void UpdateHPBar()
    {
        float fillAmount = (float)playerCurrentHP / (float)playerMaxHP;
        UpdatePlayerHPBarFill.Invoke("Player", fillAmount);
    }
}
