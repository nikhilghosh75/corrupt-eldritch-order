using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float timeUntilDestroy = 1;
    [HideInInspector] public int piercesLeft = 0;
    [HideInInspector] public int damage = 1;
    
    [HideInInspector] public int manaGenerationAmount; // Is set to weapon.manaRecovered during instantiation
    // Classes will need to alter manaGenAmount, if you need to change this please let me know and I can fix it after or try to edit
    // the PlayerClassBase script yourself to account for this. Also modified a few other things, let me know if you're confused. -Jacob
    [HideInInspector] public float manaGenerationModifier = 1f;
    public float baseSizeMultiplier = 1f;

    public bool applyBurn;
    public float timeBetweenBurns;
    public int numBurns;

    public bool applyFreeze;
    public float freezeTime;

    public bool applyShock;

    public bool belongsToPlayer = true;

    Vector3 baseScale;

    public UnityEvent onDeath = new();
    bool initialized = false;

    public bool setInactiveOnDeath = true;
    public virtual void Destroy()
    {
        ResetProjectileSize(); //Needed to ensure projectiles don't balloon over time w/ projectile size powerup
        onDeath.Invoke();
        if (setInactiveOnDeath)
        {
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        if (initialized)
        {
            Invoke("Destroy", timeUntilDestroy);
        }
        else
        {
            initialized = true;
        }
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        BossHealthSystem boss = collision.gameObject.GetComponent<BossHealthSystem>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (damage == 0)
        {
            // somehow, some projectiles had damage set to 0 and weren't damaging enemies.
            // kind of a weird fix, but it should work fine
            damage = 1;
        }

        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground") 
            || collision.gameObject.layer == LayerMask.NameToLayer("Wall")) && // hitting enemy & doesn't belong to player or hitting something that isn't an enemy & belongs to player
            (!GetComponent<IsBouncing>() || !GetComponent<IsBouncing>().enabled))   // Also make sure bouncing doesn't exist on this object
        {
            Destroy();
        }

        if (player && belongsToPlayer)
            return;

        if (enemy != null && belongsToPlayer)
        {
            enemy.TakeDamage(damage);
            float manaGenerated = (float)(manaGenerationAmount * manaGenerationModifier);
            if (manaGenerated > 0 && manaGenerated < 1) 
                EventBus.Publish<GenerateManaEvent>(new GenerateManaEvent(1)); //this accounts for weapons that have mana gen dropped below 1 after the class modifier is applied
            else 
                EventBus.Publish<GenerateManaEvent>(new GenerateManaEvent((int)manaGenerated)); //this accounts for weapons that shouldn't generate mana and weapons that generate >1 mana
            if (piercesLeft == 0)
            {
                Destroy();
            }
            else
            {
                piercesLeft--;
            }

            ApplyEffects(enemy);
        }
        else if (boss != null && belongsToPlayer)
        {
            boss.Damage(damage);
            Destroy();
        }
        else
        {
            if(player != null && !belongsToPlayer)
            {
                player.Damage(damage);
                Destroy();
            }
        }
    }

    void ApplyEffects(EnemyHealth enemy)
    {
        if (applyBurn)
        {
            enemy.StartBurning(timeBetweenBurns, numBurns);
        }

        if (applyFreeze)
        {
            if (enemy.TryGetComponent<EnemyBehavior>(out EnemyBehavior behavior))
            {
                behavior.Freeze(freezeTime);
            }
            else if (enemy.TryGetComponent<IFreeze>(out IFreeze iFreeze))
            {
                iFreeze.Freeze(freezeTime);
            }
        }

        if(applyShock)
        {
            GameObject nearestEnemy;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;

            nearestEnemy = enemies[0];

            foreach (GameObject obj in enemies)
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = obj;
                }
            }

            EnemyHealth nearestEnemyHealth = nearestEnemy.GetComponent<EnemyHealth>();
            if (nearestEnemyHealth != null)
            {
                nearestEnemyHealth.TakeDamage((int) Mathf.Max(1, damage * 0.5f));
                Debug.Log("Shock!");
            }

        }
    }

    private void Awake()
    {
        baseScale = this.transform.localScale;
    }

    public void ResetProjectileSize()
    {
        this.transform.localScale = baseScale;
    }
}
