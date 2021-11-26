using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Copy pasta from: https://www.youtube.com/watch?v=3UO-1suMbNc and https://www.youtube.com/watch?v=Mp6BWCMJZH4
//with a few changes to make it fitting for the game


public class ParalaxEffectAdv : MonoBehaviour
{
    [SerializeField]
    private GameObject[] layers;

    [SerializeField] 
    private Sprite[] layer3Sprites;

    [SerializeField]
    private Sprite[] layer2Sprites;

    [SerializeField]
    private Sprite[] layer1Sprites;

    [SerializeField]
    private GameObject playerGfx;
    private Vector3 playerOriginalPosition;

    private Camera mainCamera;
    private Vector2 screenBounds;

    private Vector3 lastScreenPosition;



    private void Awake()
    {
        playerOriginalPosition = playerGfx.transform.position;

        Debug.Log($"Original Position: {playerOriginalPosition.x}, {playerOriginalPosition.y}, {playerOriginalPosition.z}");

        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        foreach (GameObject obj in layers)
        {
            LoadChildObjects(obj);
        }

        lastScreenPosition = transform.position;
    }


    private void LoadChildObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        //int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        int childsNeeded = 3;
        GameObject clone = Instantiate(obj) as GameObject;

        for (int i = 0; i < childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);

            //Debug.Log("Sprite Number: " + i);

            c.name = obj.name; //change name temporary for the following switch case

            switch (c.name)
            {
                case "Layer3":
                    Debug.Log("Sprite Name: " + layer3Sprites[i].name);
                    c.GetComponent<SpriteRenderer>().sprite = layer3Sprites[i];
                    break;
                case "Layer2":
                    Debug.Log("Sprite Name: " + layer2Sprites[i].name);
                    c.GetComponent<SpriteRenderer>().sprite = layer2Sprites[i];
                    break;
                case "Layer1":
                    Debug.Log("Sprite Name: " + layer1Sprites[i].name);
                    c.GetComponent<SpriteRenderer>().sprite = layer1Sprites[i];
                    break;
                default:
                    break;
            }

            c.name = obj.name + $" {i}"; //give a distinct name to the object
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }


    private void RepositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }

    //reset positions of camera, layers and lastScreenPosition (used when going into combat or puzzle)
    public void ResetPositions()
    {
        playerGfx.transform.position = playerOriginalPosition;

        transform.position = new Vector3(0, 0, transform.position.z);

        foreach (GameObject obj in layers)
        {
            obj.transform.position = new Vector3(0, 0, obj.transform.position.z);
        }

        lastScreenPosition = transform.position;
    }
    

    void LateUpdate()
    {
        foreach (GameObject obj in layers)
        {
            RepositionChildObjects(obj);
            float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(transform.position.z / obj.transform.position.z));
            float difference = transform.position.x - lastScreenPosition.x;
            obj.transform.Translate(Vector3.right * difference * parallaxSpeed);
        }

        lastScreenPosition = transform.position;
    }
}
