using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ATOA;

public class Main_Menu_Buttons : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup fadeIn;

    [SerializeField]
    private RectTransform pointer;

    [SerializeField]
    private EventSystem input;

    private GameObject lastSelectedButton;


    public void Play()
    {
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
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
            pointer.position = new Vector3(pointer.position.x, 
                lastSelectedButton.transform.position.y, 
                pointer.position.z);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        }
    }
}
