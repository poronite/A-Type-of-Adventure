using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    private float energyLostPerSecond;
    private float energyGainedPerWord;
    private float currentEnergy;


    void Update()
    {
        RefreshEnergyBar();
    }


    private void RefreshEnergyBar()
    {
        currentEnergy -= energyLostPerSecond * Time.deltaTime;
        //update UI bar
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
        currentEnergy += energyGainedPerWord;
        //update UI bar
    }
}
