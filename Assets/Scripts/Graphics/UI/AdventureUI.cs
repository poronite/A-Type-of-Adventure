using UnityEngine;
using TMPro;

public class AdventureUI : MonoBehaviour
{
    //References
    [SerializeField]
    private RectTransform currentTextUITransform;

    [SerializeField]
    private RectTransform outputTextUITransform;

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

        //currentTextUIAdv.ForceMeshUpdate();

        Vector2 textSize = currentTextUIAdv.GetPreferredValues(word);

        currentTextUITransform.sizeDelta = textSize;
        currentTextUITransform.localPosition = new Vector2(0, 0);
        outputTextUITransform.sizeDelta = textSize;
        outputTextUITransform.localPosition = new Vector2(0, 0);
    }
}
