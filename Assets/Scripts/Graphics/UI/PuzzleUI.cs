using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI outputWordUIPzl;

    [SerializeField]
    private Image questionBoardUIPzl;



    public void DisplayQuestionBoardUIPzl(Sprite questionBoard)
    {
        questionBoardUIPzl.sprite = questionBoard;
    }


    public void AddCharacterUIPzl(string character)
    {
        outputWordUIPzl.text += character;
    }


    public void DeleteWordUIPzl()
    {
        outputWordUIPzl.text = string.Empty;
    }
}
