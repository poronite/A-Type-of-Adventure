using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Copy pasta from: https://www.youtube.com/watch?v=3UO-1suMbNc and https://www.youtube.com/watch?v=Mp6BWCMJZH4
//with a few changes to make it fitting for the game


public class ParalaxEffectAdv : MonoBehaviour
{
    private int numChildren = 3;
    private GameObject[] layers;
    private GameObject[] layer3Children;
    private GameObject[] layer2Children;
    private GameObject[] layer1Children;

    [SerializeField] 
    private Sprite[] layer3Sprites;

    [SerializeField]
    private Sprite[] layer2Sprites;

    [SerializeField]
    private Sprite[] layer1Sprites;

    private GameObject playerGfx;
    private Vector3 playerOriginalPosition;

    private Camera mainCamera;
    private Vector2 screenBounds;

    private Vector3 lastScreenPosition;



    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        playerGfx = GameObject.FindGameObjectWithTag("PlayerGfx");
        playerOriginalPosition = playerGfx.transform.position;

        Debug.Log($"Original Position: {playerOriginalPosition.x}, {playerOriginalPosition.y}, {playerOriginalPosition.z}");

        layers = GameObject.FindGameObjectsWithTag("ParalaxLayer");

        layer3Children = new GameObject[numChildren];
        layer2Children = new GameObject[numChildren];
        layer1Children = new GameObject[numChildren];

        foreach (GameObject obj in layers)
        {
            Debug.Log("Layer: " + obj.name);
            LoadChildObjects(obj);
        }

        lastScreenPosition = transform.position;
    }


    private void LoadChildObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        GameObject clone = Instantiate(obj) as GameObject;

        for (int i = 0; i < numChildren; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + $" {i}";

            switch (c.name.Split(' ')[0])
            {
                case "Layer3":
                    layer3Children[i] = c;
                    break;
                case "Layer2":
                    layer2Children[i] = c;
                    break;
                case "Layer1":
                    layer1Children[i] = c;
                    break;
                default:
                    break;
            }
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
    

    public void ChangeField()
    {
        for (int i = 0; i < numChildren; i++)
        {
            string name = layers[i].name;

            switch (name)
            {
                case "Layer3":
                    layer3Children[i].GetComponent<SpriteRenderer>().sprite = layer3Sprites[i];
                    break;
                case "Layer2":
                    layer2Children[i].GetComponent<SpriteRenderer>().sprite = layer2Sprites[i];
                    break;
                case "Layer1":
                    layer1Children[i].GetComponent<SpriteRenderer>().sprite = layer1Sprites[i];
                    break;
                default:
                    break;
            }
        }
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
