using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that makes the camera follow the player movement

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    //distance between player and camera
    [SerializeField]
    private float offsetX;

    public bool canMoveCamera = false;

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (canMoveCamera)
        {
            gameObject.transform.position = 
                new Vector3(player.transform.position.x + offsetX,
                gameObject.transform.position.y, 
                gameObject.transform.position.z);
        }
    }
}
