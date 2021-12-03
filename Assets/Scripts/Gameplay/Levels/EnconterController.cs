using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnconterController : MonoBehaviour
{
    public void EnconterTriggered(EncontersTemplate eventToBeTriggered)
    {
        switch (eventToBeTriggered.EnconterType)
        {
            case EnconterType.Animation:
                TriggerAnimation(eventToBeTriggered);
                break;
            default:
                break;
        } 
    }


    private void TriggerAnimation(EncontersTemplate triggeredEnconter)
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        GameObject instance = Instantiate(triggeredEnconter.EnconterTarget,
            new Vector3(cameraPosition.x + triggeredEnconter.SpawnOffset.x, cameraPosition.y + triggeredEnconter.SpawnOffset.y),
            Quaternion.identity, GameObject.FindGameObjectWithTag("Enconters").transform);

        instance.GetComponentInChildren<Animator>().Play(triggeredEnconter.AnimationToPlay, 0);
    }
}
