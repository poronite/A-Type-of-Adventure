using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that will move the player graphics for 5s when the player clicks.
//the duration is refreshed to 5s even if there's still remaining seconds left

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerspeed;

    [SerializeField]
    private float movementDuration;

    private float remainingDuration;

    private bool isMoving = true;


    private void Start()
    {
        RefreshPlayerMovementDuration();
    }



    public void RefreshPlayerMovementDuration()
    {
        remainingDuration = movementDuration;

        isMoving = true;
    }


    private void StopMovement()
    {
        isMoving = false;
    }


    private void MovePlayer()
    {
        if (isMoving)
        {
            remainingDuration -= Time.deltaTime;

            Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            position.x += playerspeed * Time.deltaTime;

            gameObject.transform.position = position;

            if (remainingDuration <= 0)
            {
                isMoving = false;
            }
        }
    }


    private void Update()
    {
        MovePlayer();
    }
}
