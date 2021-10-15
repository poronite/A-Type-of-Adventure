﻿using UnityEngine;
using UnityEngine.Events;

//This class will listen to the player's input and will do 2 different things depending if key pressed is Backspace or not.
//If Backspace pressed: will invoke DeleteInput event that sends to Typing script a request to delete last typed character (can only be used on certain parts of the game).
//Other key pressed: will use IsAllowedInput function to verify if key pressed is a allowed character and if so,
//trigger the SendInput delegate that sends a request to Typing script to type the character.

public class PlayerInput : MonoBehaviour
{
    //Delegates
    delegate void InputDelegate(string input);
    InputDelegate SendInput;

    //Unity events
    public UnityEvent DeleteInput;


    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        SendInput += gameObject.GetComponent<Typing>().TypeCharacter;
    }


    void Update()
    {
        CheckInput();
    }


    private void CheckInput()
    {        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteInput.Invoke();
        }
        else if (Input.anyKeyDown)
        {
            string key = Input.inputString;

            if (IsAllowedInput(key))
            {
                SendInput(key);
            }
        }
    }


    /// <summary>Allowed Input: letters and separators (. , ! : etc).</summary>
    private bool IsAllowedInput(string key)
    {        
        return key.Length == 1; 
    }
}