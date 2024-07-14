using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Defeated Feat", menuName = "Feats/Boss Defeated Feat")]
public class BossDefeatedFeat : BaseFeat
{
    public int numDefeats;

    public override float GetProgress()
    {
        int bossesDefeated = ProgressionManager.instance.bossDefeatedCount;
        return (float)bossesDefeated / (float)numDefeats;
    }

    public override string GetProgressText()
    {
        int bossesDefeated = ProgressionManager.instance.bossDefeatedCount;
        return bossesDefeated + "/" + numDefeats;
    }

    public override string GetProgressTextAtProgress(float progress)
    {
        int bossesDefeated = Mathf.RoundToInt(progress * numDefeats);
        return bossesDefeated + "/" + numDefeats;
    }

    public override void Initialize()
    {
        EventBus.Subscribe<BossDefeatedEvent>(OnBossDefeated);
    }

    void OnBossDefeated(BossDefeatedEvent e)
    {
        int bossesDefeated = ProgressionManager.instance.bossDefeatedCount;
        if (bossesDefeated >= numDefeats)
        {
            Unlock();
        }
    }
}
