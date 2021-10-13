using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int playerMaxHP, playerAttack;

    private int playerCurrentHP;

    ///<summary>Number of mistakes player does while typing (Adventure and Combat states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat and Puzzle states only).</summary>
    private float timeElapsed;

    private void Start()
    {
        RecoverFullHP();
    }

    //Invoked by UnityEvent in Typing script when player does a mistake
    public void AddMistake()
    {
        numMistakes++;
        Debug.Log($"Number of Mistakes: {numMistakes}");
    }

    //Invoked by UnityEvent in Typing script every Update
    public void AddTimeElapsed()
    {
        timeElapsed += Time.deltaTime;
        //Debug.Log($"Time elapsed: {(int)timeElapsed}s");
    }

    public void RecoverFullHP()
    {
        playerCurrentHP = playerMaxHP;
    }
}
