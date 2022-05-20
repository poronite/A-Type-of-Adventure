using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    //Variables
    private string playerName;

    private int playerMaxHP = 4;

    private int playerCurrentHP;

    public bool IsPlayerDodging;

    private bool isPlayerDead;

    ///<summary>Number of mistakes player does while typing (Adventure and Combat states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat and Puzzle states only).</summary>
    private float timeElapsedSeconds;


    //Delegates
    delegate void NoHP();
    NoHP GameOver;

    delegate void SendNameToAdv(string name);
    SendNameToAdv ReplaceWithName;

    //delegate void UpdateInfoUIDelegate(int info);
    //UpdateInfoUIDelegate UpdateTimeElapsedUI;
    //UpdateInfoUIDelegate UpdateMistakesUI;

    delegate void PlayerHPBarFill(string id, float fillAmount);
    PlayerHPBarFill UpdatePlayerHPBarFill;

    delegate void SnapshotDelegate(SnapshotName snapshotName);
    SnapshotDelegate ChangeSnapshot;



    private void Start()
    {
        RecoverFullHP();
    }


    public void SetDelegatesPlayerStats()
    {
        GameOver = gameObject.GetComponent<Typing>().GameOver;

        ReplaceWithName = gameObject.GetComponent<Adventure>().SetPlayerName;

        //GeneralUI UIUpdater = GameObject.FindGameObjectWithTag("GeneralUI").GetComponent<GeneralUI>();
        //UpdateTimeElapsedUI = UIUpdater.SetTimeElapsedUI;
        //UpdateMistakesUI = UIUpdater.SetMistakesUI;

        UpdatePlayerHPBarFill = GameObject.FindGameObjectWithTag("CombatGfxUI").GetComponent<CombatUI>().UpdateHealthBarFillUI;

        ChangeSnapshot = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>().ChangeSnapshot;
    }


    //Invoked by UnityEvent in Typing script when player does a mistake
    public void AddMistake()
    {
        numMistakes++;

        //UpdateMistakesUI(numMistakes);
    }


    //Invoked by UnityEvent in Typing script every Update
    public void AddTimeElapsed()
    {
        timeElapsedSeconds += Time.deltaTime;

        //UpdateTimeElapsedUI((int)timeElapsedSeconds);
    }


    public void SetName(string name)
    {
        playerName = name;
        ReplaceWithName.Invoke(playerName);
    }


    public void TakeDamage()
    {
        if (!isPlayerDead)
        {
            playerCurrentHP -= 1;

            if (playerCurrentHP <= 0)
            {
                PlayerDies();
            }
            else
            {
                Debug.Log($"Player took 1 damage | {playerCurrentHP} HP left | {playerMaxHP} Max HP.");
                UpdateHPBar();

                if (playerCurrentHP == 1)
                {
                    ChangeSnapshot.Invoke(SnapshotName.LowHealth);
                }
                else
                {
                    ChangeSnapshot.Invoke(SnapshotName.Normal);
                }
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
        IsPlayerDodging = true;
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
