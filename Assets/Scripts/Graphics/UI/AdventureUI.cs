using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdventureUI : MonoBehaviour
{
    //References
    [SerializeField]
    private RectTransform advUIElements;

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

    [SerializeField]
    private TextMeshProUGUI hintTextUIAdv;

    [SerializeField]
    private TextMeshProUGUI branchWord1UIAdv;

    [SerializeField]
    private TextMeshProUGUI branchWord2UIAdv;



    ///<summary>Display output with new character added.</summary>
    public void AddCharacterUIAdv(string character)
    {
        OffsetAdvUIElementsPosition(character);
        outputTextUIAdv.text += character;
    }


    ///<summary>Clear outputWordUIAdv after player has finished typing the word.</summary>
    public void ClearOutputWordUIAdv()
    {
        outputTextUIAdv.text = string.Empty;
        ResetAdvUIElementsPosition();
    }

    
    public void ClearCurrentWordUIAdv()
    {
        currentTextUIAdv.text = string.Empty;
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
        ResetAdvUIElementsPosition();

        currentTextUIAdv.text = word;

        Vector2 textSize = currentTextUIAdv.GetPreferredValues(word);

        currentTextUITransform.sizeDelta = textSize;
        currentTextUITransform.localPosition = new Vector2(0, 0);
        outputTextUITransform.sizeDelta = textSize;
        outputTextUITransform.localPosition = new Vector2(0, 0);
    }


    public void DisplayNewHintUIAdv(string hint)
    {
        hintTextUIAdv.text = hint;
    }


    //simulate the movement of a typewriter
    //move the adventure UI elements position when player types a word
    private void OffsetAdvUIElementsPosition(string character)
    {
        float letterWidth = currentTextUIAdv.GetPreferredValues(character).x;

        advUIElements.localPosition -= new Vector3(letterWidth, advUIElements.localPosition.y, advUIElements.localPosition.z);
    }


    //Reset adventure UI elements position after player has finished typing the current word
    private void ResetAdvUIElementsPosition()
    {
        advUIElements.localPosition = new Vector3(0, advUIElements.localPosition.y, advUIElements.localPosition.z);
    }


    public void DisplayBranchingWordsUIAdv(List<string> words)
    {
        branchWord1UIAdv.text = words[0];
        branchWord2UIAdv.text = words[1];
    }


    public void DisplayChosenBranchingWordUIAdv(string word)
    {
        branchWord1UIAdv.text = string.Empty;
        branchWord2UIAdv.text = string.Empty;

        currentTextUIAdv.text = word;
    }
}
