using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject LoadingScreen;

    //Graphics and UI divided by sections
    [SerializeField]
    private GameObject Adventure;

    [SerializeField]
    private GameObject Combat;



    public void ActivateLoadingScreen()
    {
        LoadingScreen.SetActive(true);
    }


    /// <summary>Deactivate loading screen, adventure, combat and puzzle graphics and UI.</summary>
    public void DeactivateAll()
    {
        LoadingScreen.SetActive(false);
        Adventure.SetActive(false);
        Combat.SetActive(false);
    }


    /// <summary>Change to Adventure graphics and UI.</summary>
    public void ActivateAdventure()
    {
        DeactivateAll();
        Adventure.SetActive(true);
    }


    /// <summary>Change to Combat graphics and UI.</summary>
    public void ActivateCombat()
    {
        DeactivateAll();
        Combat.SetActive(true);
    }
}
