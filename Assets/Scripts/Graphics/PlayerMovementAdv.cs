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
    private float accelerationDuration;

    [SerializeField]
    private float movementDuration;

    private float accelerationProgress = 0.0f;

    private float remainingDuration;

    //if player is currently moving 
    private bool isMoving = false;

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

        //if already moving don't trigger coroutine again or it will overlap!
        if (!isMoving) 
        {
            StartCoroutine(SmoothMovementChange(1f));
        }
    }


    //set if player can move or not regardless if player has typed a word
    public void SetPlayerMovement(bool movementState)
    {
        isPlayerStopped = movementState;

        if (!isPlayerStopped)
        {
            StartCoroutine(SmoothMovementChange(1.0f));
        }
        else if (isPlayerStopped)
        {
            StartCoroutine(SmoothMovementChange(0.0f));
        }
    }


    private void MovePlayer()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        
        position.x += playerspeed * accelerationProgress * Time.deltaTime;
        
        gameObject.transform.position = position;

        playerAnimator.SetFloat("Movement", accelerationProgress);

        Debug.Log("Movement progress: " + playerAnimator.GetFloat("Movement"));
    }


    IEnumerator SmoothMovementChange(float targetValue)
    {
        float time = 0.0f;

        while (time < accelerationDuration)
        {
            accelerationProgress = Mathf.Lerp(accelerationProgress, targetValue, time / accelerationDuration);
            MovePlayer();
            time += Time.deltaTime;
            yield return null;
        }

        accelerationProgress = targetValue;

        if (accelerationProgress >= 1.0)
        {
            isMoving = true;
        }
    }


    private void Update()
    {
        if (isMoving && !isPlayerStopped)
        {
            remainingDuration -= Time.deltaTime;

            MovePlayer();

            if (remainingDuration <= 0)
            {
                //if already in idle don't trigger coroutine again or it will overlap!
                if (isMoving)
                {
                    isMoving = false;
                    StartCoroutine(SmoothMovementChange(0.0f));
                }
            }
        }
    }
}
