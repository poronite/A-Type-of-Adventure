using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    public void TriggerEvent(EventsTemplate eventToBeTriggered)
    {
        switch (eventToBeTriggered.EventType)
        {
            case EventType.Animation:
                TriggerAnimation(eventToBeTriggered);
                break;
            default:
                break;
        } 
    }


    private void TriggerAnimation(EventsTemplate triggeredEvent)
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;

        GameObject instance = Instantiate(triggeredEvent.EventTarget,
            new Vector3(cameraPosition.x + triggeredEvent.SpawnOffset.x, cameraPosition.y + triggeredEvent.SpawnOffset.y),
            Quaternion.identity, GameObject.FindGameObjectWithTag("GraphicsEvents").transform);

        instance.GetComponentInChildren<Animator>().Play(triggeredEvent.AnimationToPlay, 0);
    }
}
