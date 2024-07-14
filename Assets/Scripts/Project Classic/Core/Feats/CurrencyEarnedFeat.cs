using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency Earned Feat", menuName = "Feats/Currency Earned Feat")]
public class CurrencyEarnedFeat : BaseFeat
{
    public int currencyThreshold;
    public override float GetProgress()
    {
        return (float)ProgressionManager.instance.permanentCurrencyAcquired / (float)currencyThreshold;
    }

    public override string GetProgressText()
    {
        return ProgressionManager.instance.permanentCurrencyAcquired + "/" + currencyThreshold;
    }

    public override string GetProgressTextAtProgress(float progress)
    {
        int permanentCurrencyAcquired = Mathf.RoundToInt(progress * currencyThreshold);
        return permanentCurrencyAcquired + "/" + currencyThreshold;
    }

    public override void Initialize()
    {
        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);
    }

    void OnCurrencyAdded(CurrencyAddedEvent e)
    {
        if (ProgressionManager.instance.permanentCurrencyAcquired >= currencyThreshold)
        {
            Unlock();
        }
    }
}
