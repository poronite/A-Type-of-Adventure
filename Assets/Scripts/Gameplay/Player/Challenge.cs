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


    //delegates
    



    public void SetDelegatesChl()
    {
        LevelController levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();


        ChallengeUI chlUIController = GameObject.FindGameObjectWithTag("ChallengeGfxUI").GetComponent<ChallengeUI>();
    }


    void Update()
    {
        RefreshEnergyBar();
    }


    private void RefreshEnergyBar()
    {
        currentEnergy -= energyLostPerSecond * Time.deltaTime;
        EnergyBarEmpty();
        //update UI bar
        //currentEnergy / 100 = energy to send to UI
    }


    private void EnergyBarEmpty()
    {
        if (currentEnergy <= 0f)
        {
            //lose
            Debug.Log("No energy. Lose");
        }
    }


    public void AddEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy + energyGainedPerWord, 0f, 100f);
        //update UI bar
        //currentEnergy / 100 = energy to send to UI
    }


    private void EnergyBarComplete()
    {
        if (currentEnergy >= 100f)
        {
            //win
            Debug.Log("Energy Filled. Win");
        }
    }
}
