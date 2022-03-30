using System.Collections;
using UnityEngine;

//Change graphics and behaviour of camera 

public class GraphicsUIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup LoadingScreen;

    //Graphics and UI divided by sections
    [SerializeField]
    private GameObject Adventure;

    [SerializeField]
    private GameObject Combat;

    [SerializeField]
    private GameObject Puzzle;

    [SerializeField]
    private GameObject Challenge;

    [SerializeField]
    private CameraMovement mainCamera;

    public IEnumerator ActivateLoadingScreen()
    {
        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f, LoadingScreen));

        Adventure.SetActive(true); //everything gets activated in order to be able 
        Combat.SetActive(true); //to change sprites while the game is loading
        Puzzle.SetActive(true);
        Challenge.SetActive(true);
    }


    private IEnumerator DeactivateLoadingScreen()
    {
        yield return StartCoroutine(FadeLoadingScreen(0, 0.5f, LoadingScreen));
    }


    IEnumerator FadeLoadingScreen(float targetValue, float duration, CanvasGroup uiElement)
    {
        float startValue = uiElement.alpha;
        float time = 0f;

        while (time < duration)
        {            
            uiElement.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        //this is here to guarantee that the alpha is 1 (or 0) instead of a very close float value
        uiElement.alpha = targetValue;
    }


    ///<summary>Change level graphics depending on level type.</summary>
    public IEnumerator ChangeLevelGraphics(string levelType)
    {
        //if last level was adventure reset positions
        //so that when the player returns from a combat or puzzle
        //the layers are ready to move
        if (Adventure.activeSelf) 
        {
            mainCamera.GetComponent<BackgroundManager>().ResetPositions();
        }

        mainCamera.canMoveCamera = false;

        switch (levelType)
        {
            case "Adventure":
                Combat.SetActive(false);
                Puzzle.SetActive(false);
                Challenge.SetActive(false);
                mainCamera.canMoveCamera = true; //activate camera
                break;
            case "Combat":
                Adventure.SetActive(false);
                Puzzle.SetActive(false);
                Challenge.SetActive(false);
                break;
            case "Puzzle":
                Adventure.SetActive(false);
                Combat.SetActive(false);
                Challenge.SetActive(false);
                break;
            case "Challenge":
                Adventure.SetActive(false);
                Combat.SetActive(false);
                Puzzle.SetActive(false);
                break;
            default:
                break;
        }

        yield return StartCoroutine(DeactivateLoadingScreen());
    }
}
