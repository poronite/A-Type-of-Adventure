using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ATOA;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private EventSystem input;

    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    private CutsceneManager cutsceneManager;

    delegate void PauseDelegate();
    PauseDelegate ResumePauseGame;

    delegate void OptionsDelegate();
    OptionsDelegate DisplayOptionsMenu;

    private void Start()
    {
        Typing typingController = GameObject.FindGameObjectWithTag("Player").GetComponent<Typing>();
        ResumePauseGame = typingController.PauseResumeGame;
        DisplayOptionsMenu = typingController.DisplayOptions;
    }

    private void OnEnable()
    {
        audioController.ResumePauseSounds(true);
        cutsceneManager.PauseVideo();
    }

    private void OnDisable()
    {
        audioController.ResumePauseSounds(false);
        cutsceneManager.ResumeVideo();
    }

    public void OnResumeButtonPressed()
    {
        ResumePauseGame.Invoke();
    }

    public void OnOptionsButtonPressed()
    {
        DisplayOptionsMenu.Invoke();
    }

    public void OnMainMenuButtonPressed()
    {
        input.sendNavigationEvents = false;
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        yield return StartCoroutine(ATOA_Utilities.FadeLoadingScreen(1f, 1f, fadeIn));

        //After fade in
        SceneManager.LoadSceneAsync(0);
    }
}
