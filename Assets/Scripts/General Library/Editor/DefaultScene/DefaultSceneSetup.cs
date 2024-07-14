/*
 * Defines what gameobjects go into a new scene when created.
 * @Zena Abulhab
 */

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSoft.Tools.DefaultScene
{
    [UnityEditor.InitializeOnLoad]
    public static class DefaultSceneSetup
    {
        // When Unity editor opens, this constructor sets up the event callback
        static DefaultSceneSetup()
        {
            EditorSceneManager.newSceneCreated += SetUpScene;
        }

        private static void SetUpScene(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            // Instantiate the Developer Console
            PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Developer Console.prefab", typeof(GameObject)));
            PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Progression Manager.prefab", typeof(GameObject)));
        }
    }
}
#endif