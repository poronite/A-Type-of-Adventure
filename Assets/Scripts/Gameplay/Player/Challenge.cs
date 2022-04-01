using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This minigame consists of a progress bar that lowers it's percentage over time.
//A word will appear on the screen and if the player completes that word the progress bar is filled by a certain amount.


public class Challenge : MonoBehaviour
{
    //variables
    private float energyLostPerSecond;
    private float energyGainedPerWord;
    private float currentEnergy;

    //variable used to prevent the script from reducing energy value
    //used when smoothly increasing the energy after typing a word
    //used when player wins challenge
    private bool stopReducingEnergy = false;

    //duration that the progress bar takes when filling after player typed a word
    private float increaseFillDuration = 0.5f;

    private LevelTemplate nextLevel;

    private List<string> currentLevelWordList = new List<string>();

    private string lastWordUsed;


    //delegates
    delegate void WordDelegate(string word);
    WordDelegate SendNextWordChl;

    delegate void ChangeLevelDelegate(LevelTemplate level);
    ChangeLevelDelegate GoToNextLevel;

    delegate void AddCharacterDelegate(string character);
    AddCharacterDelegate AddCharacterChl;

    delegate void DisplayWordDelegate(string word);
    DisplayWordDelegate DisplayNewCurrentWordChl;

    delegate void ClearWordDelegate();
    ClearWordDelegate ClearOutputWordChl;

    delegate void UpdateFillDelegate(float fill);
    UpdateFillDelegate UpdateProgressBarFillChl;




    public void SetDelegatesChl()
    {
        SendNextWordChl = gameObject.GetComponent<Typing>().NewWord;

        LevelController levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        GoToNextLevel = levelController.ChangeLevel;

        ChallengeUI chlUIController = GameObject.FindGameObjectWithTag("ChallengeGfxUI").GetComponent<ChallengeUI>();
        AddCharacterChl = chlUIController.AddCharacterUIChl;
        DisplayNewCurrentWordChl = chlUIController.DisplayNewCurrentWordUIChl;
        ClearOutputWordChl = chlUIController.ClearOutputWordUIChl;
        UpdateProgressBarFillChl = chlUIController.UpdateProgressBarFillUIChl;
    }


    public void StartChallenge(LevelTemplate currentLevel)
    {
        currentEnergy = currentLevel.StartingEnergy;
        energyLostPerSecond = currentLevel.EnergyLostPerSecond;
        energyGainedPerWord = currentLevel.EnergyGainedPerWord;
        currentLevelWordList = currentLevel.WordList;
        nextLevel = currentLevel.NextLevelAfterChallenge;

        NewWordChl();

        gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.Challenge;

        stopReducingEnergy = false;
    }


    void Update()
    {
        if (!stopReducingEnergy)
        {
            ReduceEnergy();
        }
    }


    //over time
    private void ReduceEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy - energyLostPerSecond * Time.deltaTime, 0f, 100f);
        UpdateProgressBarFillChl.Invoke(currentEnergy);
        //EnergyBarEmpty();
    }


    public void CompleteWordChl(string word)
    {
        StartCoroutine(IncreaseEnergy());
    }

    IEnumerator IncreaseEnergy()
    {
        //prevent progress bar from emptying while it's filling
        stopReducingEnergy = true;

        float targetEnergy = Mathf.Clamp(currentEnergy + energyGainedPerWord, 0f, 100f);
        float time = 0f;
        
        while (time < increaseFillDuration)
        {
            currentEnergy = Mathf.Lerp(currentEnergy, targetEnergy, time / increaseFillDuration);
            time += Time.deltaTime;
            UpdateProgressBarFillChl.Invoke(currentEnergy);
        }

        //guarantee that the current energy is equal to the final value
        currentEnergy = targetEnergy;
        UpdateProgressBarFillChl.Invoke(currentEnergy);

        NewWordChl();

        //verify winning condition
        if (!EnergyBarComplete()) //if hasn't completed challenge keep the game going
        {
            //allow progress bar to empty again
            stopReducingEnergy = false;
        }

        yield return null;
    }


    private void NewWordChl() 
    {
        string word = GenerateWordChl(currentLevelWordList, lastWordUsed);
        DisplayNewCurrentWordChl.Invoke(word);
        ClearOutputWordChl.Invoke();
        SendNextWordChl.Invoke(word);
        lastWordUsed = word;
    }


    private string GenerateWordChl(List<string> wordList, string lastGeneratedWord)
    {
        int newWordIndex;

        //if I don't make a clone the function will end up removing the words from the original list lol
        List<string> wordListClone = new List<string>(wordList);

        //reset random
        Random.InitState((int)Time.time);

        //remove last used word
        if (lastGeneratedWord != string.Empty)
            wordListClone.Remove(lastGeneratedWord);

        //select new word to generate
        newWordIndex = Random.Range(0, wordListClone.Count - 1);

        return wordListClone[newWordIndex];
    }


    private void EnergyBarEmpty()
    {
        if (currentEnergy <= 0f)
        {
            //lose
            Debug.Log("No energy. Lose");
        }
    }


    //complete challenge
    private bool EnergyBarComplete()
    {
        if (currentEnergy >= 100f)
        {
            //win
            stopReducingEnergy = true;
            gameObject.GetComponent<Typing>().CurrentPlayerState = PlayerState.ChallengeComplete;
            Debug.Log("Energy Filled. Win");
            GoToNextLevel.Invoke(nextLevel);
            return true;
        }
        return false;
    }


    public void AddCharacterUI(string character)
    {
        AddCharacterChl.Invoke(character);
    }
}
