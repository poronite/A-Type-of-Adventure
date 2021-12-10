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
            case EncounterType.Gameplay:
                break;
            case EncounterType.Graphics:
                GameObject instance = SpawnGfxObject(encounterToBeTriggered);

                //graphics encounter can be just a simple sprite or it can also have an animation
                if (encounterToBeTriggered.AnimationToPlay != string.Empty)
                {
                    TriggerAnimation(encounterToBeTriggered.AnimationToPlay, instance);
                }

                break;
            default:
                break;
        }

        StartCoroutine(DelayBeforeStoppingMovement(encounterToBeTriggered.StopPlayer, encounterToBeTriggered.delayBeforePlayerStopping));

    }


    IEnumerator DelayBeforeStoppingMovement(bool stopPlayer, float duration)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetPlayerMovement(stopPlayer);
    }


    //set if player stops moving or not
    public void SetPlayerMovement(bool movementState)
    {
        SetMovement.Invoke(movementState);
    }


    private GameObject SpawnGfxObject(EncountersTemplate triggeredEncounter)
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        return Instantiate(triggeredEncounter.EncounterGfxObject,
            new Vector3(cameraPosition.x + triggeredEncounter.SpawnOffset.x, cameraPosition.y + triggeredEncounter.SpawnOffset.y),
            Quaternion.identity, GameObject.FindGameObjectWithTag("Encounters").transform);
    }


    private void TriggerAnimation(string animationName, GameObject gfxObject)
    {
        gfxObject.GetComponentInChildren<Animator>().Play(animationName, 0);
    }
}
