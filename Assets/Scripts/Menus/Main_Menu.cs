using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ATOA;

public class Main_Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject OptionsMenu;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private RectTransform pointer;

    [SerializeField]
    private EventSystem input;

    private GameObject lastSelectedUIMainMenu;


    private void Start()
    {
        lastSelectedUIMainMenu = playButton.gameObject;
    }

    public void Play()
    {
        input.sendNavigationEvents = false;
        MainMenu.SetActive(false);
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
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    void Update()
    {
        if (MainMenu.activeInHierarchy == true)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                lastSelectedUIMainMenu = EventSystem.current.currentSelectedGameObject;
                pointer.position = new Vector3(pointer.position.x,
                    lastSelectedUIMainMenu.transform.position.y,
                    pointer.position.z);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedUIMainMenu);
            }
        }
    }
}
