using UnityEngine;

//events that are triggered when the player types a certain word
//there are 2 types of events: 
//events that can change stats for example restoring the players hp
//graphics events that trigger an animation or display a picture for example a thief that jumps into the players path

public enum EventType
{
    Animation
}

[CreateAssetMenu(fileName = "New_Event", menuName = "New Event", order = 53)]
public class EventsTemplate : ScriptableObject
{
    [SerializeField]
    public EventType eventType;

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
