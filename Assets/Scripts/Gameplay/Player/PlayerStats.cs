using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Variables
    [SerializeField]
    private int playerMaxHP, playerAttack;

    private int playerCurrentHP;

    ///<summary>Number of mistakes player does while typing (Adventure and Combat states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat and Puzzle states only).</summary>
    private float timeElapsedSeconds;

    delegate void UpdateInfoUIDelegate(int info);
    UpdateInfoUIDelegate UpdateTimeElapsedUI;
    UpdateInfoUIDelegate UpdateMistakesUI;


    private void Awake()
    {
        RecoverFullHP();
    }

    public void SetDelegates()
    {
        GeneralUI UIUpdater = GameObject.FindGameObjectWithTag("AdventureGfxUI").GetComponent<GeneralUI>();

        UpdateTimeElapsedUI += UIUpdater.SetTimeElapsedUI;
        UpdateMistakesUI += UIUpdater.SetMistakesUI;
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


    public void RecoverFullHP()
    {
        playerCurrentHP = playerMaxHP;
    }
}
