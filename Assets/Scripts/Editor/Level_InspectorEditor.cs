using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplate))]
public class Level_InspectorEditor : Editor
{
    int branches = 2;

    LevelTemplate level;

    //Variables
    SerializedProperty levelName;
    SerializedProperty levelType;
    SerializedProperty fieldType;

    //Adventure
    SerializedProperty textToType;
    SerializedProperty nextLevelAfterAdventure;
    //branching
    SerializedProperty possibleChoices;
    SerializedProperty possibleOutcomes;
    //encounters
    SerializedProperty numEncounters;
    SerializedProperty encounterWordKey;
    SerializedProperty encounterValue;

    //Combat
    SerializedProperty enemy;
    //if common enemy
    SerializedProperty nextLevelAfterCombat;
    //if Boss
    SerializedProperty nextLevelAfterKillingBoss;
    SerializedProperty nextLevelAfterSparingBoss;

    //Puzzle
    SerializedProperty correctWord;
    SerializedProperty questionBoard;
    SerializedProperty nextLevelAfterPuzzle;

    //Challenge
    SerializedProperty startingEnergy;
    SerializedProperty energyLostPerSecond;
    SerializedProperty energyGainedPerWord;
    SerializedProperty challengeBoard;
    SerializedProperty challengeBoardFill;
    SerializedProperty wordListSize;
    SerializedProperty wordList;
    SerializedProperty nextLevelAfterChallenge;



    private void OnEnable()
    {
        level = (LevelTemplate)target;

        //common to all levels
        levelName = serializedObject.FindProperty("LevelName");
        levelType = serializedObject.FindProperty("LevelType");
        fieldType = serializedObject.FindProperty("FieldType");

        //adventure
        textToType = serializedObject.FindProperty("TextToType");
        nextLevelAfterAdventure = serializedObject.FindProperty("NextLevelAfterAdventure");
        //branching
        possibleChoices = serializedObject.FindProperty("PossibleChoices");
        possibleOutcomes = serializedObject.FindProperty("PossibleOutcomes");
        //encounters
        numEncounters = serializedObject.FindProperty("NumEncounters");
        encounterWordKey = serializedObject.FindProperty("EncounterWordKey");
        encounterValue = serializedObject.FindProperty("EncounterValue");

        //combat
        enemy = serializedObject.FindProperty("Enemy");
        //common enemy
        nextLevelAfterCombat = serializedObject.FindProperty("NextLevelAfterCombat");
        //Boss
        nextLevelAfterKillingBoss = serializedObject.FindProperty("NextLevelAfterKillingBoss");
        nextLevelAfterSparingBoss = serializedObject.FindProperty("NextLevelAfterSparingBoss");

        //puzzle
        correctWord = serializedObject.FindProperty("CorrectWord");
        questionBoard = serializedObject.FindProperty("QuestionBoard");
        nextLevelAfterPuzzle = serializedObject.FindProperty("NextLevelAfterPuzzle");

        //challenge
        startingEnergy = serializedObject.FindProperty("StartingEnergy");
        energyLostPerSecond = serializedObject.FindProperty("EnergyLostPerSecond");
        energyGainedPerWord = serializedObject.FindProperty("EnergyGainedPerWord");
        challengeBoard = serializedObject.FindProperty("ChallengeBoard");
        challengeBoardFill = serializedObject.FindProperty("ChallengeBoardFill");
        wordListSize = serializedObject.FindProperty("WordListSize");
        wordList = serializedObject.FindProperty("WordList");
        nextLevelAfterChallenge = serializedObject.FindProperty("NextLevelAfterChallenge");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(levelName, new GUIContent("Name: "), true);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(levelType, new GUIContent("Level Type: "), true);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(fieldType, new GUIContent("Field Type: "), true);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //put this here because it was giving errors when changing level types
        string[] words = { "Empty" };


        //to deal with the errors once and for all
        if (level.TextToType != null)
        {
            if (level.TextToType.Length > 0)
            {
                words = level.TextToType.Split(' ');
            }
        }

        switch (level.LevelType)
        {
            case LevelType.Adventure:

                EditorGUILayout.LabelField("Adventure Variables:");

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(textToType, new GUIContent("Text to Type: "), true);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (!(words[words.Length - 1] == "*"))
                {
                    level.HasBranching = false;
                    EditorGUILayout.PropertyField(nextLevelAfterAdventure, new GUIContent("Next Level: "), true);
                }
                else if(words[words.Length - 1] == "*")
                {
                    level.HasBranching = true;
                    EditorGUILayout.LabelField("Branches:");

                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.Space();

                    for (int i = 0; i < branches; i++)
                    {
                        possibleChoices.arraySize = branches;
                        possibleOutcomes.arraySize = branches;

                        SerializedProperty currentPossibleChoice = possibleChoices.GetArrayElementAtIndex(i);
                        SerializedProperty currentPossibleOutcome = possibleOutcomes.GetArrayElementAtIndex(i);

                        EditorGUILayout.LabelField($"Branch {i + 1}:");

                        EditorGUILayout.PropertyField(currentPossibleChoice, new GUIContent($"   Word: "), true);
                        EditorGUILayout.PropertyField(currentPossibleOutcome, new GUIContent($"   Level: "), true);

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.EndVertical(); 
                }

                EditorGUILayout.Space();

                //encounters
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

                if (level.Enemy != null)
                {
                    //common enemy
                    if (!level.Enemy.IsBoss)
                    {
                        EditorGUILayout.PropertyField(nextLevelAfterCombat, new GUIContent("Next Level: "), true);
                    }
                    else //Boss
                    {
                        EditorGUILayout.LabelField("Next Level:");

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(nextLevelAfterKillingBoss, new GUIContent("  Kill: "), true);

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(nextLevelAfterSparingBoss, new GUIContent("  Spare: "), true);
                        EditorGUILayout.EndVertical();
                    }
                }
                
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
            case LevelType.Challenge:

                EditorGUILayout.LabelField("Challenge Variables:");

                EditorGUILayout.Space();

                EditorGUILayout.Slider(startingEnergy, 0f, 100f, new GUIContent("Starting Energy: "));
                //EditorGUILayout.PropertyField(startingEnergy, new GUIContent("Starting Energy: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(energyLostPerSecond, new GUIContent("Energy Lost Per Second: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(energyGainedPerWord, new GUIContent("Energy Gained Per Word: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(challengeBoard, new GUIContent("Challenge Board: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(challengeBoardFill, new GUIContent("Challenge Board Fill: "), true);

                EditorGUILayout.Space();

                //words that can appear on this challenge
                EditorGUILayout.PropertyField(wordListSize, new GUIContent("Word List Size: "), true);
                
                //prevent unnecessary errors
                if (level.WordListSize < 0)
                {
                    level.WordListSize = 0;
                }
                
                wordList.arraySize = level.WordListSize;
                
                EditorGUILayout.Space();
                
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < level.WordListSize; i++)
                {
                    SerializedProperty word = wordList.GetArrayElementAtIndex(i);
                
                    EditorGUILayout.Space();
                
                    EditorGUILayout.PropertyField(word, new GUIContent($"Word {i + 1}: "), true);                               
                
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(nextLevelAfterChallenge, new GUIContent("Next Level: "), true);

                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(level);
    }
}
