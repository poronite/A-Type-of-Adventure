using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using ATOA;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private EventSystem input;

    public void RetryCheckpoint()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().LoadFromCheckpoint();
    }

    public void ReturnMainMenu()
    {
        input.sendNavigationEvents = false;
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return StartCoroutine(ATOA_Utilities.FadeLoadingScreen(1f, 1f, fadeIn));

        //After fade in
        SceneManager.LoadSceneAsync(0);
    }
}
