#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CreateDefaultFolders : SetupWizardStep
{
    List<string> folderList = new List<string>
    {
        "Art", "Prefabs", "Scenes", "Scriptable Objects"
    };

    public CreateDefaultFolders()
    {
        name = "Create Default Folders";
    }

    public override void Do()
    {
        // Create all the folders in the folder list
        foreach (string folder in folderList)
        {
            if (AssetDatabase.IsValidFolder("Assets/" + folder))
                continue;

            AssetDatabase.CreateFolder("Assets", folder);
            File.WriteAllText("Assets/" + folder + "/.gitkeep", "");
        }
    }

    public override bool HasBeenDone()
    {
        // If even a single folder hasn't been created, we need to do this task
        foreach (string folder in folderList)
        {
            if (!AssetDatabase.IsValidFolder("Assets/" + folder))
                return false;
        }

        return true;
    }
}
#endif