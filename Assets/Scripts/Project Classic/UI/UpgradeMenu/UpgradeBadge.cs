using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBadge : MonoBehaviour
{
    public Image image;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);
        EventBus.Subscribe<UpgradePurchasedEvent>(OnUpgradePurchased);

        RecalculateUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCurrencyAdded(CurrencyAddedEvent e)
    {
        RecalculateUpgrades();
    }

    void OnUpgradePurchased(UpgradePurchasedEvent e)
    {
        RecalculateUpgrades();
    }

    void RecalculateUpgrades()
    {
        int currency = RunManager.Instance.permanentCurrency;
        int possibleUpgrades = 0;

        foreach (UpgradeTreeData upgradeTree in RunManager.Instance.upgrades)
        {
            int cost = 0;
            for(int i = upgradeTree.CurrentLevel; i < upgradeTree.upgrades.Count; i++)
            {
                UpgradeTreeData.UpgradeData upgrade = upgradeTree.upgrades[i];
                if (upgrade.cost + cost > currency)
                {
                    break;
                }
                possibleUpgrades++;
                cost += upgrade.cost;
            }
        }

        if (possibleUpgrades > 0)
        {
            image.gameObject.SetActive(true);
            text.text = possibleUpgrades.ToString();
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }

}
