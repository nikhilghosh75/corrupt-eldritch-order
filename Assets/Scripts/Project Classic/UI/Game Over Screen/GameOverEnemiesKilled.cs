using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverEnemiesKilled : MonoBehaviour
{
    public TMP_Text soldierText;
    public TMP_Text droneText;
    public TMP_Text infestationText;
    public TMP_Text warlockText;
    public TMP_Text debufferText;

    public int enemiesKilledSpeed = 25;

    public void ResetEnemiesKilled()
    {
        soldierText.text = "0";
        droneText.text = "0";
        infestationText.text = "0";
        warlockText.text = "0";
        debufferText.text = "0";
    }

    public IEnumerator ShowEnemiesKilled()
    {
        yield return StartCoroutine(HandleText(EnemyType.Soldier, soldierText));
        yield return StartCoroutine(HandleText(EnemyType.Drone, droneText));
        yield return StartCoroutine(HandleText(EnemyType.Debuffer, debufferText));
        yield return StartCoroutine(HandleText(EnemyType.Warlock, warlockText));
        yield return StartCoroutine(HandleText(EnemyType.Infestation, infestationText));
    }

    IEnumerator HandleText(EnemyType type, TMP_Text text)
    {
        int enemiesKilled = GetEnemiesKilled(type);
        for (int current = 0; current < enemiesKilled;)
        {
            int delta = Mathf.Max((int)(enemiesKilledSpeed * Time.deltaTime), 1);
            current += delta;
            text.text = current.ToString();

            yield return null;
        }
        text.text = enemiesKilled.ToString();
    }

    int GetEnemiesKilled(EnemyType type)
    {
        ProgressionSaveData startOfRunSaveData = RunManager.Instance.saveDataAtStartOfRun;
        int currentEnemiesKilled = ProgressionManager.instance.enemyTypesKilled[type];

        switch (type)
        {
            case EnemyType.Soldier: return currentEnemiesKilled - startOfRunSaveData.soldiersKilledCount;
            case EnemyType.Drone: return currentEnemiesKilled - startOfRunSaveData.dronesKilledCount;
            case EnemyType.Infestation: return currentEnemiesKilled - startOfRunSaveData.infestationKilledCount;
            case EnemyType.Debuffer: return currentEnemiesKilled - startOfRunSaveData.debufferKilledCount;
            case EnemyType.Warlock: return currentEnemiesKilled - startOfRunSaveData.warlockKilledCount;
        }

        return 0;
    }
}
