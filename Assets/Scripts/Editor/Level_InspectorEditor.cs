﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplate))]
public class Level_InspectorEditor : Editor
{
    LevelTemplate level;

    //Variables
    SerializedProperty levelName;
    SerializedProperty levelType;

    //Adventure
    SerializedProperty textToType;
    //choices
    SerializedProperty possibleChoices;
    SerializedProperty possibleOutcomes;

    SerializedProperty numChoices;
    SerializedProperty wordKey;
    SerializedProperty levelValue;
    //graphics events
    SerializedProperty numEncounters;
    SerializedProperty encounterWordKey;
    SerializedProperty encounterValue;

    //Combat
    SerializedProperty enemy;
    SerializedProperty nextLevelAfterCombat;

    //Puzzle
    SerializedProperty correctWord;
    SerializedProperty questionBoard;
    SerializedProperty nextLevelAfterPuzzle;


    private void OnEnable()
    {
        level = (LevelTemplate)target;

        levelName = serializedObject.FindProperty("LevelName");
        levelType = serializedObject.FindProperty("LevelType");

        textToType = serializedObject.FindProperty("TextToType");

        possibleChoices = serializedObject.FindProperty("PossibleChoices");
        possibleOutcomes = serializedObject.FindProperty("PossibleOutcomes");

        numChoices = serializedObject.FindProperty("NumChoices");
        wordKey = serializedObject.FindProperty("WordKey");
        levelValue = serializedObject.FindProperty("LevelValue");

        numEncounters = serializedObject.FindProperty("NumEncounters");
        encounterWordKey = serializedObject.FindProperty("EncounterWordKey");
        encounterValue = serializedObject.FindProperty("EncounterValue");

        enemy = serializedObject.FindProperty("Enemy");
        nextLevelAfterCombat = serializedObject.FindProperty("NextLevelAfterCombat");

        correctWord = serializedObject.FindProperty("CorrectWord");
        questionBoard = serializedObject.FindProperty("QuestionBoard");
        nextLevelAfterPuzzle = serializedObject.FindProperty("NextLevelAfterPuzzle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(levelName, new GUIContent("Name: "), true);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(levelType, new GUIContent("Level Type: "), true);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //put this here because it was giving errors when changing level types
        string[] words = { "Empty" };

        if (level.TextToType != string.Empty)
        {
            words = level.TextToType.Split(' ');
        }

        switch (level.LevelType)
        {
            case LevelType.Adventure:

                EditorGUILayout.LabelField("Adventure Variables:");

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(textToType, new GUIContent("Text to Type: "), true);

                EditorGUILayout.Space();


                //Display dictionaries

                //choices
                EditorGUILayout.PropertyField(numChoices, new GUIContent("Next possible levels: "), true);

                //prevent unnecessary errors
                if (level.NumChoices < 0)
                {
                    level.NumChoices = 0;
                }

                wordKey.arraySize = level.NumChoices;
                levelValue.arraySize = level.NumChoices;

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                for (int i = 0; i < level.NumChoices; i++)
                {
                    SerializedProperty choiceWord = wordKey.GetArrayElementAtIndex(i);
                    SerializedProperty choiceLevel = levelValue.GetArrayElementAtIndex(i);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(choiceWord, new GUIContent($"Required Word {i + 1} (Index): "), true);

                    if (choiceWord.intValue - 1 > 0)
                    {
                        if (words[choiceWord.intValue - 1] == "*")
                        {
                            possibleChoices.arraySize = level.NumChoices;
                            possibleOutcomes.arraySize = level.NumChoices;

                            SerializedProperty currentPossibleChoice = possibleChoices.GetArrayElementAtIndex(i);
                            SerializedProperty currentPossibleOutcome = possibleOutcomes.GetArrayElementAtIndex(i);

                            EditorGUILayout.PropertyField(currentPossibleChoice, new GUIContent($"Word: "), true);

                            EditorGUILayout.PropertyField(currentPossibleOutcome, new GUIContent($"Level: "), true);
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Word: {words[choiceWord.intValue - 1]}");
                            EditorGUILayout.PropertyField(choiceLevel, new GUIContent($"Level {i + 1}: "), true);
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"Word: {words[0]}");
                    }

                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                //events
                EditorGUILayout.PropertyField(numEncounters, new GUIContent("Events: "), true);

                //prevent unnecessary errors
                if (level.NumEncounters < 0)
                {
                    level.NumEncounters = 0;
                }

                encounterWordKey.arraySize = level.NumEncounters;
                encounterValue.arraySize = level.NumEncounters;

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                for (int i = 0; i < level.NumEncounters; i++)
                {
                    SerializedProperty eventWord = encounterWordKey.GetArrayElementAtIndex(i);
                    SerializedProperty eventLevel = encounterValue.GetArrayElementAtIndex(i);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(eventWord, new GUIContent($"Required Word {i + 1}: "), true);

                    if (eventWord.intValue - 1 > 0)
                    {
                        EditorGUILayout.LabelField($"Word: {words[eventWord.intValue - 1]}");
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"Word: {words[0]}");
                    }

                    EditorGUILayout.PropertyField(eventLevel, new GUIContent($"Event {i + 1}: "), true);

                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                break;
            case LevelType.Combat:

                EditorGUILayout.LabelField("Combat Variables:");

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(enemy, new GUIContent("Enemy: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(nextLevelAfterCombat, new GUIContent("Next Level: "), true);
                break;
            case LevelType.Puzzle:

                EditorGUILayout.LabelField("Puzzle Variables:");

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(correctWord, new GUIContent("Correct Word: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(questionBoard, new GUIContent("Question Board: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(nextLevelAfterPuzzle, new GUIContent("Next Level: "), true);
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(level);
    }
}
