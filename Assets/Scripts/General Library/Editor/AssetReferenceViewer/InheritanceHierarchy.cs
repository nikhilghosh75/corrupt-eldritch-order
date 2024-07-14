#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Tools.AssetReferenceViewer
{
    public class InheritanceHierarchy
    {
        static readonly string[] UNITY_CLASSES = { "MonoBehaviour", "ScriptableObject", "Editor" };

        public class Node
        {
            public string guid;
            public string className;
            public Node parent;
            public List<Node> children;

            public Node(string _className)
            {
                className = _className;
                children = new List<Node>();
            }

            public Node(string _className, string _guid)
            {
                className = _className;
                guid = _guid;
                children = new List<Node>();
            }

            public int NumAncestors()
            {
                int level = 0;
                Node current = this;
                while(current != null)
                {
                    level++;
                    current = current.parent;
                }
                return level;
            }

            public List<Node> GetAncestors()
            {
                List<Node> ancestors = new List<Node>();
                Node current = parent;
                while (current != null)
                {
                    ancestors.Add(current);
                    current = current.parent;
                }

                return ancestors;
            }
        }

        public List<Node> rootNodes;
        public Dictionary<string, Node> GUIDToNode;

        public InheritanceHierarchy()
        {
            rootNodes = new List<Node>();
            GUIDToNode = new Dictionary<string, Node>();

            foreach(string unityClass in UNITY_CLASSES)
            {
                rootNodes.Add(new Node(unityClass));
            }
        }

        public void Generate(AssetReferenceDatabase database)
        {
            int numNodes = -1;
            int iterations = 0;

            while(numNodes != GUIDToNode.Count && iterations < 10)
            {
                numNodes = GUIDToNode.Count;
                
                foreach(KeyValuePair<string, AssetReferenceDatabase.ScriptInfo> info in database.GUIDToScriptInfo)
                {
                    AttemptInsertClass(database, info.Key, info.Value);
                }

                iterations++;
            }
        }

        void AttemptInsertClass(AssetReferenceDatabase database, string guid, AssetReferenceDatabase.ScriptInfo info)
        {
            if (GUIDToNode.ContainsKey(guid))
                return;

            for (int i = 0; i < UNITY_CLASSES.Length; i++)
            {
                if (info.inheritsFrom.Contains(UNITY_CLASSES[i]))
                {
                    Node node = new Node(info.className, guid);
                    rootNodes[i].children.Add(node);
                    node.parent = rootNodes[i];

                    GUIDToNode.Add(guid, node);
                }
            }

            for(int i = 0; i < info.inheritsFrom.Count; i++)
            {
                string className = info.inheritsFrom[i];
                string classGUID = "";

                if (database.ClassNameToGUID.ContainsKey(className))
                    classGUID = database.ClassNameToGUID[className];
                else
                {
                    foreach(string namespaceName in info.namespacesIncluded)
                    {
                        string fullClassName = namespaceName + "." + className;
                        if (database.ClassNameToGUID.ContainsKey(fullClassName))
                        {
                            classGUID = database.ClassNameToGUID[fullClassName];
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(classGUID))
                    continue;

                if (GUIDToNode.ContainsKey(classGUID))
                {
                    Node node = new Node(info.className, guid);
                    node.parent = GUIDToNode[classGUID];
                    GUIDToNode[classGUID].children.Add(node);

                    GUIDToNode.Add(guid, node);
                    return;
                }
            }
        }
    }
}
#endif