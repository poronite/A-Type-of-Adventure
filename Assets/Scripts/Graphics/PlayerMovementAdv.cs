using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script that will move the player graphics for 5s when the player clicks.
//the duration is refreshed to 5s even if there's still remaining seconds left

public class PlayerMovementAdv : MonoBehaviour
{
    [SerializeField]
    private float playerspeed;

    [SerializeField]
    private float movementDuration;

    private float remainingDuration;

    //if player is currently moving 
    private bool isMoving = true;

    //force player to stop
    private bool isPlayerStopped = false;

    [SerializeField]
    private Animator playerAnimator;


    private void Start()
    {
        RefreshPlayerMovementDuration();
    }


    public void RefreshPlayerMovementDuration()
    {
        remainingDuration = movementDuration;

        isMoving = true;

        if (!isPlayerStopped)
        {
            playerAnimator.Play("Walk", 0);
        }
    }


    //set if player can move or not regardless if player has typed a word
    public void SetPlayerMovement(bool movementState)
    {
        isPlayerStopped = movementState;

        if (isPlayerStopped)
        {
            playerAnimator.Play("Idle", 0);
        }
    }


    private void MovePlayer()
    {
        if (isMoving && !isPlayerStopped)
        {
            remainingDuration -= Time.deltaTime;

            Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

            position.x += playerspeed * Time.deltaTime;

            gameObject.transform.position = position;

            if (remainingDuration <= 0)
            {
                isMoving = false;

                playerAnimator.Play("Idle", 0);
            }
        }
    }


    private void Update()
    {
        MovePlayer();
    }
}
