using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change graphics and behaviour of camera 

public class GraphicsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject LoadingScreen;

    //Graphics and UI divided by sections
    [SerializeField]
    private GameObject Adventure;

    [SerializeField]
    private GameObject Combat;

    [SerializeField]
    private CameraMovement mainCamera;

    public void ActivateLoadingScreen()
    {
        Adventure.SetActive(true); //everything gets activated in order to be able 
        Combat.SetActive(true); //to change sprites while the game is loading

        //for now it only does this
        LoadingScreen.SetActive(true); 
    }


    ///<summary>Change level graphics depending on level type.</summary>
    public void ChangeLevelGraphics(string levelType)
    {
        ActivateLoadingScreen();

        //if last level was adventure reset positions
        //so that when the player returns from a combat or puzzle
        //the layers are ready to move
        if (Adventure.activeSelf) 
        {
            mainCamera.GetComponent<ParalaxEffectAdv>().ResetPositions();
        }

        mainCamera.canMoveCamera = false;

        switch (levelType)
        {
            case "Adventure":
                Combat.SetActive(false);
                mainCamera.canMoveCamera = true; //activate camera
                break;
            case "Combat":
                Adventure.SetActive(false);
                break;
            case "Puzzle":
                break;
            default:
                break;
        }

        LoadingScreen.SetActive(false);
    }
}
