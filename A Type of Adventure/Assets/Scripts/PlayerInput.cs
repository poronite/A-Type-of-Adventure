using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This class will listen to the player's input and will do 2 different things depending if key pressed is Backspace or not.
//If Backspace pressed: will invoke DeleteLetterInput event that sends to (class name) a request to delete last typed letter (can only be used on certain parts of the game)
//Other key pressed: will use IsLetter function to verify if key pressed is a letter and if so, trigger the SendLetterDelegate that sends a request to (class name) to type the letter.

public class PlayerInput : MonoBehaviour
{
    //Delegates
    delegate void InputDelegate(string input);
    InputDelegate SendLetterInput;

    //Unity events
    public UnityEvent DeleteLetterInput;


    //Player game object will never be disabled so OnEnable is enough
    private void OnEnable()
    {
        SendLetterInput += gameObject.GetComponent<Typing>().TypeLetter;
    }


    void Update()
    {
        CheckInput();
    }


    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DeleteLetterInput.Invoke();
        }
        else if (Input.anyKeyDown)
        {
            string key = Input.inputString;

            if (IsLetter(key))
            {
                SendLetterInput(key);
            }
        }
    }


    private bool IsLetter(string key)
    {
        //key.Length == 1 is so that it doesn't spew errors when pressing shift or similar keys
        return (key.Length == 1 && char.IsLetter(key[0])); 
    }
}
