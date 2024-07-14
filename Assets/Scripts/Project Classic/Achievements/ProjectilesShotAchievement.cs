using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

[CreateAssetMenu(fileName="New ProjectilesShotAchievement", menuName="Achievements/ProjectilesShotAchievement")]
public class ProjectilesShotAchievement : BaseAchievement
{
    public int num_to_fire = 1000;
    public int progress_interval = 100;

    private int projectiles_shot = 0;

    public override void Initialize()
    {
        EventBus.Subscribe<ProjectileShotEvent>(OnProjectileShot);
    }

    void OnProjectileShot(ProjectileShotEvent e)
    {
        projectiles_shot++;
        if(projectiles_shot >= num_to_fire)
        {
            Unlock();
        }
        else if(projectiles_shot % progress_interval == 0 && projectiles_shot != 0)
        {
            IndicateAchievementProgress(projectiles_shot, num_to_fire);
        }
    }
}
