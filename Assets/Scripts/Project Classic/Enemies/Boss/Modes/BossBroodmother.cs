using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBroodmother : BossMode
{
    private int attack_counter = 0;

    public override int ModeID { get { return 5; } }

    public GameObject[] EnemyTypes;
    public int NumEnemiesToSpawn = 3;
    public float SpawnRadius = 15.0f;

    private PlatformEffector2D[] platforms;

    public override void OnBehaviorStart()
    {
        platforms = FindObjectsOfType<PlatformEffector2D>();
    }

    public override void OnPostAttack()
    {
        attack_counter++;

        if(attack_counter == 3)
        {
            attack_counter = 0;
        }
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < NumEnemiesToSpawn; i++)
        {
            int enemyType = Random.Range(0, EnemyTypes.Length);
            GameObject new_enemy = Instantiate(EnemyTypes[enemyType]);
            if(new_enemy.GetComponent<EnemyData>().type == EnemyType.Soldier)
            {
                new_enemy.transform.position = FindGroundSpawnPos();
            }
            else if(new_enemy.GetComponent<EnemyData>().type == EnemyType.Drone)
            {
                new_enemy.transform.position = FindAerialSpawnPos();
            }
        }
        attack_counter = 0;
    }

    Vector2 FindGroundSpawnPos()
    {
        Vector2 current_pos = transform.position;
        List<PlatformEffector2D> platforms_in_radius = new List<PlatformEffector2D>();

        foreach (PlatformEffector2D platform in platforms)
        {
            Vector2 vec_to_platform = (Vector2)platform.transform.position - current_pos;
            if (vec_to_platform.magnitude < SpawnRadius)
            {
                platforms_in_radius.Add(platform);
            }
        }

        if (platforms_in_radius.Count > 0)
        {
            PlatformEffector2D random_platform = platforms_in_radius[Random.Range(0, platforms_in_radius.Count)];
            return FindSpotOnPlatform(random_platform);
        }
        else
        {
            //if no valid platform, spawn enemy at boss pos
            return current_pos;
        }
    }

    Vector2 FindAerialSpawnPos()
    {
        Vector2 random_dir = Random.insideUnitCircle.normalized;
        float random_dist = Random.Range(0, SpawnRadius);
        Vector2 spawn_pos = (Vector2)transform.position + random_dir * random_dist;
        return spawn_pos;
    }

    private Vector2 FindSpotOnPlatform(PlatformEffector2D platform)
    {
        Vector2 result = new();

        result.y =
            platform.gameObject.transform.position.y +
            (platform.GetComponent<BoxCollider2D>().bounds.extents.y / 2);

        //choose a random x coordinate on the platform to jump to
        float xmin =
            platform.gameObject.transform.position.x -
            (platform.GetComponent<BoxCollider2D>().bounds.extents.x / 2);
        float xmax =
            platform.gameObject.transform.position.x +
            (platform.GetComponent<BoxCollider2D>().bounds.extents.x / 2);
        result.x = Random.Range(xmin, xmax);

        return result;
    }
}
