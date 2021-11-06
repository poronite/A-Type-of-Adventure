using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelTemplate))]
public class Level_InspectorEditor : Editor
{
    LevelTemplate level;

    //for the dictionary with the multiple choices
    int numChoices;
    SerializedProperty wordKey;
    SerializedProperty levelValue;

    private void OnEnable()
    {
        level = (LevelTemplate)target;

        wordKey = serializedObject.FindProperty("wordKey");
        levelValue = serializedObject.FindProperty("levelValue");
    }

    public override void OnInspectorGUI()
    {
        level.levelName = EditorGUILayout.TextField("Name", level.levelName);

        EditorGUILayout.Space();

        level.levelType = (LevelType)EditorGUILayout.EnumFlagsField("Level Type", level.levelType);

        EditorGUILayout.Space();

        switch (level.levelType)
        {
            case LevelType.Adventure:

                level.textToType = EditorGUILayout.TextArea(level.textToType, GUILayout.Height(100));

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

                    
                    EditorGUILayout.PropertyField(choiceWord, new GUIContent("Word " + i), true);
                    EditorGUILayout.PropertyField(choiceLevel, new GUIContent("Level " + i), true);
                    

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
                break;
            case LevelType.Combat:
                //EditorGUILayout.PropertyField
                break;
            case LevelType.Puzzle:
                break;
            default:
                break;
        }

        EditorUtility.SetDirty(level);
    }
}
