using UnityEngine;

public class Puzzle : MonoBehaviour
{
    //variables
    private string correctWord = string.Empty;

    private Sprite questionBoard;

    private LevelTemplate nextLevel;


    //delegates
    delegate void SendWordDelegate(string word);
    SendWordDelegate SendAnswerPzl;

    delegate void DisplayQuestionDelegate(Sprite questionBoard);
    DisplayQuestionDelegate DisplayQuestionBoardPzl;

    delegate void OutputUIDelegate(string output);
    OutputUIDelegate AddCharacterPzl;

    delegate void ResetAnswerDelegate();
    ResetAnswerDelegate DeleteWordPzl;

    delegate void ChangeLevelDelegate(LevelTemplate level);
    ChangeLevelDelegate GoToNextLevel;



    public void SetDelegatesPzl()
    {
        PuzzleUI PzlUIController = GameObject.FindGameObjectWithTag("PuzzleGfxUI").GetComponent<PuzzleUI>();

        DisplayQuestionBoardPzl = PzlUIController.DisplayQuestionBoardUIPzl;
        AddCharacterPzl = PzlUIController.AddCharacterUIPzl;
        DeleteWordPzl = PzlUIController.DeleteWordUIPzl;

        SendAnswerPzl = gameObject.GetComponent<Typing>().NewWord;
        GoToNextLevel = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ChangeLevel;
    }


    public void StartPuzzle(string correctWordPzl, Sprite questionBoardPzl, LevelTemplate nextLevelPzl)
    {
        nextLevel = nextLevelPzl;
        correctWord = correctWordPzl.ToLower();
        questionBoard = questionBoardPzl;
        ResetAnswerPzl();
        SendAnswerPzl.Invoke(correctWord);
        DisplayQuestionBoardUI();
        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Puzzle;
        Debug.Log("Puzzle Time!");
    }


    ///<summary>Display the question board of the puzzle when level starts.</summary>
    private void DisplayQuestionBoardUI()
    {
        DisplayQuestionBoardPzl.Invoke(questionBoard);
    }


    public void AddCharacterUI(string character)
    {
        AddCharacterPzl.Invoke(character);
    }


    public void ResetAnswerPzl()
    {
        DeleteWordPzl.Invoke();
    }


    public void CompleteWordPzl(string word)
    {
        GoToNextLevel.Invoke(nextLevel);
    }
}
