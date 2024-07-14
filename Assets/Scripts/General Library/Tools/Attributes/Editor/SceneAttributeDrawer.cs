#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/*
 * The property drawer for the Scene attribute
 * Displays the names of all the scenes in the build settings
 * Written by Nikhil Ghosh '24
 */

namespace WSoft
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] sceneNames = GetAllSceneNames();
            int index = Array.IndexOf(sceneNames, property.stringValue);

            int newIndex = EditorGUILayout.Popup(property.displayName, index, sceneNames);
            if (newIndex >= 0)
            {
                property.stringValue = sceneNames[newIndex];
            }
        }

        static string[] GetAllSceneNames()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] sceneNames = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlashIndex = scenePath.LastIndexOf("/");
                int lastPeriodIndex = scenePath.LastIndexOf(".");
                string sceneName = scenePath.Substring(lastSlashIndex + 1, lastPeriodIndex - lastSlashIndex - 1);

                sceneNames[i] = sceneName;
            }

            return sceneNames;
        }
    }
}
#endif