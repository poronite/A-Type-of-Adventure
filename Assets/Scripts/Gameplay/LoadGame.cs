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
        AsyncOperation loadedLevel = SceneManager.LoadSceneAsync("Graphics", LoadSceneMode.Additive);

        while (!loadedLevel.isDone)
        {
            yield return loadedLevel;
        }

        //After everything is loaded.
        SetupStartGame();
    }


    /// <summary>Setup essential things like delegates and start the game.</summary>
    private void SetupStartGame()
    {
        //setup delegates
        //player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().SetGeneralUIDelegates();
        player.GetComponent<Adventure>().SetDelegatesAdv();
        player.GetComponent<Combat>().SetDelegatesCmb();
        player.GetComponent<Puzzle>().SetDelegatesPzl();

        //enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.GetComponent<Enemy>().SetDelegatesEnemy();
        enemy.GetComponent<EnemyStats>().SetDelegatesEnemyStats();

        //level
        GameObject levelController = GameObject.FindGameObjectWithTag("LevelController");
        levelController.GetComponent<LevelController>().SetDelegatesLevel();


        //Start Game
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ChangeLevel(firstLevel);
    }
}
