using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using ATOA;

public class PlayerStats : MonoBehaviour
{
    //Variables
    private string playerName;

    private int playerMaxHP = 4;

    private int playerCurrentHP = 4;

    public bool isPlayerDodging = false;

    private bool isPlayerDead = false;

    ///<summary>Number of mistakes player does while typing (Adventure and Combat states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat and Puzzle states only).</summary>
    private float timeElapsedSeconds;

    private PostProcessVolume globalVolume;


    public int PlayerCurrentHP
    {
        get
        {
            return playerCurrentHP;
        }
        set
        {
            playerCurrentHP = value;

            UpdateGraphicsBasedOnHP();
        }
    }

    public bool IsPlayerDodging
    {
        get
        {
            return isPlayerDodging;
        }

        set
        {
            isPlayerDodging = value;

            //update UI
        }
    }


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

        globalVolume = FindObjectOfType<PostProcessVolume>();

        //Debug.Log(globalVolume.name);
    }


    //When player does a mistake
    public void AddMistake()
    {
        numMistakes++;

        //UpdateMistakesUI(numMistakes);
    }


    //Invoked Typing script every Update
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
            PlayerCurrentHP -= 1;

            Debug.Log($"Player took 1 damage | {playerCurrentHP} HP left | {playerMaxHP} Max HP.");

            if (playerCurrentHP == 0)
            {
                PlayerDies();
            }
        }
    }


    private void PlayerDies()
    {
        isPlayerDead = true;
        GameOver.Invoke();
        Debug.Log("Player died.");
    }

    public void RecoverFullHP()
    {
        playerCurrentHP = playerMaxHP;
    }


    public void UpdateGraphicsBasedOnHP()
    {
        UpdateHPUI();

        switch (playerCurrentHP)
        {
            case 4:
            case 3:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, false, 0f));
                UpdateSoundFilter(SnapshotName.Normal);
                break;
            case 2:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, true, 0.15f));
                UpdateSoundFilter(SnapshotName.Normal);
                break;
            case 1:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, true, 0.3f));
                UpdateSoundFilter(SnapshotName.LowHealth);
                break;
            case 0:
                break;
            default:
                break;
        }
    }

    public void UpdateSoundFilter(SnapshotName name)
    {
        ChangeSnapshot.Invoke(name);
    }

    public void UpdateHPUI()
    {
        float fillAmount = (float)playerCurrentHP / (float)playerMaxHP;
        UpdatePlayerHPBarFill.Invoke("Player", fillAmount);
    }
}
