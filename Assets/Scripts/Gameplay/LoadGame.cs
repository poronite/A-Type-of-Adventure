using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField]
    private LevelTemplate firstLevel;

    void Awake()
    {
        StartCoroutine(LoadGraphicsScene());
    }


    IEnumerator LoadGraphicsScene()
    {
        AsyncOperation loadedLevel = SceneManager.LoadSceneAsync("Graphics_Audio", LoadSceneMode.Additive);

        while (!loadedLevel.isDone)
        {
            yield return loadedLevel;
        }

        //After everything is loaded.
        SetupStartGame();
    }


    ///<summary>Setup essential things like delegates and start the game.</summary>
    private void SetupStartGame()
    {
        //setup delegates
        //player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().SetDelegatesPlayerStats();
        player.GetComponent<Typing>().SetDelegatesTyping();
        player.GetComponent<Adventure>().SetDelegatesAdv();
        player.GetComponent<Combat>().SetReferences();
        player.GetComponent<Combat>().SetDelegatesCmb();
        player.GetComponent<Puzzle>().SetDelegatesPzl();
        player.GetComponent<Challenge>().SetDelegatesChl();

        //enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.GetComponent<Enemy>().SetDelegatesEnemy();
        enemy.GetComponent<EnemyStats>().SetDelegatesEnemyStats();

        //level
        LevelController levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        levelController.SetDelegatesLevel();

        EncounterController encounterController = GameObject.FindGameObjectWithTag("EncountersController").GetComponent<EncounterController>();
        encounterController.SetDelegatesEncounters();


        //Start Game
        levelController.ChangeLevel(firstLevel);
    }
}
