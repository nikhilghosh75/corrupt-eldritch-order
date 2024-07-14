using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Audio;

public class RangedSoldierEnemy : EnemyBehavior
{
    public float enemySpeed = 3.0f;
    [Header("General")]
    public bool isStationary = false;

    [Header("Shooting")]
    public GameObject firePoint;
    public float attackTimer;
    private float shotCooldown;
    public float shotCooldownMin = 2.0f;
    public float shotCooldownMax = 4.0f;
    public float movementDelay;
    public float timeUntilDestroy;
    public float bulletSpeed;
    public float shootRange = 10f;
    public float chargeDuration = 1.0f;
    public Color chargingColor = Color.red;
    public AudioEvent shootSfx;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]
    private Transform groundChecker;
    [SerializeField]
    private Transform middleGroundChecker;

    public LayerMask groundLayer;
    private LevelEnemyManager enemyManager;

    [SerializeField]
    GameObject target;

    bool active = false;
    private bool canMove = true;

    SpriteRenderer enemyRenderer;

    public ProjectilePool projectilePool;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        projectilePool = ProjectilePool.Instance;
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyManager = transform.parent.GetComponent<LevelEnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 1;
        anim.SetBool("Moving", active && isMoving);
    }

    void Move()
    {
        if (isStationary) return;
        if (!IsGroundAhead() && IsGrounded())
        {
            //turn around
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            Vector2 velocity = rb.velocity;
            velocity.x = (transform.localScale.x / Mathf.Abs(transform.localScale.x)) * enemySpeed;
            rb.velocity = velocity;
        }
    }

    private IEnumerator Shoot()
    {
        canMove = false;
        rb.velocity = Vector2.zero;

        float direction = target.transform.position.x - transform.position.x;
        if(direction < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        //charge up shot
        StartCoroutine(AttackAnimation());
        yield return new WaitForSeconds(chargeDuration);

        anim.SetTrigger("Shoot");

        //play audio
        shootSfx.PlayAudio(gameObject);

        GameObject spawnedBullet = projectilePool.SpawnProjectile<SimpleProjectile>(firePoint.transform.position, Quaternion.identity);

        if (spawnedBullet != null)
        {
            spawnedBullet.GetComponent<TrackDamageOnCollide>().damageSource = "Soldier";

            Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
            Projectile projectile = spawnedBullet.GetComponent<Projectile>();
            projectile.belongsToPlayer = false;
            projectile.timeUntilDestroy = timeUntilDestroy;

            Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;
            rb.velocity = bulletSpeed * directionToPlayer;

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            spawnedBullet.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

        shotCooldown = Random.Range(shotCooldownMin, shotCooldownMax);

        yield return new WaitForSeconds(movementDelay);
        canMove = true;
    }

    IEnumerator AttackAnimation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = chargingColor;
        yield return new WaitForSeconds(chargeDuration/4);
        sr.color = originalColor;
        yield return new WaitForSeconds(chargeDuration / 4);
        sr.color = chargingColor;
        yield return new WaitForSeconds(chargeDuration / 4);
        sr.color = originalColor;

        yield return null;
    }

    protected override IEnumerator EnemyRoutine()
    {
        while (true)
        {
            if(canMove)
            //if (canMove && enemyManager.playerInLevel)
            {
                Move();
                if (attackTimer > shotCooldown)
                {
                    yield return StartCoroutine(Shoot());
                    attackTimer = 0;
                }
                attackTimer += Time.deltaTime;
            }
            yield return null;
        }
    }

    bool IsGroundAhead()
    {
        //Debug.DrawRay(groundChecker.position, Vector2.down * 5, Color.red, 2.0f);
        //Debug.DrawRay(groundChecker.position, Vector2.right * transform.localScale.x * 5, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(groundChecker.position, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D wallHit = Physics2D.Raycast(groundChecker.position, Vector2.right * transform.localScale.x, 0.1f, groundLayer);
        return hit.collider != null || wallHit.collider != null;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(middleGroundChecker.position, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    public void Death()
    {
        canMove = false;
        StopAllCoroutines();
    }
}
