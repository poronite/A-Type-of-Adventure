using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that will move the player graphics for 5s when the player clicks.
//the duration is refreshed to 5s even if there's still remaining seconds left

enum MovementState
{
    Idle,
    Move,
    Stopped
}

public class PlayerMovementAdv : MonoBehaviour
{
    [SerializeField]
    private float playerspeed;

    [SerializeField]
    private float movementDuration;

    private MovementState moveState = MovementState.Idle;

    private float accelerationProgress = 0.0f;

    private float remainingDuration;


    private void Start()
    {
        RefreshPlayerMovementDuration();
    }


    public void RefreshPlayerMovementDuration()
    {
        remainingDuration = movementDuration;

        //if already moving don't trigger coroutine again or it will overlap!
        if (moveState == MovementState.Idle)
        {
            moveState = MovementState.Move;
            accelerationProgress = 1.0f;
            remainingDuration = movementDuration;
        }
    }


    private void MovePlayer()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        
        position.x += playerspeed * accelerationProgress * Time.deltaTime;
        
        gameObject.transform.position = position;

        //Debug.Log("Movement progress: " + playerAnimator.GetFloat("Movement"));
    }


    private void Update()
    {
        if (moveState == MovementState.Move)
        {
            remainingDuration -= Time.deltaTime;

            MovePlayer();

            if (remainingDuration <= 0)
            {
                moveState = MovementState.Idle;
                accelerationProgress = 0.0f;
            }
        }
    }
}
