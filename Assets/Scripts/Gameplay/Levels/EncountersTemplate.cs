using UnityEngine;

//enconters that are triggered when the player types a certain word
//there are 2 types of enconters: 
//enconters that can change stats for example restoring the players hp
//graphics enconters that trigger an animation or display a picture for example a thief that jumps into the players path

public enum EncounterType
{
    Assault
}

[CreateAssetMenu(fileName = "New_Enconter", menuName = "New Enconter", order = 53)]
public class EncountersTemplate : ScriptableObject
{
    public EncounterType EnconterType;

    public GameObject EnconterTarget;

    public Vector3 SpawnOffset;

    public string AnimationToPlay;
}
