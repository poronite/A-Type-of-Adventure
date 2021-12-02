using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdventureUI : MonoBehaviour
{
    //References
    [SerializeField]
    private TextMeshProUGUI writtenTextUIAdv;

    [SerializeField]
    private TextMeshProUGUI currentTextUIAdv;

    [SerializeField]
    private TextMeshProUGUI outputTextUIAdv;

    [SerializeField]
    private TextMeshProUGUI remainingTextUIAdv;



    /// <summary>Display output with new character added.</summary>
    public void DisplayNewOutputWordUIAdv(string character)
    {
        outputTextUIAdv.text += character;
    }

    /// <summary>Clear outputWordUIAdv after player has finished typing the word.</summary>
    public void ClearOutputWordUIAdv()
    {
        outputTextUIAdv.text = string.Empty;
    }

    public void UpdateWrittenTextUIAdv(string text)
    {
        writtenTextUIAdv.text = text;
    }

    public void UpdateRemainingTextUIAdv(string text)
    {
        remainingTextUIAdv.text = text;
    }

    public void DisplayNewCurrentWordUIAdv(string word)
    {
        currentTextUIAdv.text = word;
    }
}
