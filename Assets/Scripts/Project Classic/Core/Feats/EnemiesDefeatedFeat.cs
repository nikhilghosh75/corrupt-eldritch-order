using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A feat that unlocks when a certain number of enemies are killed.
/// </summary>
[CreateAssetMenu(fileName = "New Enemies Defeated Feat", menuName = "Feats/Enemies Defeated Feat")]
public class EnemiesDefeatedFeat : BaseFeat
{
    public int enemiesKillTarget = 500;

    public override float GetProgress()
    {
        int enemiesKilled = ProgressionManager.instance.enemiesKilled;
        return (float)enemiesKilled / (float)enemiesKillTarget;
    }

    public override string GetProgressText()
    {
        int enemiesKilled = ProgressionManager.instance.enemiesKilled;
        return enemiesKilled.ToString() + "/" + enemiesKillTarget.ToString();
    }

    public override string GetProgressTextAtProgress(float progress)
    {
        int enemiesKilled = Mathf.RoundToInt(progress * enemiesKillTarget);
        return enemiesKilled.ToString() + "/" + enemiesKillTarget.ToString();
    }

    public override void Initialize()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    private void OnEnemyKilled(EnemyKilledEvent e)
    {
        int enemiesKilled = ProgressionManager.instance.enemiesKilled;
        if (enemiesKilled >= enemiesKillTarget)
        {
            Unlock();
        }
        else
        {
            IndicateFeatProgress(enemiesKilled, enemiesKillTarget);
        }
    }
}
