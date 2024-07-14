using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

[CreateAssetMenu(fileName = "New Enemies Defeated Consecutively Achievement", menuName = "Achievements/Enemies Defeated Consecutively Achievement")]
public class EnemiesDefeatedConsecutivelyAchievement : BaseAchievement
{

    public int numToKill = 10;
    public int indicateProgressInterval = 1;
    private int currentEnemiesKilled = 0;

    public override void Initialize()
    {
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthUpdated);
    }

    void OnEnemyKilled(EnemyKilledEvent e)
    {
        currentEnemiesKilled++;
        if (currentEnemiesKilled >= numToKill)
        {
            Unlock();
        }
        else if (currentEnemiesKilled % indicateProgressInterval == 0 && currentEnemiesKilled != 0)
        {
            IndicateAchievementProgress(currentEnemiesKilled, numToKill);
        }
    }

    void OnHealthUpdated(UpdateHealthEvent e)
    {
        if (e.healthDelta < 0)
        {
            currentEnemiesKilled = 0;
        }
    }
}
