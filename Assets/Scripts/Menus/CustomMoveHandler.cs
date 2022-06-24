using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

//unica vez que uma interface é util
public class CustomMoveHandler : MonoBehaviour, IMoveHandler
{
    [SerializeField]
    StudioEventEmitter buttonSelectAudio;

    public void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Up:
            case MoveDirection.Down:
                buttonSelectAudio.Play();
                break;
            default:
                break;
        }
    }
}
