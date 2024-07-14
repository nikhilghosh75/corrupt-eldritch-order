using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyPrefab();
    }

    public void SpawnEnemyPrefab()
    {
        GameObject spawnedObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        EnemyHealth enemyHealth = spawnedObject.GetComponent<EnemyHealth>();
        enemyHealth.events.OnDeath.AddListener(OnEnemyDeath);
    }

    void OnEnemyDeath()
    {
        Invoke("SpawnEnemyPrefab", delay);
    }
}
