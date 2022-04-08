using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeUI : MonoBehaviour
{
    //References
    [SerializeField]
    private RectTransform currentTextUITransform;

    [SerializeField]
    private RectTransform outputTextUITransform;


    [SerializeField]
    private TextMeshProUGUI currentWordUIChl;

    [SerializeField]
    private TextMeshProUGUI outputWordUIChl;

    [SerializeField]
    private Image challengeBoardUIChl;

    [SerializeField]
    private Image progressBarFillUIChl;


    
    public void SetChallengeBoardUIChl(Sprite challengeBoard, Sprite fill)
    {
        challengeBoardUIChl.sprite = challengeBoard;
        progressBarFillUIChl.sprite = fill;
    }


    public void AddCharacterUIChl(string character)
    {
        outputWordUIChl.text += character;
    }


    public void DisplayNewCurrentWordUIChl(string word)
    {
        currentWordUIChl.text = word;

        Vector2 textPosition = currentTextUITransform.localPosition;
        Vector2 textSize = currentWordUIChl.GetPreferredValues(word);

        currentTextUITransform.sizeDelta = textSize;
        currentTextUITransform.localPosition = textPosition;
        outputTextUITransform.sizeDelta = textSize;
        outputTextUITransform.localPosition = textPosition;
    }


    public void ClearOutputWordUIChl()
    {
        outputWordUIChl.text = string.Empty;
    }


    public void UpdateProgressBarFillUIChl(float fillAmount)
    {
        progressBarFillUIChl.fillAmount = fillAmount / 100;
    }
}
