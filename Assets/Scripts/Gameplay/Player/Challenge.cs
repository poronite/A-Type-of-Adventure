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


    //delegates
    delegate void ChangeLevelDelegate(LevelTemplate level);
    ChangeLevelDelegate GoToNextLevel;

    delegate void AddCharacterDelegate(string character);
    AddCharacterDelegate AddCharacterChl;

    delegate void DisplayWordDelegate(string word);
    DisplayWordDelegate DisplayNewCurrentWordChl;

    delegate void ClearWordDelegate();
    ClearWordDelegate ClearCurrentWordChl;
    ClearWordDelegate ClearOutputWordChl;

    delegate void UpdateFillDelegate(float fill);
    UpdateFillDelegate UpdateProgressBarFillChl;




    public void SetDelegatesChl()
    {
        LevelController levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        GoToNextLevel = levelController.ChangeLevel;

        ChallengeUI chlUIController = GameObject.FindGameObjectWithTag("ChallengeGfxUI").GetComponent<ChallengeUI>();
        AddCharacterChl = chlUIController.AddCharacterUIChl;
        DisplayNewCurrentWordChl = chlUIController.DisplayNewCurrentWordUIChl;
        ClearCurrentWordChl = chlUIController.ClearCurrentWordUIChl;
        ClearOutputWordChl = chlUIController.ClearOutputWordUIChl;
        UpdateProgressBarFillChl = chlUIController.UpdateProgressBarFillUIChl;
    }


    public void StartChallenge(LevelTemplate currentLevel)
    {
        nextLevel = currentLevel.NextLevelAfterChallenge;

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
        EnergyBarEmpty();
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

        //verify winning condition
        EnergyBarComplete();

        //allow progress bar to empty again
        stopReducingEnergy = false;
        
        yield return null;
    }


    private void GenerateWordChl()
    {

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
    private void EnergyBarComplete()
    {
        if (currentEnergy >= 100f)
        {
            //win
            stopReducingEnergy = true;
            Debug.Log("Energy Filled. Win");
            GoToNextLevel.Invoke(nextLevel);
        }
    }


    public void AddCharacterUI(string character)
    {
        AddCharacterChl.Invoke(character);
    }
}
