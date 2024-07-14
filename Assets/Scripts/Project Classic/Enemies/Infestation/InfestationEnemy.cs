using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Infestation Enemy by Kennedy McCarthy

public class InfestationEnemy : EnemyBehavior
{
    private float diffX;

    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject[] enemies;

    public AK.Wwise.Event shootSFX;

    public ProjectilePool projectilePool;
    public GameObject firePoint;

    public float timer = 1.55f;
    public float cooldown = 2;
    public float attackDelay = 0.5f;

    public float timeUntilDestroy = 5;
    public float bulletSpeed = 10;
    public Color chargingColor = Color.red;
    public Color infestingColor = Color.green;

    private LevelEnemyManager enemyManager;
    private Animator anim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        projectilePool = ProjectilePool.Instance;
        enemyManager = transform.parent.GetComponent<LevelEnemyManager>();
        if (!enemyManager)
        {
            foreach (Transform child in transform.parent)
            {
                if (child.GetComponent<LevelEnemyManager>())
                {
                    enemyManager = child.GetComponent<LevelEnemyManager>();
                }
            }
        }
        anim = GetComponent<Animator>();
        timer += .45f;
    }

    void Update()
    {
        if (target)
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            diffX = transform.position.x - target.transform.position.x;
            if (diffX > 0)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        EnemyEvent();
    }

    private void EnemyEvent()
    {
        if (timer <= 0)
        {
            timer = cooldown;
            int randEvent = Random.Range(0, 3);
            if (randEvent == 0)
            {
                StartCoroutine(Attack());
            }
            else
            {
                BuffEnemy();
            }
        }
    }

    private void BuffEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<EnemyHealth> healthList = new List<EnemyHealth>();

        GameObject closestEnemy = gameObject;
        float closestRange = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyHealth>() &&
                enemyManager.playerInLevel && !enemy.GetComponent<InfestationEnemy>() && enemy != gameObject)
            {
                EnemyHealth healthScript = enemy.GetComponent<EnemyHealth>();
                float range = (enemy.transform.position - gameObject.transform.position).magnitude;
                if (!healthScript.buffed && range < closestRange)
                {
                    closestEnemy = enemy;
                    closestRange = range;
                }
            }
        }
        if (closestEnemy == gameObject)
        {
            Attack();
            return;
        }

        int rand = Random.Range(0, healthList.Count);
        EnemyHealth target = closestEnemy.GetComponent<EnemyHealth>();

        target.buffed = true;
        target.maxHealth += 2;
        target.Heal(2);

        if (SettingsLoader.Settings.particlesEnabled)
        {
            target.GetComponentsInChildren<ParticleSystem>()[1].Play();
        }
        StartCoroutine(InfestationAnimation());
    }

    IEnumerator InfestationAnimation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = infestingColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;

        yield return null;
    }

    IEnumerator AttackAnimation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = chargingColor;
        yield return new WaitForSeconds(attackDelay / 2);
        sr.color = originalColor;

        yield return null;
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        StartCoroutine(AttackAnimation());
        yield return new WaitForSeconds(attackDelay);

        GameObject spawnedBullet = projectilePool.SpawnProjectile<LobbedProjectile>(firePoint.transform.position, Quaternion.identity);
        shootSFX?.Post(gameObject);

        if (spawnedBullet != null)
        {
            Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
            Projectile projectile = spawnedBullet.GetComponent<Projectile>();
            projectile.belongsToPlayer = false;

            projectile.timeUntilDestroy = timeUntilDestroy;

            if (target != null)
            {
                Vector3 playerPos = target.transform.position;
                playerPos += new Vector3(0f, 8f, 0f);

                Vector2 directionToPlayer = (playerPos - transform.position).normalized;
                rb.velocity = new Vector2(directionToPlayer.x, 1f) * bulletSpeed;

                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                spawnedBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                rb.velocity = Vector2.down * bulletSpeed;
            }
        }
    }

    IEnumerator DeathAnim()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1.65f);
        Destroy(this.gameObject);
    }

    public void Death()
    {
        StartCoroutine(DeathAnim());
    }

    protected override IEnumerator EnemyRoutine()
    {
        yield break;
    }
}