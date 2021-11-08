using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplate))]
public class Level_InspectorEditor : Editor
{
    LevelTemplate level;


    //Adventure
    SerializedProperty wordKey;
    SerializedProperty levelValue;

    //Combat
    SerializedProperty enemy;
    SerializedProperty nextLevelAfterCombat;

    //Puzzle
    SerializedProperty correctWord;
    SerializedProperty nextLevelAfterPuzzle;


    private void OnEnable()
    {
        level = (LevelTemplate)target;

        wordKey = serializedObject.FindProperty("WordKey");
        levelValue = serializedObject.FindProperty("LevelValue");

        enemy = serializedObject.FindProperty("Enemy");
        nextLevelAfterCombat = serializedObject.FindProperty("NextLevelAfterCombat");

        correctWord = serializedObject.FindProperty("CorrectWord");
        nextLevelAfterPuzzle = serializedObject.FindProperty("NextLevelAfterPuzzle");
    }

    public override void OnInspectorGUI()
    {
        level.LevelName = EditorGUILayout.TextField("Name", level.LevelName);

        EditorGUILayout.Space();

        level.LevelType = (LevelType)EditorGUILayout.EnumPopup("Level Type", level.LevelType);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        switch (level.LevelType)
        {
            case LevelType.Adventure:

                EditorGUILayout.LabelField("Adventure Variables:");

                EditorGUILayout.Space();

                level.TextToType = EditorGUILayout.TextField("Text to Type", level.TextToType);

                EditorGUILayout.Space();

                level.NumChoices = EditorGUILayout.IntField("Choices", level.NumChoices);

                //Display dictionary
                serializedObject.Update(); //Never update things other than property fields after this line

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

                    
                    EditorGUILayout.PropertyField(choiceWord, new GUIContent($"Required Word {i + 1}: "), true);
                    EditorGUILayout.PropertyField(choiceLevel, new GUIContent($"Chosen Level {i + 1}: "), true);
                    

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }

                EditorGUILayout.EndVertical();

                serializedObject.ApplyModifiedProperties();
                break;
            case LevelType.Combat:

                EditorGUILayout.LabelField("Combat Variables:");

                EditorGUILayout.Space();

                serializedObject.Update();

                EditorGUILayout.PropertyField(enemy, new GUIContent("Enemy: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(nextLevelAfterCombat, new GUIContent("Next Level: "), true);

                serializedObject.ApplyModifiedProperties();
                break;
            case LevelType.Puzzle:

                EditorGUILayout.LabelField("Puzzle Variables:");

                EditorGUILayout.Space();

                serializedObject.Update();

                EditorGUILayout.PropertyField(correctWord, new GUIContent("Correct Word: "), true);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(nextLevelAfterPuzzle, new GUIContent("Next Level: "), true);

                serializedObject.ApplyModifiedProperties();
                break;
            default:
                break;
        }

        EditorUtility.SetDirty(level);
    }
}
