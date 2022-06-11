using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Copy pasta from: https://www.youtube.com/watch?v=3UO-1suMbNc and https://www.youtube.com/watch?v=Mp6BWCMJZH4
//with a few changes to make it fitting for the game


public class BackgroundManager : MonoBehaviour
{
    //non paralax background related variables
    [SerializeField]
    private SpriteRenderer backgroundCmb;
    [SerializeField]
    private SpriteRenderer backgroundPzl;
    [SerializeField]
    private SpriteRenderer backgroundChl;

    [SerializeField]
    private Sprite plainsBackgroundCmb;
    [SerializeField]
    private Sprite plainsBackgroundPzl;
    [SerializeField]
    private Sprite plainsBackgroundChl;

    [SerializeField]
    private Sprite magicForestBackgroundCmb;
    [SerializeField]
    private Sprite magicForestBackgroundPzl;
    [SerializeField]
    private Sprite magicForestBackgroundChl;

    [SerializeField]
    private Sprite citadelBackgroundCmb;
    [SerializeField]
    private Sprite citadelBackgroundPzl;
    [SerializeField]
    private Sprite citadelBackgroundChl;

    //paralax layer related variables
    private int numChildren = 3;

    private GameObject[] layers;

    private GameObject[] layer3Children;
    private GameObject[] layer2Children;
    private GameObject[] layer1Children;
    
    private Sprite[] layer3Sprites;    
    private Sprite[] layer2Sprites;    
    private Sprite[] layer1Sprites;

    [SerializeField]
    private Sprite[] plainsGroundSprites;
    [SerializeField]
    private Sprite[] plainsTreeSprites;
    [SerializeField]
    private Sprite[] plainsSkySprites;

    [SerializeField]
    private Sprite[] magicForestGroundSprites;
    [SerializeField]
    private Sprite[] magicForestTreeSprites;
    [SerializeField]
    private Sprite[] magicForestSkySprites;

    [SerializeField]
    private Sprite[] citadelGroundSprites;
    [SerializeField]
    private Sprite[] citadelWallSprites;
    [SerializeField]
    private Sprite[] citadelSkySprites;


    //camera and player related variables and references
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

        //Debug.Log($"Original Position: {playerOriginalPosition.x}, {playerOriginalPosition.y}, {playerOriginalPosition.z}");

        layers = GameObject.FindGameObjectsWithTag("ParalaxLayer");

        layer3Children = new GameObject[numChildren];
        layer2Children = new GameObject[numChildren];
        layer1Children = new GameObject[numChildren];

        layer3Sprites = new Sprite[numChildren];
        layer2Sprites = new Sprite[numChildren];
        layer1Sprites = new Sprite[numChildren];

        foreach (GameObject obj in layers)
        {
            //Debug.Log("Layer: " + obj.name);
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

        //Debug.Log("Layers ready");

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
    
    
    public void ChangeFieldNonParalax(FieldType type)
    {
        switch (type)
        {
            case FieldType.Plains:
                backgroundCmb.sprite = plainsBackgroundCmb;
                backgroundPzl.sprite = plainsBackgroundPzl;
                backgroundChl.sprite = plainsBackgroundChl;
                break;
            case FieldType.MagicForest:
                backgroundCmb.sprite = magicForestBackgroundCmb;
                backgroundPzl.sprite = magicForestBackgroundPzl;
                backgroundChl.sprite = magicForestBackgroundChl;
                break;
            case FieldType.Citadel:
                backgroundCmb.sprite = citadelBackgroundCmb;
                backgroundPzl.sprite = citadelBackgroundPzl;
                backgroundChl.sprite = citadelBackgroundChl;
                break;
            default:
                break;
        }
    }


    public void ChangeFieldParalax(FieldType type)
    {
        //Debug.Log("Change Field");

        switch (type)
        {
            case FieldType.Plains:
                layer1Sprites = plainsGroundSprites;
                layer2Sprites = plainsTreeSprites;
                layer3Sprites = plainsSkySprites;
                break;
            case FieldType.MagicForest:
                layer1Sprites = magicForestGroundSprites;
                layer2Sprites = magicForestTreeSprites;
                layer3Sprites = magicForestSkySprites;
                break;
            case FieldType.Citadel:
                layer1Sprites = citadelGroundSprites;
                layer2Sprites = citadelWallSprites;
                layer3Sprites = citadelSkySprites;
                break;
            default:
                break;
        }


        for (int i = 0; i < layers.Length; i++)
        {
            string layerParentName = layers[i].name;

            for (int j = 0; j < numChildren; j++)
            {
                //Debug.Log($"Layer Parent: {layerParentName}");

                switch (layerParentName)
                {
                    case "Layer1":
                        layer1Children[j].GetComponent<SpriteRenderer>().sprite = layer1Sprites[j];
                        break;
                    case "Layer2":
                        layer2Children[j].GetComponent<SpriteRenderer>().sprite = layer2Sprites[j];
                        break;
                    case "Layer3":
                        layer3Children[j].GetComponent<SpriteRenderer>().sprite = layer3Sprites[j];
                        break;
                    default:
                        break;
                }
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
