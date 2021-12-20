using UnityEngine;

public class Puzzle : MonoBehaviour
{
    //variables
    private string correctWord = string.Empty;

    private Sprite questionBoard;

    private LevelTemplate nextLevel;



    public void SetDelegatesPzl()
    {

    }


    public void StartPuzzle(string correctWordPzl, Sprite questionBoardPzl, LevelTemplate nextLevelPzl)
    {
        nextLevel = nextLevelPzl;
        correctWord = correctWordPzl;
        questionBoard = questionBoardPzl;
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Puzzle;
        Debug.Log("Puzzle Time!");
    }
}
