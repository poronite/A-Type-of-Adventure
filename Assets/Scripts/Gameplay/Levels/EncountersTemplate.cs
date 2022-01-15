using UnityEngine;

//encounters that are triggered when the player types a certain word
//there are several types of encounters:
//Gameplay encounter: trigger a function that affects the player gameplay like restoring the player's hp
//Cutscene encounter: display a cutscene


public enum EncounterType
{
    Gameplay,
    Cutscene
}

[CreateAssetMenu(fileName = "New_Encounter", menuName = "New Encounter", order = 53)]
public class EncountersTemplate : ScriptableObject
{
    //both
    public EncounterType EncounterType;

    //gameplay
    //insert here gameplay stuff

    //cutscene
    //to signal when to end the cutscene and return to adventure
    public bool EndOfCutscene;

    //duration of the transition between stripes
    public float TransitionDuration;

    //is this encounter a new cutscene or a continuation from a previous cutscene
    public bool NewCutscene;

    //if cutscene is new, instantiate new cutscene
    public GameObject CutscenePrefab;
}
