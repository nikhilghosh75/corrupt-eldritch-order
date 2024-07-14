using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade Tree", menuName = "Project Classic/Upgrade/Upgrade Tree")]
public class UpgradeTreeData : ScriptableObject
{
    [System.Serializable]
    public class UpgradeData
    {
        public int cost;
    }

    public UpgradeTypes treeType;
    public List<UpgradeData> upgrades;

    int currentLevel = 0; // 0 means no upgrades in the tree have been bought
    public int CurrentLevel { get { return currentLevel; } }

    public void PurchaseNextLevel()
    {
        currentLevel++;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }
}