using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using ATOA;

public class EndGameScreen : MonoBehaviour
{
    WaitForSeconds delay = new WaitForSeconds(1.0f);

    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI timePlayedText;

    [SerializeField]
    private TextMeshProUGUI mistakesText;

    [SerializeField]
    private TextMeshProUGUI rankText;

    [SerializeField]
    private GameObject returnMainMenuText;

    [SerializeField]
    private StudioEventEmitter popUpSound;

    [SerializeField]
    private StudioEventEmitter congratsSound;


    public IEnumerator DisplayEndGameScreen(int score, string timePlayed, int mistakes, string rank)
    {
        yield return delay;

        scoreText.text = $"{score}";

        popUpSound.Play();

        yield return delay;

        timePlayedText.text = timePlayed;

        popUpSound.Play();

        yield return delay;

        mistakesText.text = $"{mistakes}";

        popUpSound.Play();

        yield return delay;

        rankText.text = rank;

        congratsSound.Play();

        returnMainMenuText.SetActive(true);

        GameObject.FindGameObjectWithTag("Player").GetComponent<Typing>().CurrentPlayerState = PlayerState.EndGame;
    }

    public IEnumerator LoadMainMenu()
    {
        yield return StartCoroutine(ATOA_Utilities.FadeLoadingScreen(1f, 1f, fadeIn));

        //After fade in
        SceneManager.LoadSceneAsync(0);
    }
}
