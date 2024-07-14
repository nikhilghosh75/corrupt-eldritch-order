using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : ScriptableObject
{
    UpgradeTree[] upgradeTrees;

    private void Start()
    {
        upgradeTrees = FindObjectsByType<UpgradeTree>(FindObjectsSortMode.None);
    }
}
