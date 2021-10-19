using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    //References
    [SerializeField]
    private Text timeElapsedUI;

    [SerializeField]
    private Text numMistakesUI;



    public void SetTimeElapsedUI(int time)
    {
        string timeConverted = ConvertToMinSec(time);

        timeElapsedUI.text = "Time elapsed: " + timeConverted;
    }

    private string ConvertToMinSec(int time)
    {
        return (time / 60).ToString() + ":" + (time % 60).ToString("00");
    }

    public void SetMistakesUI(int mistakes)
    {
        numMistakesUI.text = $"Mistakes: {mistakes}";
    }
}
