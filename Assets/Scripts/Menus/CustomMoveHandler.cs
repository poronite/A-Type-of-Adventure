using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

enum Direction
{
    Vertical,
    Horizontal
}

//unica vez que uma interface é util
public class CustomMoveHandler : MonoBehaviour, IMoveHandler
{
    [SerializeField]
    private Direction movementDirection = Direction.Vertical;

    [SerializeField]
    StudioEventEmitter buttonSelectAudio;

    public void OnMove(AxisEventData eventData)
    {
        MoveDirection direction = eventData.moveDir;

        switch (movementDirection)
        {
            case Direction.Vertical:
                if (direction == MoveDirection.Up || direction == MoveDirection.Down)
                    buttonSelectAudio.Play();
                break;
            case Direction.Horizontal:
                if (direction == MoveDirection.Left || direction == MoveDirection.Right)
                    buttonSelectAudio.Play();
                break;
            default:
                break;
        }
    }
}
