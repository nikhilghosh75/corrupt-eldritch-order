using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using WSoft.Combat;
using System;

public class WarlockEnemy : EnemyBehavior
{
    public float speed;
    public float bulletSpeed;
    public float timer;

    public int damageOnCollision = 10;
    public float knockback = 20f;

    [SerializeField]
    private GameObject target;

    private Rigidbody2D rb;
    private Animator anim;
    private WarlockTeleport teleportAction;
    private WarlockAttack attackAction;
    public bool attacking = false;

    public GameObject firePoint;
    public ProjectilePool projectilePool;
    private LevelEnemyManager enemyManager;
    private float direction = 1;
    private bool isDead = false;

    private Coroutine actionCoroutine = null;

    [Header("Audio")]
    public AK.Wwise.Event shoot;
    public AK.Wwise.Event teleport;
    public AK.Wwise.Event attack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attackAction = GetComponent<WarlockAttack>();
        teleportAction = GetComponent<WarlockTeleport>();
        target = GameObject.FindGameObjectWithTag("Player");
        projectilePool = ProjectilePool.Instance;

        enemyManager = transform.parent.GetComponent<LevelEnemyManager>();
        if (!enemyManager)
        {
            foreach(Transform child in transform.parent)
            {
                if (child.GetComponent<LevelEnemyManager>())
                {
                    enemyManager = child.GetComponent<LevelEnemyManager>();
                }
            }
        }
        if (timer < .01) {
            timer = 1f;
        }
    }

    void Update()
    {
        if (!enemyManager.playerInLevel)
        {
            //Idle behavior
            return;
        }
        float dist = transform.position.x - target.transform.position.x;
        if (Math.Abs(dist) > 1f)
        {
            if ((dist > 0 && transform.parent.localScale.x > 0) ||
                (dist < 0 && transform.parent.localScale.x < 0))//if direction is not the direction towards the player
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                direction = -1;
            }
            else if ((dist < 0 && transform.parent.localScale.x > 0) ||
                (dist > 0 && transform.parent.localScale.x < 0))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                direction = 1;
            }

            if (attacking)
            {
                return;
            }
        }
        Move();
    }

    protected override IEnumerator EnemyRoutine()
    {
        while (this)
        {
            if (enemyManager.playerInLevel)
            {
                yield return new WaitForSeconds(timer);
                attacking = true;
                yield return StartCoroutine(ExecuteAction(attackAction));
                yield return StartCoroutine(ExecuteAction(teleportAction));
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }

    IEnumerator DeathAnim()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1.65f);
        Destroy(this.gameObject);
    }


    void Move()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < .25f)
        {
            return;
        }
        Vector2 velocity = rb.velocity;
        velocity.x = direction * speed;
        rb.velocity = velocity;
    }

    public void Death()
    {
        isDead = true;
        StartCoroutine(DeathAnim());
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int knockDirection = transform.position.x < collision.gameObject.transform.position.x ? 1 : -1;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback * knockDirection, knockback / 2), ForceMode2D.Impulse);
            collision.gameObject.GetComponent<PlayerHealth>().Damage(damageOnCollision);
        }
    }
}
