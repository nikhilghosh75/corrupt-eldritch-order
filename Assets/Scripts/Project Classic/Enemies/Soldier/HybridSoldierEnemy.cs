 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridSoldierEnemy : BaseJumpingEnemy
{
    [Header("Shooting")]
    public float bulletSpeed;
    public GameObject firePoint;
    public ProjectilePool projectilePool;
    public Color chargingColor = Color.red;

    [Header("Charge Attack")]
    public float chargeDuration = 1.0f;
    public float chargeRange = 10.0f;
    public float chargeSpeedMultiplier = 5.0f;
    public float chargeDelay = 1.5f;
    public float chargeCooldown = 1.0f;
    public float chargeShakeSpeed = 50f;
    public float chargeShakeAmount = 1f;

    protected override void Start()
    {
        base.Start();
        projectilePool = ProjectilePool.Instance;
    }

    protected override IEnumerator EnemyRoutine()
    {
        //TODO: should hybrid soldier shoot more often?

        while (true)
        {
            UpdateOrientation();

            if (canMove && enemyManager.playerInLevel)
            {
                Move();
                if (canAct)
                {
                    if (DistanceToPlayerY() > 1 && IsPlayerGrounded())
                    {
                        yield return StartCoroutine(Jump());
                    }
                    else if (Mathf.Abs(DistanceToPlayerX()) < chargeRange && DistanceToPlayerY() > -1)
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            yield return StartCoroutine(ChargeAttack());
                        }
                        else
                        {
                            yield return StartCoroutine(ShootAttack());
                        }
                    }
                }

            }
            yield return null;
        }
    }

    IEnumerator ChargeAttack()
    {
        canMove = false;
        canAct = false;

        //set attack direction before charging attack
        int chargeDirection = DistanceToPlayerX() > 0 ? 1 : -1;

        //dont move for a few seconds
        StartCoroutine(chargeAnimation());
        yield return new WaitForSeconds(chargeDelay);

        float timer = 0.0f;
        //charge in the same direction for duration
        while (timer < chargeDuration)
        {
            Vector2 velocity = rb.velocity;
            velocity.x = chargeDirection * speed * chargeSpeedMultiplier;
            rb.velocity = velocity;

            //if collides with player during charge, apply knockback
            if (CheckPlayerCollision())
            {
                OnPlayerOverlap(chargeDirection);
                break;
            }
            else if (DistanceToPlayerX() / chargeDirection < 0)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        canMove = true;
        yield return new WaitForSeconds(chargeCooldown);
        canAct = true;
    }

    void Shoot()
    {
        GameObject spawnedBullet = projectilePool.SpawnProjectile<SimpleProjectile>(firePoint.transform.position, Quaternion.identity);
        anim.SetTrigger("Shoot");

        if (spawnedBullet != null)
        {
            spawnedBullet.GetComponent<TrackDamageOnCollide>().damageSource = "Soldier";

            Rigidbody2D rb = spawnedBullet.GetComponent<Rigidbody2D>();
            Projectile projectile = spawnedBullet.GetComponent<Projectile>();
            projectile.belongsToPlayer = false;

            int directionToPlayer = DistanceToPlayerX() < 0 ? -1 : 1;
            rb.velocity = bulletSpeed * new Vector2(directionToPlayer, 0);
        }
    }

    IEnumerator ShootAttack()
    {
        canMove = false;
        StartCoroutine(AttackAnimation());
        yield return new WaitForSeconds(chargeDelay);
        
        for (int i = 0; i < 2; i++)
        {
            Shoot();
            yield return new WaitForSeconds(.2f);
        }
        canMove = true;
    }

    IEnumerator AttackAnimation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = chargingColor;
        yield return new WaitForSeconds(chargeDuration / 4);
        sr.color = originalColor;
        yield return new WaitForSeconds(chargeDuration / 4);
        sr.color = chargingColor;
        yield return new WaitForSeconds(chargeDuration / 4);
        sr.color = originalColor;

        yield return null;
    }


    IEnumerator chargeAnimation()
    {
        float timer = 0.0f;
        while (timer < chargeDelay)
        {

            Vector3 position = transform.position;

            position.x += Mathf.Sin(Time.time * chargeShakeSpeed) * chargeShakeAmount;

            transform.position = position;

            timer += Time.deltaTime;
            yield return null;
        }
    }
}