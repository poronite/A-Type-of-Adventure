using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterController : MonoBehaviour
{
    //delegates
    delegate void EncounterDelegates(bool moveState);
    EncounterDelegates SetMovement;


    public void SetDelegatesEnconters()
    {
        SetMovement += GameObject.FindGameObjectWithTag("PlayerGfx").GetComponent<PlayerMovementAdv>().SetPlayerMovement;
    }


    public void EnconterTriggered(EncountersTemplate encounterToBeTriggered)
    {
        switch (encounterToBeTriggered.EncounterType)
        {
            case EncounterType.Appear:
                AppearEncounter(encounterToBeTriggered);
                break;
            case EncounterType.Assault:
                AssaultEncounter(encounterToBeTriggered);
                break;
            default:
                break;
        } 
    }


    private void AppearEncounter(EncountersTemplate encounterToBeTriggered)
    {
        SetPlayerMovement(encounterToBeTriggered.StopPlayer);
        SpawnGfxObject(encounterToBeTriggered);
    }


    private void AssaultEncounter(EncountersTemplate encounterToBeTriggered)
    {
        SetPlayerMovement(encounterToBeTriggered.StopPlayer);
        GameObject instance = SpawnGfxObject(encounterToBeTriggered);
        TriggerAnimation(encounterToBeTriggered, instance);
    }


    private GameObject SpawnGfxObject(EncountersTemplate triggeredEncounter)
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        return Instantiate(triggeredEncounter.EnconterTarget,
            new Vector3(cameraPosition.x + triggeredEncounter.SpawnOffset.x, cameraPosition.y + triggeredEncounter.SpawnOffset.y),
            Quaternion.identity, GameObject.FindGameObjectWithTag("Encounters").transform);
    }


    //set if player can move or not
    public void SetPlayerMovement(bool movementState)
    {
        SetMovement.Invoke(movementState);
    }


    private void TriggerAnimation(EncountersTemplate triggeredEncounter, GameObject gfxObject)
    {
        gfxObject.GetComponentInChildren<Animator>().Play(triggeredEncounter.AnimationToPlay, 0);
    }
}
