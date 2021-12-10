using UnityEngine;

//encounters that are triggered when the player types a certain word
//there are several types of encounters:
//Appear encounter: spawn an npc for example
//Assault encounter: spawn an npc and ambush the player usually used before a battle


public enum EncounterType
{
    Appear,
    Assault
}

[CreateAssetMenu(fileName = "New_Enconter", menuName = "New Enconter", order = 53)]
public class EncountersTemplate : ScriptableObject
{
    public EncounterType EncounterType;

    public GameObject EnconterTarget;

    public Vector3 SpawnOffset;

    public bool StopPlayer;

    public string AnimationToPlay;
}
