using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that makes the camera follow the player movement

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public bool canMoveCamera = false;

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (canMoveCamera)
        {
            gameObject.transform.position = new Vector2(player.transform.position.x, gameObject.transform.position.y);
        }
    }
}
