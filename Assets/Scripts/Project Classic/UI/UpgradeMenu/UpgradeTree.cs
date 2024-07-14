using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTree : MonoBehaviour
{
    [SerializeField] private UpgradeTreeData upgradeTreeData;

    [SerializeField] private List<UpgradeButton> buttons;

    void Start()
    {
        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);
        EventBus.Subscribe<ItemPurchasedEvent>(OnItemPurchased);

        foreach(UpgradeButton button in buttons)
        {
            button.InitializeButton(this);
        }
    }

    public int GetUpgradeCost(int targetLevel)
    {
        return upgradeTreeData.upgrades[targetLevel - 1].cost;
    }

    public bool GetIsBought(int targetLevel)
    {
        return targetLevel <= upgradeTreeData.CurrentLevel;
    }

    public int GetNextLevel()
    {
        return upgradeTreeData.CurrentLevel;
    }

    public UpgradeTypes GetTreeType()
    {
        if (upgradeTreeData != null) 
        {
            return upgradeTreeData.treeType;
        }
        Debug.LogError("UpgradeTreeData not initialized! Returning health by default!");
        return UpgradeTypes.Health;
    }

    public void PurchaseNextUpgrade()
    {
        if (upgradeTreeData != null)
        {
            upgradeTreeData.PurchaseNextLevel();
        }
    }

    void OnCurrencyAdded(CurrencyAddedEvent e)
    {
        foreach (UpgradeButton button in buttons)
        {
            button.InitializeButton(this);
        }
    }

    void OnItemPurchased(ItemPurchasedEvent e)
    {
        if (e.currencyType == CurrencyType.Permanent)
        {
            foreach (UpgradeButton button in buttons)
            {
                button.InitializeButton(this);
            }
        }
    }
}
