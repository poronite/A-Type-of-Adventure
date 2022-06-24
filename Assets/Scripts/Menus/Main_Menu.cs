using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using ATOA;

public class Main_Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    
    [SerializeField]
    private GameObject OptionsMenu;

    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private RectTransform pointer;

    [SerializeField]
    private EventSystem input;


    //snapshot
    [SerializeField]
    private EventReference normalStateSnapshotReference;

    private EventInstance normalStateSnapshotInstance;



    private void Start()
    {
        Time.timeScale = 1.0f;

        normalStateSnapshotInstance = RuntimeManager.CreateInstance(normalStateSnapshotReference);

        normalStateSnapshotInstance.start();
    }

    public void Play()
    {
        MainMenu.SetActive(false);
        input.sendNavigationEvents = false;
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return StartCoroutine(ATOA_Utilities.FadeLoadingScreen(1f, 1f, fadeIn));

        //After fade in
        SceneManager.LoadSceneAsync(1);
    }


    public void Options()
    {
        Debug.Log("Open Options Menu");
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    private void OnDestroy()
    {
        normalStateSnapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        normalStateSnapshotInstance.release();
    }
}
