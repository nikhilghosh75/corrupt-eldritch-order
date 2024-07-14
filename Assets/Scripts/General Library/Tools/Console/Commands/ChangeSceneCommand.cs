using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Changes the scene to a given scene
 * Written by Nikhil Ghosh '24, Brandon Schulz '22, William Bostick '20
 */

namespace WSoft.Tools.Console
{
    public class ChangeSceneCommand : ConsoleCommand
    {
        public ChangeSceneCommand()
        {
            commandWord = "changescene";
        }

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                Debug.LogError("Invalid Args");
                return false;
            }

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = scenePath.Substring(scenePath.LastIndexOf('/') + 1);
                string sceneNameNoExt = sceneName.Substring(0, sceneName.Length - 6); // Remove .unity extension
                if (sceneNameNoExt == args[0])
                {
                    SceneManager.LoadScene(i);
                    return true;
                }
            }

            Debug.LogError("Could not find scene with name " + args[0]);
            return false;
        }

        public override List<string> GetValidArgs() 
        {
            List<string> args = new List<string>();
            for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                args.Add(SceneManager.GetSceneByBuildIndex(i).name);
            }

            return args;
        }
    }
}