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
    public EventType EventType;

    public GameObject EventTarget;

    public Vector3 SpawnOffset;

    public string AnimationToPlay;
}
