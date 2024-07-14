using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyManager : MonoBehaviour
{
    public List<EnemySpawnPoint> spawnPoints;

    public bool spawnWhenPlayerEnters = false;
    public bool playerInLevel = false;

    List<EnemyHealth> enemiesInLevel = new List<EnemyHealth>();
    LevelGenerator levelGen;

    bool playerGotHit = false, spawned = false, updated = false, missed = false;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthUpdated);
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyDeath);
        levelGen = GameObject.Find("Level Generator")
            .GetComponent<LevelGenerator>();

        ChangeSpawns();
    }

    public void GenerateAllEnemies()
    {
        foreach (EnemySpawnPoint e in spawnPoints)
        {
            if (e != null && e.enemy != null)
            {
                GenerateEnemy(e.transform.position, e.enemy);
            }
        }
        spawned = true;
    }

    public void GenerateEnemy(Vector2 position, EnemyData enemy)
    {
        // Spawn enemy
        GameObject e = Instantiate(enemy.gameObject, position, Quaternion.identity, transform);
        enemiesInLevel.Add(e.GetComponent<EnemyHealth>());
    }

    public void ChangeSpawns()
    {
        bool spawnOnStart = !spawnWhenPlayerEnters;

        foreach (Transform child in transform)
        {
            EnemySpawnPoint spawnPoint = child.GetComponent<EnemySpawnPoint>();
            if (spawnPoint)
            {
                LevelConfig.EnemyWeight[] list = levelGen.enemyData[spawnPoint.enemyType];
                spawnPoint.enemyList = new EnemyData[list.Length];
                spawnPoint.probability = new int[list.Length];

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i].enemy.GetComponent<EnemyData>())
                    {
                        spawnPoint.enemyList[i] = list[i].enemy.GetComponent<EnemyData>();
                        spawnPoint.probability[i] = list[i].probability;
                    }
                }

                child.GetComponent<EnemySpawnPoint>().GetEnemy();
            }
        }

        if (spawnOnStart || missed)
        {
            GenerateAllEnemies();
        }
        updated = true;
    }

    public bool PlayerInLevel() => playerInLevel;

    void OnHealthUpdated(UpdateHealthEvent e)
    {
        if (e.healthDelta < 0)
        {
            playerGotHit = true;
        }
    }

    void OnEnemyDeath(EnemyKilledEvent e)
    {
        if (enemiesInLevel.Contains(e.enemy))
        {
            enemiesInLevel.Remove(e.enemy);

            if (enemiesInLevel.Count == 0)
            {
                EventBus.Publish(new RoomClearEvent(!playerGotHit));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInLevel = true;
            Debug.Log("New room entered. Theme: " + gameObject.GetComponent<LevelChunk>().theme);
            if (gameObject.GetComponent<LevelChunk>().type == LevelType.End)
                EventBus.Publish(new RoomEnterEvent(gameObject.GetComponent<LevelChunk>().theme, true));
            else
                EventBus.Publish(new RoomEnterEvent(gameObject.GetComponent<LevelChunk>().theme, false));


            if (spawnWhenPlayerEnters && !spawned && !updated)
            {
                missed = true;
            }
            else if (spawnWhenPlayerEnters && !spawned && updated)
            {
                GenerateAllEnemies();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInLevel = false;
        }
    }
}