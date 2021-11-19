using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffectAdv : MonoBehaviour
{
    //variables
    private float spriteLength, startPosition;

    [SerializeField]
    private float parallaxEffectSpeed;


    //references
    public GameObject cam;


    private void Start()
    {
        startPosition = transform.position.x;
        spriteLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    private void Update()
    {
        float distance = (cam.transform.position.x * parallaxEffectSpeed);

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        
        //move sprite to the right or left if camera is past the border
        if (transform.position.x - cam.transform.position.x  > 19.2f)
        {
            startPosition += spriteLength;
        }
        else if (transform.position.x - cam.transform.position.x < -19.2f)
        {
            startPosition -= spriteLength;
        }
    }

}
