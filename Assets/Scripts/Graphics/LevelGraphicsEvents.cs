using UnityEngine;

//script that represents a script instantiates an animation like the enemy jumping closer to the player for example

public enum EventType
{
    Animation
}

[CreateAssetMenu(fileName = "New_Graphics_Event", menuName = "New Graphics Event", order = 53)]
public class LevelGraphicsEvents : ScriptableObject
{
    [SerializeField]
    private EventType eventType;

    [SerializeField]
    private GameObject eventTarget;

    [SerializeField]
    private Vector3 spawnOffset;

    [SerializeField]
    private string animationToPlay;


    public void TriggerEvent()
    {
        switch (eventType)
        {
            case EventType.Animation:
                TriggerAnimation();
                break;
            default:
                break;
        }
    }

    private void TriggerAnimation()
    {
        Vector3 cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        
        GameObject instance = Instantiate(eventTarget, 
            new Vector3(cameraPosition.x + spawnOffset.x, cameraPosition.y + spawnOffset.y), 
            Quaternion.identity, GameObject.FindGameObjectWithTag("GraphicsEvents").transform);
        
        instance.GetComponent<Animator>().Play(animationToPlay, 0);
    }
}
