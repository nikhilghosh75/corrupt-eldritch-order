using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Audio;

public abstract class EnemyFireProjectileAction : EnemyAction
{
    // Stores ID for this attack, only used on boss
    public int id = 0;

    public AudioEvent roar;

    // Reference to projectile prefab
    [SerializeField] protected GameObject projectilePrefab;
    public ProjectileData projectileData;

    // Location to spawn projectile from
    [SerializeField] protected Transform firePosition;

    // Reference to projectile pool
    [SerializeField] protected ProjectilePool projectilePool;

    // Cooldown for an attack
    [SerializeField] public float cooldownTime = 1.0f;

    private void Start()
    {
        projectilePool = ProjectilePool.Instance;
    }

    protected void FireBulletAtTarget(Vector2 target, float angle_offset, int bulletSpeed)
    {
        //Play audio
        roar.PlayAudio(gameObject);

        //TODO: give more params to account for speed, time to destroy, etc.
        if (!projectileData)
        {
            GameObject spawnedBullet = projectilePool.SpawnProjectile<SimpleProjectile>(firePosition.transform.position, Quaternion.identity);

            if (spawnedBullet != null)
            {
                Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
                Projectile projectile = spawnedBullet.GetComponent<Projectile>();
                projectile.belongsToPlayer = false;

                //TODO: make the projectile manager handle these variables when initializing a projectile
                //giving defualt value of 1
                projectile.timeUntilDestroy = 1.0f;
                Vector2 fireAgentPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 directionToTarget = (target - fireAgentPos).normalized;
                rb.velocity = bulletSpeed * directionToTarget;

                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                angle += angle_offset; // Add your angle offset here
                spawnedBullet.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

                Vector2 newDirection = Quaternion.Euler(0, 0, angle_offset) * directionToTarget; // Rotate directionToPlayer by angleOffset
                rb.velocity = bulletSpeed * newDirection;
            }
        }
        else
        {
            // Find the direction vector to the target
            Vector2 fireAgentPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 directionToTarget = (target - fireAgentPos).normalized;

            // Get the angle for this direction vector
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            angle += angle_offset; // Add your angle offset here
            Quaternion spawnRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            projectilePool.SpawnProjectile(projectileData, firePosition.position, directionToTarget, spawnRotation);
        }
    }
}
