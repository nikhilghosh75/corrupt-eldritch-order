using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyClass
{
    Ground,
    Flying,
    Ranged,
    Boss
}

public class EnemySpawnPoint : MonoBehaviour
{
    public EnemyClass enemyType;
    [HideInInspector]
    public EnemyData enemy;
    [HideInInspector]
    public EnemyData[] enemyList;
    [HideInInspector]
    public int[] probability;

    private void Start()
    {
        if (!transform.parent)
        {
            return;
        }
        if (!transform.parent.GetComponent<LevelEnemyManager>())
        {
            LevelEnemyManager lem = transform.parent.GetComponentInChildren<LevelEnemyManager>();
            gameObject.transform.parent = lem.transform;
        }
    }

    public void GetEnemy()
    {
        int result;
        int total = 0;
        int weightTotal = 0;

        System.Random Random = new System.Random();
        foreach (int weight in probability)
        {
            weightTotal += weight;
        }
        int randVal = Random.Next(weightTotal);
        for (result = 0; result < probability.Length; result++)
        {
            total += probability[result];
            if (total > randVal) break;
        }
        enemy = enemyList[result];
    }
}
