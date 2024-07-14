using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] float fireRate;
    float lastFireTime = -10000f;

    // Store projectile data
    ProjectileData projectileData;

    // Store the turrets goal position
    Vector3 goalPosition;

    Rigidbody2D rigid;
    ProjectilePool pool;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        pool = ProjectilePool.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop if near goal position
        if (Vector3.Distance(rigid.position, goalPosition) < 0.5f)
        {
            rigid.velocity = Vector3.zero;
        }
        else
        {
            return;
        }

        // Fire projectiles
        if (Time.time - lastFireTime > fireRate)
        {
            // Set last fire time
            lastFireTime = Time.time;

            // Spawn cross of projectiles
            pool.SpawnProjectile(projectileData, transform.position, Vector3.up, Quaternion.identity);
            pool.SpawnProjectile(projectileData, transform.position, Vector3.left, Quaternion.identity);
            pool.SpawnProjectile(projectileData, transform.position, Vector3.right, Quaternion.identity);
            pool.SpawnProjectile(projectileData, transform.position, Vector3.down, Quaternion.identity);
        }
    }

    // Initialize the projectiles info
    public void InitializeTurret(ProjectileData data, Vector3 goal, float speed)
    {
        projectileData = data;
        goalPosition = goal;

        GetComponent<Rigidbody2D>().velocity = (goalPosition - transform.position).normalized * speed;
    }
}
