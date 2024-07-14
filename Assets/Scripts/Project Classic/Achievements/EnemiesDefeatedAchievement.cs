using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

[CreateAssetMenu(fileName="New Enemies Defeated Achievement", menuName="Achievements/Enemies Defeated Achievement")]
public class EnemiesDefeatedAchievement : BaseAchievement
{
    public int numToKill = 100;
    public int indicateProgressInterval = 10;

    public override void Initialize()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    void OnEnemyKilled(EnemyKilledEvent e)
    {
        int currentEnemiesKilled = ProgressionManager.instance.enemiesKilled;
        if (currentEnemiesKilled >= numToKill)
        {
            Unlock();
        }
        else if(currentEnemiesKilled % indicateProgressInterval == 0 && currentEnemiesKilled != 0)
        {
            IndicateAchievementProgress(currentEnemiesKilled, numToKill);
        }
    }
}
