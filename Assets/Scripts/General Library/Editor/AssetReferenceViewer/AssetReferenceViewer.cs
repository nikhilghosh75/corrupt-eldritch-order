#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace WSoft.Tools.AssetReferenceViewer
{
    public class AssetReferenceViewer : EditorWindow
    {
        [System.Flags]
        public enum Filter
        {
            None = 0,
            Image = 1,
            Prefab = 2,
            Scene = 4,
            Script = 8,
            ScriptableObject = 16,
            Animation = 32,
            All = ~0
        }

        public enum Tab
        {
            Main,
            AssetsReferencedBy,
            ScriptInfo,
            InheritanceHierarchy
        }

        public struct TabInfo
        {
            public Tab type;
            public string assetViewingGUID;

            public TabInfo(Tab _type, string _assetViewingGUID)
            {
                type = _type;
                assetViewingGUID = _assetViewingGUID;
            }
        }

        AssetReferenceDatabase database = new AssetReferenceDatabase();

        Dictionary<string, bool> AssetPathToOpened = new Dictionary<string, bool>();
        EditorGUITools.DynamicScrollData mainViewScrollData = new EditorGUITools.DynamicScrollData();
        bool referencesTabOpened = false;

        Filter currentFilter = Filter.All;
        Tab currentTab = Tab.Main;
        bool includeThirdParty = true;
        Stack<TabInfo> tabs = new Stack<TabInfo>();
        string currentViewGUID;
        
        Vector2 assetsReferencedByScrollbar;

        [MenuItem("WolverineSoft/Asset Reference Viewer")]
        public static void OpenWindow()
        {
            AssetReferenceViewer viewer = GetWindow<AssetReferenceViewer>(false, "Asset Reference Viewer", true);
            viewer.StartReload();
        }

        void OnGUI()
        {
            switch (currentTab)
            {
                case Tab.Main:                  RenderMainView(); break;
                case Tab.AssetsReferencedBy:    RenderAssetsReferencedByTab(); break;
                case Tab.ScriptInfo:            RenderScriptInfoTab(); break;
                case Tab.InheritanceHierarchy:  RenderInheritanceHierarchyTab(); break;
            }
        }

        void StartReload()
        {
            database.Clear();
            database.Initialize();

            AssetPathToOpened.Clear();
            foreach (string assetPath in database.AssetPaths)
            {
                AssetPathToOpened.Add(assetPath, false);
            }
        }

        void Back()
        {
            if(tabs.Count == 0)
            {
                currentTab = Tab.Main;
            }
            else
            {
                currentTab = tabs.Peek().type;
                currentViewGUID = tabs.Peek().assetViewingGUID;
            }
        }

        void AddTab(Tab tab, string viewGuid)
        {
            tabs.Push(new TabInfo(currentTab, currentViewGUID));

            currentTab = tab;
            currentViewGUID = viewGuid;
        }

        void RenderFilters()
        {
            Filter newFilter = (Filter)EditorGUILayout.EnumFlagsField("Filter", currentFilter);
            if((int)newFilter != (int)currentFilter)
            {
                // Add stuff here
            }
            currentFilter = newFilter;

            includeThirdParty = EditorGUILayout.Toggle("Include Third Party Assets", includeThirdParty);
        }

        void RenderMainView()
        {
            RenderFilters();

            EditorGUITools.DynamicScroll<string>(this, mainViewScrollData,
                database.GUIDS, HeightOfResult, RenderResult);
        }

        int HeightOfResult(string guid, int index)
        {
            string assetPath = database.GUIDToAssetPath[guid];

            if (!CanRenderAssetPath(assetPath))
                return 0;

            if (!AssetPathToOpened.ContainsKey(assetPath))
                return (int)EditorGUIUtility.singleLineHeight;

            if (AssetPathToOpened[assetPath])
                return (int)EditorGUIUtility.singleLineHeight * 3;

            return (int)EditorGUIUtility.singleLineHeight;
        }

        void RenderResult(string guid, int index)
        {
            string assetPath = database.GUIDToAssetPath[guid];

            if (!CanRenderAssetPath(assetPath))
                return;

            if (!AssetPathToOpened.ContainsKey(assetPath))
                AssetPathToOpened.Add(assetPath, false);

            bool opened = AssetPathToOpened[assetPath];
            opened = EditorGUILayout.BeginFoldoutHeaderGroup(opened, assetPath);
            AssetPathToOpened[assetPath] = opened;

            if (opened)
            {
                EditorGUILayout.LabelField("GUID: " + guid);
                AssetsReferencedByField(guid);

                if (assetPath.EndsWith(".prefab"))
                    RenderPrefab(assetPath, guid);
                else if (assetPath.EndsWith(".asset"))
                    RenderSO(assetPath, guid);
                else if (assetPath.EndsWith(".unity"))
                    RenderScene(assetPath, guid);
                else if (assetPath.EndsWith(".cs"))
                    RenderScript(assetPath, guid);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        bool CanRenderAssetPath(string assetPath)
        {
            if ((currentFilter & GetFilterForPath(assetPath)) == 0)
                return false;

            if (IsThirdParty(assetPath) && !includeThirdParty)
                return false;

            return true;
        }

        void RenderPrefab(string assetPath, string guid)
        {
            referencesTabOpened = EditorGUILayout.BeginFoldoutHeaderGroup(referencesTabOpened, "References in Prefab");
            if (referencesTabOpened)
            {
                AssetReferenceDatabase.PrefabInfo info = database.GUIDToPrefabInfo[guid];
                foreach (string guidReferenced in info.GUIDsReferenced)
                {
                    EditorGUILayout.LabelField(database.GUIDToAssetPath[guidReferenced]);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void RenderSO(string assetPath, string guid)
        {
            if (!database.GUIDToSOInfo.ContainsKey(guid))
                return;

            string baseScriptGUID = database.GUIDToSOInfo[guid].baseScriptGUID;
            string baseScriptName = GUIDToScriptName(baseScriptGUID);

            EditorGUILayout.LabelField("Base Script: " + baseScriptName);
        }

        void RenderScene(string assetPath, string guid)
        {
            referencesTabOpened = EditorGUILayout.BeginFoldoutHeaderGroup(referencesTabOpened, "References in Scene");
            if (referencesTabOpened)
            {
                AssetReferenceDatabase.SceneInfo info = database.GUIDToSceneInfo[guid];
                foreach (string guidReferenced in info.GUIDsReferenced)
                {
                    EditorGUILayout.LabelField(database.GUIDToAssetPath[guidReferenced]);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void RenderScript(string assetPath, string guid)
        {
            if (!database.GUIDToScriptInfo.ContainsKey(guid))
                return;

            if(GUILayout.Button("View Script Info"))
            {
                AddTab(Tab.ScriptInfo, guid);
            }
        }

        void RenderAssetsReferencedByTab()
        {
            if (GUILayout.Button("Back"))
                Back();

            EditorGUILayout.LabelField(database.GUIDToAssetPath[currentViewGUID]);
            EditorGUILayout.LabelField("GUID: " + currentViewGUID);

            HashSet<string> referencedByGUIDs = database.GUIDToAssetsReferencedBy[currentViewGUID];

            EditorGUILayout.Space();

            assetsReferencedByScrollbar = EditorGUILayout.BeginScrollView(assetsReferencedByScrollbar);
            foreach (string guid in referencedByGUIDs)
            {
                EditorGUILayout.LabelField(database.GUIDToAssetPath[guid]);
            }
            EditorGUILayout.EndScrollView();
        }

        void RenderScriptInfoTab()
        {
            if (GUILayout.Button("Back"))
            {
                Back();
                return;
            }

            AssetReferenceDatabase.ScriptInfo info = database.GUIDToScriptInfo[currentViewGUID];
            EditorGUILayout.LabelField("Assembly: " + GetAssemblyDisplayName(info.owningAssemblyGUID, currentViewGUID));

            EditorGUILayout.LabelField("Inherits From: ");
            for (int i = 0; i < info.inheritsFrom.Count; i++)
                EditorGUILayout.LabelField("    " + info.inheritsFrom[i]);

            if (info.inheritsFrom.Count > 0)
            {
                if (GUILayout.Button("View Inheritance Hierarchy"))
                {
                    AddTab(Tab.InheritanceHierarchy, currentViewGUID);
                }
            }
        }

        void RenderInheritanceHierarchyTab()
        {
            if (GUILayout.Button("Back"))
            {
                Back();
                return;
            }

            InheritanceHierarchy.Node node = database.inheritanceHierarchy.GUIDToNode[currentViewGUID];
            if (node == null)
            {
                EditorGUILayout.LabelField("Cannot find node in inheritance hierarchy");
                return;
            }

            List<InheritanceHierarchy.Node> ancestors = node.GetAncestors();
            
            for(int i = 0; i < ancestors.Count; i++)
            {
                if (database.GUIDS.Contains(ancestors[i].guid))
                {
                    string ancestorFilepath = database.GUIDToAssetPath[ancestors[i].guid];
                    string ancestorFilename = ancestorFilepath.Substring(ancestorFilepath.LastIndexOf('\\') + 1, ancestorFilepath.Length - ancestorFilepath.LastIndexOf('\\') - 4);
                    string ancestorName = new string('\t', i) + ancestorFilename;

                    if (GUILayout.Button(ancestorName, ClickableTextStyle()))
                    {
                        AddTab(Tab.ScriptInfo, ancestors[i].guid);
                    }
                }
                else
                {
                    GUILayout.Label(node.className);
                }
            }

            string className = new string('\t', ancestors.Count) + node.className;
            if (GUILayout.Button(className, ClickableTextStyle()))
            {
                AddTab(Tab.ScriptInfo, node.guid);
            }

            for(int i = 0; i < node.children.Count; i++)
            {
                string guid = node.children[i].guid;

                string childFilepath = database.GUIDToAssetPath[guid];
                string childFilename = childFilepath.Substring(childFilepath.LastIndexOf('\\') + 1, childFilepath.Length - childFilepath.LastIndexOf('\\') - 4);
                string childName = new string('\t', ancestors.Count + 1) + childFilename;

                if (GUILayout.Button(childName, ClickableTextStyle()))
                {
                    AddTab(Tab.ScriptInfo, guid);
                }
            }
        }

        Filter GetFilterForPath(string assetPath)
        {
            if (assetPath.EndsWith(".png") || assetPath.EndsWith(".jpg")) return Filter.Image;
            if (assetPath.EndsWith(".prefab")) return Filter.Prefab;
            if (assetPath.EndsWith(".unity")) return Filter.Scene;
            if (assetPath.EndsWith(".cs")) return Filter.Script;
            if (assetPath.EndsWith(".asset")) return Filter.ScriptableObject;
            if (assetPath.EndsWith(".anim")) return Filter.Animation;
            if (assetPath.EndsWith(".controller")) return Filter.Animation;

            return Filter.All;
        }

        string GUIDToScriptName(string guid)
        {
            string filepath = database.GUIDToAssetPath[guid];
            int slashIndex = filepath.LastIndexOf("/");
            return filepath.Substring(slashIndex + 1, filepath.Length - slashIndex - 4);
        }

        void AssetsReferencedByField(string guid)
        {
            int numReferencedBy = database.GUIDToAssetsReferencedBy[guid].Count;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Referenced By: " + numReferencedBy + " assets");

            if (GUILayout.Button("Show"))
            {
                AddTab(Tab.AssetsReferencedBy, guid);
            }
            EditorGUILayout.EndHorizontal();
        }

        bool IsThirdParty(string filepath)
        {
            if (filepath.Contains("ThirdParty") || filepath.Contains("Third Party") || filepath.Contains("thirdparty"))
                return true;

            return false;
        }

        string GetAssemblyDisplayName(string assemblyGuid, string scriptGuid)
        {
            if(string.IsNullOrEmpty(assemblyGuid))
            {
                string scriptAssetPath = database.GUIDToAssetPath[scriptGuid];

                if (scriptAssetPath.Contains("Editor/"))
                    return "Assembly-CSharp Editor";
                return "Assembly-CSharp";
            }

            string assemblyAssetPath = database.GUIDToAssetPath[assemblyGuid];
            return assemblyAssetPath.Substring(assemblyAssetPath.LastIndexOf('/') + 1);
        }

        GUIStyle ClickableTextStyle(bool bold = false)
        {
            GUIStyle s = bold ? EditorStyles.boldLabel : new GUIStyle();
            RectOffset b = s.border;
            b.left = 0;
            b.top = 0;
            b.right = 0;
            b.bottom = 0;
            s.border = b;
            s.normal.textColor = Color.white;
            return s;
        }
    }

}

#endif