using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplate))]
public class Level_InspectorEditor : Editor
{
    LevelTemplate level;


    //Adventure
    int numChoices;
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

        wordKey = serializedObject.FindProperty("wordKey");
        levelValue = serializedObject.FindProperty("levelValue");

        enemy = serializedObject.FindProperty("enemy");
        nextLevelAfterCombat = serializedObject.FindProperty("nextLevelAfterCombat");

        correctWord = serializedObject.FindProperty("correctWord");
        nextLevelAfterPuzzle = serializedObject.FindProperty("nextLevelAfterPuzzle");
    }

    public override void OnInspectorGUI()
    {
        level.levelName = EditorGUILayout.TextField("Name", level.levelName);

        EditorGUILayout.Space();

        level.levelType = (LevelType)EditorGUILayout.EnumPopup("Level Type", level.levelType);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        switch (level.levelType)
        {
            case LevelType.Adventure:

                EditorGUILayout.LabelField("Adventure Variables:");

                EditorGUILayout.Space();

                level.textToType = EditorGUILayout.TextField("Text to Type", level.textToType);

                EditorGUILayout.Space();

                //Display dictionary
                serializedObject.Update();

                numChoices = EditorGUILayout.IntField("Choices", numChoices);

                //prevent unnecessary errors
                if (numChoices < 0)
                {
                    numChoices = 0;
                }

                wordKey.arraySize = numChoices;
                levelValue.arraySize = numChoices;

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical();
                for (int i = 0; i < numChoices; i++)
                {
                    var choiceWord = wordKey.GetArrayElementAtIndex(i);
                    var choiceLevel = levelValue.GetArrayElementAtIndex(i);

                    
                    EditorGUILayout.PropertyField(choiceWord, new GUIContent($"Word {i}: "), true);
                    EditorGUILayout.PropertyField(choiceLevel, new GUIContent($"Level {i}: "), true);
                    

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
