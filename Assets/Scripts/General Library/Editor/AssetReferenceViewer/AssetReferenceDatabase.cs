#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace WSoft.Tools.AssetReferenceViewer
{
    public class AssetReferenceDatabase
    {
        // Contains details about a given prefab
        public class PrefabInfo
        {
            // A list of GUIDs contained in the prefab
            public List<string> GUIDsReferenced = new List<string>();
        }

        // Contains details about scriptable objects
        public class SOInfo
        {
            public string baseScriptGUID = "";
        }

        // Contains details about a given scene
        public class SceneInfo
        {
            // A list of GUIDs contained in the scene
            public List<string> GUIDsReferenced = new List<string>();
        }

        // Contains details about a given script file
        public class ScriptInfo
        {
            // A list of classes/interfaces that the core script of the file inherits from
            public List<string> inheritsFrom = new List<string>();
            // The assembly that the script file belongs to
            public string owningAssemblyGUID = "";
            // The namespaces included in the script file (i.e. the "using" declarations)
            public List<string> namespacesIncluded;
            // The namespace that wraps the file (i.e. the "namespace" declaration)
            public string wrappingNamespace;
            // The name of the class
            public string className;
        }

        public enum AnimationAssetType
        {
            Animation,
            Controller
        }

        public class AnimationInfo
        {
            public AnimationAssetType assetType;
        }

        // A list of all asset paths (for easy iteration)
        public List<string> AssetPaths = new List<string>();
        // A list of all GUIDs (for easy iteration)
        public List<string> GUIDS = new List<string>();

        public Dictionary<string, string> AssetPathToGUID = new Dictionary<string, string>();
        public Dictionary<string, string> GUIDToAssetPath = new Dictionary<string, string>();

        // The following dicts all use GUIDS where there are strings
        public Dictionary<string, PrefabInfo> GUIDToPrefabInfo = new Dictionary<string, PrefabInfo>();
        public Dictionary<string, SOInfo> GUIDToSOInfo = new Dictionary<string, SOInfo>();
        public Dictionary<string, SceneInfo> GUIDToSceneInfo = new Dictionary<string, SceneInfo>();
        public Dictionary<string, ScriptInfo> GUIDToScriptInfo = new Dictionary<string, ScriptInfo>();
        public Dictionary<string, AnimationInfo> GUIDToAnimationInfo = new Dictionary<string, AnimationInfo>();
        public Dictionary<string, HashSet<string>> GUIDToAssetsReferencedBy = new Dictionary<string, HashSet<string>>();

        // The following dictionaries are for getting and searching scripts
        public Dictionary<string, string> ClassNameToGUID = new Dictionary<string, string>();
        public InheritanceHierarchy inheritanceHierarchy;

        public bool initialized { get; private set; }

        static readonly string[] IGNORED_FILEPATHS = { "Assets/WWise", "Assets/Wwise", "Assets/StreamingAssets", "Assets/TextMesh Pro" };

        public AssetReferenceDatabase()
        {
            initialized = false;
        }

        public void Initialize()
        {
            initialized = false;

            InitializeGUIDS();
            InitializePrefabInfos();
            InitializeSOInfos();
            InitializeSceneInfos();
            InitializeScriptInfos();

            initialized = true;
        }

        public void Clear()
        {
            AssetPaths.Clear();
            AssetPathToGUID.Clear();
            GUIDS.Clear();
            GUIDToAssetPath.Clear();
            GUIDToPrefabInfo.Clear();
            GUIDToAssetsReferencedBy.Clear();
        }

        /// <summary>
        /// Gets the filepath relative to the Assets folder
        /// </summary>
        string GetRelativeFilepath(string filepath)
        {
            string relativeFilepath = filepath.Substring(Application.dataPath.Length - 6);
            relativeFilepath = relativeFilepath.Replace('\\', '/');
            return relativeFilepath;
        }

        /// <summary>
        /// Should this file be viewed in the Asset Reference Viewer?
        /// </summary>
        bool IsViewableFile(string filepath)
        {
            foreach (string ignoredFilepath in IGNORED_FILEPATHS)
            {
                if (filepath.Contains(ignoredFilepath))
                    return false;
            }

            return !filepath.EndsWith(".meta") && !filepath.EndsWith(".gitkeep");
        }

        /// <summary>
        /// Populates the dictionaries AssetPathToGUID and GUIDToAssetPath
        /// Should be run before other asset reference database functions
        /// </summary>
        void InitializeGUIDS()
        {
            foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.*", SearchOption.AllDirectories))
            {
                string relativeFilepath = GetRelativeFilepath(filepath);
                if (!IsViewableFile(relativeFilepath))
                    continue;

                string GUID = AssetDatabase.AssetPathToGUID(relativeFilepath);
                if (GUIDToAssetPath.ContainsKey(GUID))
                    continue;

                AssetPathToGUID.Add(relativeFilepath, GUID);
                GUIDToAssetPath.Add(GUID, relativeFilepath);

                GUIDToAssetsReferencedBy.Add(GUID, new HashSet<string>());

                AssetPaths.Add(relativeFilepath);
                GUIDS.Add(GUID);
            }
        }

        void InitializePrefabInfos()
        {
            foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories))
            {
                PrefabInfo prefabInfo = GetPrefabInfo(filepath);
                string relativeFilepath = GetRelativeFilepath(filepath);
                GUIDToPrefabInfo.Add(AssetPathToGUID[relativeFilepath], prefabInfo);
            }
        }

        void InitializeSOInfos()
        {
            foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.asset", SearchOption.AllDirectories))
            {
                string relativeFilepath = GetRelativeFilepath(filepath);
                if (!IsViewableFile(relativeFilepath))
                    continue;

                SOInfo soInfo = GetSOInfo(filepath);
                GUIDToSOInfo.Add(AssetPathToGUID[relativeFilepath], soInfo);
            }
        }

        void InitializeSceneInfos()
        {
            foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories))
            {
                string relativeFilepath = GetRelativeFilepath(filepath);
                if (!IsViewableFile(relativeFilepath))
                    continue;

                SceneInfo sceneInfo = GetSceneInfo(filepath);
                GUIDToSceneInfo.Add(AssetPathToGUID[relativeFilepath], sceneInfo);
            }
        }

        void InitializeScriptInfos()
        {
            foreach (string filepath in Directory.EnumerateFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories))
            {
                string relativeFilepath = GetRelativeFilepath(filepath);
                if (!IsViewableFile(relativeFilepath))
                    continue;

                ScriptInfo scriptInfo = GetScriptInfo(filepath);
                GUIDToScriptInfo.Add(AssetPathToGUID[relativeFilepath], scriptInfo);
            }

            inheritanceHierarchy = new InheritanceHierarchy();
            inheritanceHierarchy.Generate(this);
        }

        PrefabInfo GetPrefabInfo(string filepath)
        {
            PrefabInfo info = new PrefabInfo();

            string relativeFilepath = GetRelativeFilepath(filepath);
            string prefabGUID = AssetPathToGUID[relativeFilepath];

            foreach (string line in File.ReadLines(filepath))
            {
                if (line.Contains("{fileID: "))
                {
                    string guid = ParseGUID(line);

                    if (!string.IsNullOrEmpty(guid))
                    {
                        if (!GUIDToAssetsReferencedBy.ContainsKey(guid))
                            GUIDToAssetsReferencedBy.Add(guid, new HashSet<string>());

                        info.GUIDsReferenced.Add(guid);
                        GUIDToAssetsReferencedBy[guid].Add(prefabGUID);
                    }
                }
            }

            return info;
        }

        SOInfo GetSOInfo(string filepath)
        {
            SOInfo info = new SOInfo();

            string relativeFilepath = GetRelativeFilepath(filepath);
            string soGUID = AssetPathToGUID[relativeFilepath];

            foreach (string line in File.ReadLines(filepath))
            {
                if (line.Contains("{fileID: "))
                {
                    string guid = ParseGUID(line);

                    if (!string.IsNullOrEmpty(guid))
                    {
                        if (!GUIDToAssetsReferencedBy.ContainsKey(guid))
                            GUIDToAssetsReferencedBy.Add(guid, new HashSet<string>());

                        GUIDToAssetsReferencedBy[guid].Add(soGUID);

                        if (line.Contains("m_Script: "))
                        {
                            info.baseScriptGUID = guid;
                        }
                    }
                }
            }

            return info;
        }

        SceneInfo GetSceneInfo(string filepath)
        {
            SceneInfo info = new SceneInfo();

            string relativeFilepath = GetRelativeFilepath(filepath);
            string prefabGUID = AssetPathToGUID[relativeFilepath];

            foreach (string line in File.ReadLines(filepath))
            {
                if (line.Contains("{fileID: "))
                {
                    string guid = ParseGUID(line);

                    if (!string.IsNullOrEmpty(guid))
                    {
                        if (!GUIDToAssetsReferencedBy.ContainsKey(guid))
                            GUIDToAssetsReferencedBy.Add(guid, new HashSet<string>());

                        info.GUIDsReferenced.Add(guid);
                        GUIDToAssetsReferencedBy[guid].Add(prefabGUID);
                    }
                }
            }

            return info;
        }

        ScriptInfo GetScriptInfo(string filepath)
        {
            ScriptInfo info = new ScriptInfo();

            info.owningAssemblyGUID = GetAssemblyGUID(filepath);
            info.inheritsFrom = FindInheritanceInfo(filepath);
            info.namespacesIncluded = FindAvailibleNamespaces(filepath);
            info.className = filepath.Substring(filepath.LastIndexOf('\\') + 1, filepath.Length - filepath.LastIndexOf('\\') - 4);

            AddAvailibleClassNames(filepath, ref info.wrappingNamespace);

            return info;
        }

        string ParseGUID(string line)
        {
            int fileIDIndex = line.IndexOf("{fileID: ");
            if (line.Length < fileIDIndex + 16)
                return "";

            string fileIDStr = line.Substring(fileIDIndex + 8, 8);
            int fileID;
            if (int.TryParse(fileIDStr, out fileID))
            {
                int guidIndex = line.IndexOf("guid: ");
                if (guidIndex != -1 && line.Length >= guidIndex + 38)
                {
                    string guid = line.Substring(guidIndex + 6, 32);
                    return guid.Contains(" ") ? " " : guid;
                }
            }

            return "";
        }

        string GetAssemblyGUID(string filepath)
        {
            // Iterate through all the folders, and see if there is an assembly
            string relativeFilepath = GetRelativeFilepath(filepath);
            int numFolders = relativeFilepath.Count(x => x == '/');
            string currentFolderPath = filepath;
            for (int i = numFolders - 1; i >= 0; i--)
            {
                currentFolderPath = currentFolderPath.Substring(0, currentFolderPath.LastIndexOf('/'));
                foreach (string assemblyFilepath in Directory.EnumerateFiles(currentFolderPath, "*"))
                {
                    if (assemblyFilepath.Contains(".meta") || !assemblyFilepath.Contains(".asmdef"))
                        continue;

                    // Is a legit assembly
                    string relativeAssemblyFilepath = GetRelativeFilepath(assemblyFilepath);
                    return AssetPathToGUID[relativeAssemblyFilepath];
                }
            }

            return "";
        }

        List<string> FindInheritanceInfo(string filepath)
        {
            // Figure out Inheritance
            string scriptFileText = File.ReadAllText(filepath);
            string filename = filepath.Substring(filepath.LastIndexOf('\\') + 1, filepath.Length - filepath.LastIndexOf('\\') - 4);
            
            // Get the first instance of the word "class"
            int classIdentifierIndex = scriptFileText.IndexOf("class ");

            if (filepath.EndsWith("LevelEvents.cs"))
            {
                Debug.Log("Hello");
            }

            // Figure out where the class of the file starts
            while (classIdentifierIndex != -1)
            {
                // Gets the index of the end of the word after "class"
                // Assumes there isn't a class name less than 3 letters
                int spaceIndex = scriptFileText.IndexOf(' ', classIdentifierIndex + 8);
                if (spaceIndex != -1 && spaceIndex < classIdentifierIndex)
                {
                    classIdentifierIndex = scriptFileText.IndexOf("class ", classIdentifierIndex + 6);
                    continue;
                }

                // If a class has no inheritance (or is empty), the class name string ends at the bracket
                // and a space may not be found
                int bracketIndex = scriptFileText.IndexOf('{', classIdentifierIndex + 8);
                if (bracketIndex < spaceIndex || spaceIndex == -1)
                {
                    spaceIndex = bracketIndex;
                }

                // Figure out the name of the class
                string className = scriptFileText.Substring(classIdentifierIndex + 6, spaceIndex - classIdentifierIndex - 6);
                className = className.Trim();
                if (className == filename)
                {
                    break;
                }

                classIdentifierIndex = scriptFileText.IndexOf("class ", classIdentifierIndex + 6);
            }

            if (classIdentifierIndex != -1)
            {
                int colonIndex = scriptFileText.IndexOf(':', classIdentifierIndex);
                if (colonIndex != -1)
                {
                    int curlyBracketIndex = scriptFileText.IndexOf('{', colonIndex);

                    if (curlyBracketIndex != -1)
                    {
                        string inheritanceString = scriptFileText.Substring(colonIndex + 1, curlyBracketIndex - colonIndex - 2).Trim();
                        return new List<string>(inheritanceString.Split(','));
                    }
                }
            }
            return new List<string>();
        }

        void AddAvailibleClassNames(string filepath, ref string namespaceName)
        {
            string relativeFilepath = GetRelativeFilepath(filepath);
            string GUID = AssetPathToGUID[relativeFilepath];
            string scriptFileText = File.ReadAllText(filepath);

            // Get Namespace involved in file
            // We assume for simplicity that no file has more than 1 namespace declaration
            namespaceName = "";
            int namespaceIndex = scriptFileText.IndexOf("namespace ");
            int spaceIndex = -1;
            if (namespaceIndex != -1)
            {
                spaceIndex = scriptFileText.IndexOf(' ', namespaceIndex + 12);
                if (spaceIndex != -1)
                {
                    namespaceName = scriptFileText.Substring(namespaceIndex + 10, spaceIndex - namespaceIndex - 10);
                    namespaceName = namespaceName.Trim();
                }
            }

            // SECOND PASS - Get each class in the file
            // Get the first instance of the word "class"
            int classIdentifierIndex = scriptFileText.IndexOf("class ");

            // Figure out where the class of the file starts
            while (classIdentifierIndex != -1)
            {
                // Gets the index of the end of the word after "class"
                // Assumes there isn't a class name less than 3 letters
                spaceIndex = scriptFileText.IndexOf(' ', classIdentifierIndex + 8);
                if (spaceIndex == -1 || spaceIndex <= classIdentifierIndex)
                {
                    classIdentifierIndex = scriptFileText.IndexOf("class ", classIdentifierIndex + 6);
                    continue;
                }

                // Figure out the name of the class
                string className = scriptFileText.Substring(classIdentifierIndex + 6, spaceIndex - classIdentifierIndex - 6);
                className = className.Trim();

                // As a sanity check, ensure a class has a '{' or ':' within 10 characters of its declaration
                int bracketIndex = scriptFileText.IndexOf('{', spaceIndex);
                int colonIndex = scriptFileText.IndexOf(':', spaceIndex);
                if (bracketIndex < spaceIndex + 10 || colonIndex < spaceIndex + 10)
                {
                    // This is a valid class and we should add it to our known class names
                    string fullClassName = "";
                    if (!string.IsNullOrEmpty(namespaceName))
                    {
                        fullClassName += RemoveNewlines(namespaceName) + ".";
                    }
                    fullClassName += RemoveNewlines(className);
                    if (!ClassNameToGUID.ContainsKey(fullClassName))
                    {
                        ClassNameToGUID.Add(fullClassName, GUID);
                    }

                }

                classIdentifierIndex = scriptFileText.IndexOf("class ", classIdentifierIndex + 6);
            }
        }

        List<string> FindAvailibleNamespaces(string filepath)
        {
            List<string> namespaces = new List<string>();

            string scriptFileText = File.ReadAllText(filepath);
            string[] lines = scriptFileText.Split('\n');

            foreach(string line in lines)
            {
                string trimmedLine = line.Replace("\r", "").Trim();
                if (trimmedLine.StartsWith("using ") && trimmedLine.EndsWith(";"))
                {
                    namespaces.Add(trimmedLine.Substring(6, trimmedLine.Length - 7).Trim());
                }
            }

            return namespaces;
        }

        string RemoveNewlines(string str)
        {
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            str = str.Replace("\t", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            return str;
        }
    }
}

#endif