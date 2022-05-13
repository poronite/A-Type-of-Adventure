using UnityEngine;
using UnityEngine.Video;

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

    //image
    //image to display
    public bool DisplayImage;

    public Sprite ImageToDisplay;

    //cutscene
    //video to play
    public VideoClip cutsceneVideo;

    //loop version to play after previous video
    public VideoClip cutsceneVideoLoop;

    //to signal when to end the cutscene and return to adventure
    public bool EndOfCutscene;
}
