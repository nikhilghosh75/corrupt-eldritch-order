using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Core;
using WSoft.Tools.Console;

public class ResetPermanentUpgradesCommand : ConsoleCommand
{
    public ResetPermanentUpgradesCommand()
    {
        commandWord = "resetpermanentupgrades";
    }

    public override List<string> GetValidArgs()
    {
        return new List<string>();
    }

    public override bool Process(string[] args)
    {
        foreach (UpgradeTreeData upgradeTree in RunManager.Instance.upgrades)
        {
            upgradeTree.SetLevel(0);
        }

        SaveManager.Delete("PermanentData");

        return true;
    }
}
