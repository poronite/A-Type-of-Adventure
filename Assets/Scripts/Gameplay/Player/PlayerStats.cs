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

    private bool isPlayerDodging = false;

    ///<summary>Number of mistakes player does while typing (Adventure, Combat and Challenge states only).</summary>
    private int numMistakes;

    ///<summary>Time elapsed since start of the game. (Adventure, Combat, Puzzle and Challenge states only).</summary>
    private float timeElapsedSeconds;

    //float to make sure it divides properly with time
    private float numWordsTyped;

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

    delegate void SnapshotDelegate(SoundState snapshotName, bool stopAllSounds);
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

    //Invoked every word completed
    //the string is just there otherwise couldn't activate this on already existing delegate
    public void AddTypedWordNumber(string filler)
    {
        numWordsTyped++;
    }


    public void SetName(string name)
    {
        playerName = name;
        ReplaceWithName.Invoke(playerName);
    }


    public void TakeDamage()
    {
        PlayerCurrentHP -= 1;
        
        Debug.Log($"Player took 1 damage | {playerCurrentHP} HP left | {playerMaxHP} Max HP.");
        
        if (playerCurrentHP == 0)
        {
            PlayerDies();
        }
    }


    private void PlayerDies()
    {
        GameOver.Invoke();
        Debug.Log("Player died.");
    }

    public void RecoverFullHP()
    {
        PlayerCurrentHP = playerMaxHP;
    }


    public void UpdateGraphicsBasedOnHP()
    {
        UpdateHPUI();

        switch (playerCurrentHP)
        {
            case 4:
            case 3:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, false, 0f));
                UpdateSoundFilter(SoundState.Normal);
                break;
            case 2:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, true, 0.15f));
                UpdateSoundFilter(SoundState.Normal);
                break;
            case 1:
                StartCoroutine(ATOA_Utilities.VignetteLerp(globalVolume, 2f, true, 0.3f));
                UpdateSoundFilter(SoundState.LowHealth);
                break;
            case 0:
                break;
            default:
                break;
        }
    }

    public void UpdateSoundFilter(SoundState name)
    {
        ChangeSnapshot.Invoke(name, false);
    }

    public void UpdateHPUI()
    {
        float fillAmount = (float)playerCurrentHP / (float)playerMaxHP;
        UpdatePlayerHPBarFill.Invoke("Player", fillAmount);
    }

    public void SetScoreRank()
    {
        //wpm - mistakes
        float wpm = numWordsTyped / (timeElapsedSeconds / 60);

        int minutes = (int)timeElapsedSeconds / 60;
        int seconds = (int)timeElapsedSeconds % 60;

        string timePlayed = string.Empty;

        if (minutes < 10)
        {
            timePlayed += $"0{minutes} : ";
        }
        else
        {
            timePlayed += $"{minutes} : ";
        }

        if (seconds < 10)
        {
            timePlayed += $"0{seconds}";
        }
        else
        {
            timePlayed += $"{seconds}";
        }

        int score = (int)wpm - numMistakes / 60;

        if (score <= 0)
        {
            score = 1;
        }

        string rank = "E";

        if (score > 75)
        {
            rank = "S";
        }
        else if (score > 60)
        {
            rank = "A";
        }
        else if (score > 40)
        {
            rank = "B";
        }
        else if (score > 25)
        {
            rank = "C";
        }
        else if (score > 15)
        {
            rank = "D";
        }
        else if (score > 1)
        {
            rank = "E";
        }

        EndGameScreen endGame = GameObject.FindGameObjectWithTag("EndGame").GetComponent<EndGameScreen>();

        StartCoroutine(endGame.DisplayEndGameScreen(score, timePlayed, numMistakes, rank));
    }
}
