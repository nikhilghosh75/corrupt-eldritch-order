using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Beat Game Feat", menuName = "Feats/Beat Game Feat")]
public class BeatGameFeat : BaseFeat
{

    public override float GetProgress()
    {
        if (ProgressionManager.instance.beatenGame)
            return 1f;
        return 0f;
    }

    public override string GetProgressText()
    {
        if (ProgressionManager.instance.beatenGame)
            return "YES";
        return "NO";
    }

    public override string GetProgressTextAtProgress(float progress)
    {
        if (Mathf.Approximately(progress, 1f))
            return "YES";
        return "NO";
    }

    public override void Initialize()
    {
        EventBus.Subscribe<GameCompleteEvent>(OnGameWon);
    }

    void OnGameWon(GameCompleteEvent e)
    {
        Unlock();
    }
}
