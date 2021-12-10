using UnityEngine;

//encounters that are triggered when the player types a certain word
//there are several types of encounters:
//Gameplay encounter: trigger a function that affects the player gameplay like restoring the player's hp
//Graphics encounter: spawn an npc and ambush the player usually used before a battle


public enum EncounterType
{
    Gameplay,
    Graphics
}

[CreateAssetMenu(fileName = "New_Encounter", menuName = "New Encounter", order = 53)]
public class EncountersTemplate : ScriptableObject
{
    //both
    public EncounterType EncounterType;

    public bool StopPlayer;

    public float delayBeforePlayerStopping;

    //graphics
    public GameObject EncounterGfxObject;

    public Vector3 SpawnOffset;

    public string AnimationToPlay;
}
