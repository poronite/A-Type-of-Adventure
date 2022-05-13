using UnityEngine;

//encounters that are triggered when the player types a certain word
//there are 2 types of encounters:
//Image encounter: show an image on screen
//Cutscene encounter: display a cutscene


public enum EncounterType
{
    Image,
    Cutscene
}

[CreateAssetMenu(fileName = "New_Encounter", menuName = "New Encounter", order = 53)]
public class EncountersTemplate : ScriptableObject
{
    //both
    public EncounterType EncounterType;

    public FieldType FieldType;

    //image
    //image to display
    public bool DisplayImage;

    public Sprite ImageToDisplay;

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
