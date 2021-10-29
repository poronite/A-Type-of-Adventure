using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGraphicsScene : MonoBehaviour
{
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

        //enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.GetComponent<Enemy>().SetDelegatesEnemy();
        enemy.GetComponent<EnemyStats>().SetDelegatesEnemyStats();


        //Start Game
        player.GetComponent<Adventure>().StartAdventure();
    }
}
