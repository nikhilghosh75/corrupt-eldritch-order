using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

/// <summary>
/// An achievement for jumping a certain number of times
/// </summary>`
[CreateAssetMenu(fileName = "Jump Achievement", menuName = "Achievements/Jump Achievement")]
public class JumpAchievement : BaseAchievement
{
    public int numJumpsRequired;

    public override void Initialize()
    {
        EventBus.Subscribe<JumpEvent>(OnJump);
    }

    void OnJump(JumpEvent e)
    {
        int currJumps = ProgressionManager.instance.jumpCount;
        if (currJumps >= numJumpsRequired)
        {
            Unlock();
        }
        else if (currJumps != 0)
        {
            IndicateAchievementProgress((int)currJumps, numJumpsRequired);
        }
    }
}
