#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CodebaseAnalytics : EditorWindow
{
    public struct CodebaseDetails
    {
        public int files;
        public int linesOfCode;
        public int classes;
    }

    static CodebaseDetails globalDetails;
    static Dictionary<string, CodebaseDetails> folderPathToDetails;

    Vector2 scrollbar;
    Dictionary<string, bool> folderPathOpened = new Dictionary<string, bool>();

    public static void GenerateCodebaseDetails()
    {
        globalDetails.files = 0;
        globalDetails.linesOfCode = 0;
        globalDetails.classes = 0;

        folderPathToDetails = new Dictionary<string, CodebaseDetails>();

        foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories))
        {
            string relativeFilepath = GetRelativeFilepath(filepath);
            string folderPath = relativeFilepath.Substring(0, relativeFilepath.LastIndexOf('/'));

            globalDetails.files++;
            int linesOfCode = 0;
            int classes = 0;

            bool inComment = false;
            foreach (string line in File.ReadLines(filepath))
            {
                linesOfCode++;

                if (line.Contains("/*") && !line.Contains("*/"))
                    inComment = true;
                else if (line.Contains("*/"))
                    inComment = false;

                if (line.Contains("public class") && !inComment)
                    classes++;
            }

            globalDetails.linesOfCode += linesOfCode;
            globalDetails.classes += classes;

            string currentFolderPath = folderPath;
            while(currentFolderPath.Length > 0)
            {
                if (folderPathToDetails.ContainsKey(currentFolderPath))
                {
                    CodebaseDetails details = folderPathToDetails[currentFolderPath];
                    details.files += 1;
                    details.linesOfCode += linesOfCode;
                    details.classes += classes;
                    folderPathToDetails[currentFolderPath] = details;
                }
                else
                {
                    CodebaseDetails details = new CodebaseDetails();
                    details.files = 1;
                    details.linesOfCode = linesOfCode;
                    details.classes = classes;
                    folderPathToDetails.Add(currentFolderPath, details);
                }

                if (currentFolderPath.LastIndexOf('/') < 0)
                    currentFolderPath = "";
                else
                    currentFolderPath = currentFolderPath.Substring(0, currentFolderPath.LastIndexOf('/'));
            }
        }
    }

    [MenuItem("Project Multiply/Codebase Analytics")]
    public static void Open()
    {
        GetWindow<CodebaseAnalytics>("Codebase Analytics");
        GenerateCodebaseDetails();
    }

    private void OnGUI()
    {
        GUILayout.Label("Global Files: " + globalDetails.files);
        GUILayout.Label("Global Lines of Code: " + globalDetails.linesOfCode);
        GUILayout.Label("Global Classes: " + globalDetails.classes);

        List<string> folderPaths = folderPathToDetails.Keys.ToList();
        folderPaths.Sort();

        scrollbar = EditorGUILayout.BeginScrollView(scrollbar);
        foreach (string folderPath in folderPaths)
        {
            if (!folderPathOpened.ContainsKey(folderPath))
                folderPathOpened.Add(folderPath, false);

            bool opened = folderPathOpened[folderPath];
            opened = EditorGUILayout.BeginFoldoutHeaderGroup(opened, folderPath);
            folderPathOpened[folderPath] = opened;

            if(opened)
            {
                GUILayout.Label("Files: " + folderPathToDetails[folderPath].files);
                GUILayout.Label("Lines of Code: " + folderPathToDetails[folderPath].linesOfCode);
                GUILayout.Label("Classes: " + folderPathToDetails[folderPath].classes);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        EditorGUILayout.EndScrollView();
    }

    static string GetRelativeFilepath(string filepath)
    {
        string relativeFilepath = filepath.Substring(Application.dataPath.Length - 6);
        relativeFilepath = relativeFilepath.Replace('\\', '/');
        return relativeFilepath;
    }
}
#endif