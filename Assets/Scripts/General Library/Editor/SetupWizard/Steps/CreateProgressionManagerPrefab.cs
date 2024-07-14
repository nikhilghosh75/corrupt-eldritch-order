#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WSoft.Achievements;

public class CreateProgressionManagerPrefab : SetupWizardStep
{
    public CreateProgressionManagerPrefab()
    {
        name = "Create Progression Manager";
    }

    public override void Do()
    {
        GameObject prefabBase = new GameObject("Progression Manager");

        prefabBase.AddComponent<AchievementsManager>();

        PrefabUtility.SaveAsPrefabAsset(prefabBase, "Assets/Prefabs/Progression Manager.prefab");
    }

    public override bool HasBeenDone()
    {
        return AssetDatabase.FindAssets("Progression Manager", new string[] { "Assets/Prefabs" }).Length >= 1;
    }
}
#endif