#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetupWizard : EditorWindow
{
    List<SetupWizardStep> steps = new List<SetupWizardStep>();

    [MenuItem("WolverineSoft/Setup Wizard")]
    public static void OpenSetupWizard()
    {
        SetupWizard wizard = GetWindow<SetupWizard>(false, 
            "WolverineSoft General Library Setup Wizard", true);

        wizard.SetupSteps();
    }

    public void OnGUI()
    {
        GUILayout.Label("Welcome to the WolverineSoft General Library Setup Wizard");
        GUILayout.Label("It allows a user to setup the prefabs that they will need for games");
        GUILayout.Label("This is only really necessary if you want to use the actual prefabs in the game");

        GUILayout.Space(20f);

        GUILayout.Label("Status:");

        GUIStyle redStyle = new GUIStyle();
        redStyle.normal.textColor = Color.red;
        GUIStyle greenStyle = new GUIStyle();
        greenStyle.normal.textColor = Color.green;

        int numStepsDone = 0;

        foreach (SetupWizardStep step in steps)
        {
            if(step.HasBeenDone())
            {
                GUILayout.Label(step.Name + " (DONE)", greenStyle);
                numStepsDone++;
            }
            else
            {
                GUILayout.Label(step.Name, redStyle);
            }
        }

        GUILayout.Label(numStepsDone + "/" + steps.Count + " tasks done");
        
        if(GUILayout.Button("Start"))
        {
            for(int i = 0; i < steps.Count; i++)
            {
                if(!steps[i].HasBeenDone())
                    steps[i].Do();
            }
        }

        if (GUILayout.Button("Reload"))
            SetupSteps();
    }

    void SetupSteps()
    {
        steps.Clear();

        // Note that we add the steps manually here because we want them to be in a
        // specific order
        steps.Add(new CreateDefaultFolders());
        steps.Add(new CreateDevConsolePrefab());
        steps.Add(new CreateProgressionManagerPrefab());
    }
}
#endif