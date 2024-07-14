using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockAttack : EnemyAction
{
    public ProjectilePool projectilePool;
    public AK.Wwise.Event shoot;
    public GameObject firePoint;

    private GameObject target;
    public float bulletSpeed;

    public override void Act()
    {
        StartCoroutine(Attack());
    }

    public override float GetActionTime()
    {
        return 1.2f;
    }

    public override void Interrupt()
    {
        
    }

    IEnumerator Attack()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject warlockBullet = projectilePool.SpawnProjectile<warlockProjectile>(firePoint.transform.position, Quaternion.identity);
            shoot?.Post(gameObject);

            if (warlockBullet != null)
            {
                Rigidbody2D rb = warlockBullet.GetComponent<Rigidbody2D>();
                Projectile projectile = warlockBullet.GetComponent<Projectile>();
                projectile.belongsToPlayer = false;

                int directionToPlayer = transform.position.x < target.transform.position.x ? 1 : -1;
                rb.velocity = bulletSpeed * new Vector2(directionToPlayer, 0);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
}
