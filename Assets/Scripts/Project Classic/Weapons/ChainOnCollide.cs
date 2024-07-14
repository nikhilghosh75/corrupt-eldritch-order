using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainOnCollide : MonoBehaviour
{
    public int numberOfChains;
    public float damageMultiplier;
    public float speedMultiplier;
    public LayerMask enemyLayerMask;
    public LayerMask levelLayerMask;

    int currentChain = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentChain == numberOfChains)
        {
            return;
        }

        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            currentChain++;

            GameObject nextEnemy = FindEnemy(enemy.gameObject);
            if (nextEnemy == null)
                return;

            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            Projectile projectile = GetComponent<Projectile>();

            Vector2 direction = nextEnemy.transform.position - transform.position;
            float speed = rigidbody.velocity.magnitude;
            rigidbody.velocity = direction.normalized * speed * speedMultiplier;

            projectile.damage = (int)(projectile.damage * damageMultiplier);
        }
    }

    GameObject FindEnemy(GameObject currentEnemy)
    {
        Vector2 position = transform.position;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(position, 25f, enemyLayerMask);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject == currentEnemy)
            {
                continue;
            }

            Vector2 enemyPosition = enemy.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyPosition - position, 25f, levelLayerMask);
            if (hit.collider == null)
            {
                return enemy.gameObject;
            }
        }

        return null;
    }
}
