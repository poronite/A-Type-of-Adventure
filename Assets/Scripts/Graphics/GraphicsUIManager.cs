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
        LoadingScreen.SetActive(true); //for now it only does this
    }


    /// <summary>Deactivate loading screen, adventure, combat and puzzle graphics and UI.</summary>
    public void DeactivateAll()
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

        Adventure.SetActive(false);
        Combat.SetActive(false);
        LoadingScreen.SetActive(false);
    }


    /// <summary>Change to Adventure graphics and UI.</summary>
    public void ActivateAdventure()
    {
        DeactivateAll();
        mainCamera.canMoveCamera = true; //activate camera
        Adventure.SetActive(true);
    }


    /// <summary>Change to Combat graphics and UI.</summary>
    public void ActivateCombat()
    {
        DeactivateAll();
        Combat.SetActive(true);
    }
}
