using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    public float targetHeight;
    public float speed;

    public float forceScale;
    public float minDistanceScale;
    public float maxDistanceScale;
    public float minDistance;

    private float distanceX;
    private float multiplier;

    public int damageOnCollision;

    [Header("Shooting")]
    [SerializeField]
    public GameObject firePoint;
    public float attackTimer;
    public float fireCooldown;
    public float timeUntilDestroy;
    public float bulletSpeed;

    [SerializeField]
    GameObject target;

    public ProjectilePool projectilePool;
    private LevelEnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        projectilePool = ProjectilePool.Instance;
        enemyManager = transform.parent.GetComponent<LevelEnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyManager.playerInLevel)
        {
            //Idle behavior
            return;
        }

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        distanceX = Mathf.Abs(transform.position.x - target.transform.position.x);
        multiplier = (Vector2.Distance(transform.position, target.transform.position) < minDistance ? -1 : 1);

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, targetHeight),
            Time.deltaTime * speed * multiplier * Mathf.Clamp(distanceX / 10, 0, maxDistanceScale));

        if (transform.position.y - targetHeight < minDistance)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, forceScale * Random.Range(-1f, 1f)));
        }

        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(damageOnCollision);
        }
    }

    public void Attack()
    {
        if (attackTimer <= 0 && target.activeInHierarchy)
        {
            attackTimer = fireCooldown;

            GameObject spawnedBullet = projectilePool.SpawnProjectile<SimpleProjectile>(firePoint.transform.position, Quaternion.identity);

            if (spawnedBullet != null)
            {
                Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
                Projectile projectile = spawnedBullet.GetComponent<Projectile>();
                projectile.belongsToPlayer = false;

                projectile.timeUntilDestroy = timeUntilDestroy;

                if (target != null)
                {
                    Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;
                    rb.velocity = bulletSpeed * directionToPlayer;

                    float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                    spawnedBullet.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                }
                else
                {
                    rb.velocity = Vector2.down * bulletSpeed;
                }
            }
        }
    }

}
