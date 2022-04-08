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
    private float accelerationDuration;

    [SerializeField]
    private float movementDuration;

    private MovementState moveState = MovementState.Idle;

    private float accelerationProgress = 0.0f;

    private float remainingDuration;

    private Coroutine movementCoroutine = null;

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
        if (moveState == MovementState.Idle)
        {
            isMovementCoroutineRunning();
            moveState = MovementState.Move;
            movementCoroutine = StartCoroutine(SmoothMovementChange());
        }
    }


    private void MovePlayer()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        
        position.x += playerspeed * accelerationProgress * Time.deltaTime;
        
        gameObject.transform.position = position;

        playerAnimator.SetFloat("Movement", accelerationProgress);

        //Debug.Log("Movement progress: " + playerAnimator.GetFloat("Movement"));
    }


    IEnumerator SmoothMovementChange()
    {
        float time = 0.0f;
        float targetValue = 0.0f;

        switch (moveState)
        {
            case MovementState.Idle:
                targetValue = 0.0f;
                break;
            case MovementState.Move:
                targetValue = 1.0f;
                break;
            default:
                break;
        }

        while (time < accelerationDuration)
        {
            accelerationProgress = Mathf.Lerp(accelerationProgress, targetValue, time / accelerationDuration);
            MovePlayer();
            time += Time.deltaTime;
            yield return null;
        }

        accelerationProgress = targetValue;

        movementCoroutine = null;
    }


    private void isMovementCoroutineRunning()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
    }


    private void Update()
    {
        if (moveState == MovementState.Move)
        {
            remainingDuration -= Time.deltaTime;

            MovePlayer();

            if (remainingDuration <= 0)
            {
                isMovementCoroutineRunning();
                moveState = MovementState.Idle;
                movementCoroutine = StartCoroutine(SmoothMovementChange());
            }
        }
    }
}
