using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=zit45k6CUMk

public class ParalaxEffectAdv : MonoBehaviour
{
    //variables
    private float spriteLength, startPosition;

    [SerializeField]
    private float parallaxEffect;


    //references
    public GameObject cam;


    private void Start()
    {
        startPosition = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    private void Update()
    {
        //to determine if last background should move to the right
        float limit = cam.transform.position.x * (1 - parallaxEffect);
        float distanceFromStartPosition = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPosition + distanceFromStartPosition, transform.position.y, transform.position.z);

        //move sprite to the right if camera is past the border
        if (limit > startPosition + spriteLength)
        {
            startPosition += spriteLength;
        }
        else if (limit < startPosition - (spriteLength / 2))
        {
            startPosition -= spriteLength;
        }
    }

}
