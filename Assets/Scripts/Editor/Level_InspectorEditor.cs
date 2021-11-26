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
    SerializedProperty numChoices;
    SerializedProperty wordKey;
    SerializedProperty levelValue;
    //graphics events
    SerializedProperty numEvents;
    SerializedProperty eventWordKey;
    SerializedProperty eventValue;

    //Combat
    SerializedProperty enemy;
    SerializedProperty nextLevelAfterCombat;

    //Puzzle
    SerializedProperty correctWord;
    SerializedProperty nextLevelAfterPuzzle;


    private void OnEnable()
    {
        level = (LevelTemplate)target;

        levelName = serializedObject.FindProperty("LevelName");
        levelType = serializedObject.FindProperty("LevelType");

        textToType = serializedObject.FindProperty("TextToType");

        numChoices = serializedObject.FindProperty("NumChoices");
        wordKey = serializedObject.FindProperty("WordKey");
        levelValue = serializedObject.FindProperty("LevelValue");

        numEvents = serializedObject.FindProperty("NumEvents");
        eventWordKey = serializedObject.FindProperty("EventWordKey");
        eventValue = serializedObject.FindProperty("EventValue");

        enemy = serializedObject.FindProperty("Enemy");
        nextLevelAfterCombat = serializedObject.FindProperty("NextLevelAfterCombat");

        correctWord = serializedObject.FindProperty("CorrectWord");
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

        switch (level.LevelType)
        {
            case LevelType.Adventure:

                EditorGUILayout.LabelField("Adventure Variables:");

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(textToType, new GUIContent("Text to Type: "), true);

                EditorGUILayout.Space();


                //Display dictionaries

                //choices
                EditorGUILayout.PropertyField(numChoices, new GUIContent("Choices: "), true);

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
                    var choiceWord = wordKey.GetArrayElementAtIndex(i);
                    var choiceLevel = levelValue.GetArrayElementAtIndex(i);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(choiceWord, new GUIContent($"Required Word {i + 1}: "), true);
                    EditorGUILayout.PropertyField(choiceLevel, new GUIContent($"Chosen Level {i + 1}: "), true);
                    
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                //events
                EditorGUILayout.PropertyField(numEvents, new GUIContent("Events: "), true);

                //prevent unnecessary errors
                if (level.NumEvents < 0)
                {
                    level.NumEvents = 0;
                }

                eventWordKey.arraySize = level.NumEvents;
                eventValue.arraySize = level.NumEvents;

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                for (int i = 0; i < level.NumEvents; i++)
                {
                    var eventWord = eventWordKey.GetArrayElementAtIndex(i);
                    var eventLevel = eventValue.GetArrayElementAtIndex(i);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(eventWord, new GUIContent($"Required Word {i + 1}: "), true);
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

                EditorGUILayout.PropertyField(nextLevelAfterPuzzle, new GUIContent("Next Level: "), true);
                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(level);
    }
}
