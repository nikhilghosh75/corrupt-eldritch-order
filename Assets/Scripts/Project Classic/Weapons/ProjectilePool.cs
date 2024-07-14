using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : Projectile { }
public class minimachinegunProjectile : Projectile { }
public class revolverProjectile : Projectile { }
public class manaextractorProjectile : Projectile { }
public class sniperProjectile : Projectile { }
public class flamethrowerProjectile : Projectile { }
public class explosiveProjectile : Projectile { }
public class lasershotgunProjectile : Projectile { }
public class impactgunProjectile : Projectile { }
public class bouncingbladesProjectile : Projectile { }
public class tripleexplosivemachinegunProjectile : Projectile { }
public class supersniperProjectile : Projectile { }
public class manaburstProjectile : Projectile { }
public class chainlightningProjectile : Projectile { }
public class warlockProjectile : Projectile { }

public class ProjectilePool : MonoBehaviour
{
    public GameObject baseProjectilePrefab;

    public GameObject simpleProjectilePrefab;
    public GameObject lobbedProjectilePrefab;

    // Weak Weapons
    public GameObject minimachinegunProjectilePrefab;
    public GameObject revolverProjectilePrefab;
    public GameObject manaextractorProjectilePrefab;

    // Strong Weapons
    public GameObject sniperProjectilePrefab;
    public GameObject flamethrowerProjectilePrefab;
    public GameObject explosiveProjectilePrefab;

    // Combo Weapons
    public GameObject lasershotgunProjectilePrefab;
    public GameObject impactgunProjectilePrefab;
    public GameObject bouncingbladesProjectilePrefab;
    public GameObject tripleexplosivemachinegunProjectilePrefab;
    public GameObject supersniperProjectilePrefab;
    public GameObject chainlightningProjectilePrefab;
    public GameObject manaburstProjectilePrefab;
    public GameObject warlockProjectilePrefab;

    // Customize the pool size based on your needs
    public int initialPoolSize = 10;

    // Pointer to the next projectile in the projectile pool
    int poolPointer = 0;

    public List<GameObject> baseProjectiles;
    public Dictionary<System.Type, List<Projectile>> projectilePools;
    public Dictionary<System.Type, Transform> projectilePoolParents;

    public static ProjectilePool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        projectilePools = new Dictionary<System.Type, List<Projectile>>();
        projectilePoolParents = new Dictionary<System.Type, Transform>();

        GameObject basePoolParent = new GameObject("BaseProjectile Pool");
        basePoolParent.transform.parent = transform;
        projectilePoolParents[typeof(Projectile)] = basePoolParent.transform;
        // Initialize Base Projectiles
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject newProjectile = Instantiate(baseProjectilePrefab, basePoolParent.transform);
            newProjectile.SetActive(false);
            baseProjectiles.Add(newProjectile);
        }

        InitializePool<SimpleProjectile>(simpleProjectilePrefab, initialPoolSize);
        InitializePool<LobbedProjectile>(lobbedProjectilePrefab, initialPoolSize);
        InitializePool<warlockProjectile>(warlockProjectilePrefab, initialPoolSize);

        InitializePool<minimachinegunProjectile>(minimachinegunProjectilePrefab, initialPoolSize);
        InitializePool<revolverProjectile>(revolverProjectilePrefab, initialPoolSize);
        InitializePool<manaextractorProjectile>(manaextractorProjectilePrefab, initialPoolSize);

        InitializePool<sniperProjectile>(sniperProjectilePrefab, initialPoolSize);
        InitializePool<flamethrowerProjectile>(flamethrowerProjectilePrefab, initialPoolSize);
        InitializePool<explosiveProjectile>(explosiveProjectilePrefab, initialPoolSize);

        InitializePool<lasershotgunProjectile>(lasershotgunProjectilePrefab, initialPoolSize);
        InitializePool<impactgunProjectile>(impactgunProjectilePrefab, initialPoolSize);
        InitializePool<manaburstProjectile>(manaburstProjectilePrefab, 2);
        InitializePool<bouncingbladesProjectile>(bouncingbladesProjectilePrefab, initialPoolSize);
        InitializePool<tripleexplosivemachinegunProjectile>(tripleexplosivemachinegunProjectilePrefab, initialPoolSize);
        InitializePool<supersniperProjectile>(supersniperProjectilePrefab, initialPoolSize);
        InitializePool<chainlightningProjectile>(chainlightningProjectilePrefab, initialPoolSize);
    }

    void InitializePool<T>(GameObject prefab, int size) where T : Projectile
    {
        // Makes sure prefab is assigned so script doesn't break
        if (!prefab)
        {
            return;
        }
        List<Projectile> pool = new List<Projectile>();

        // Initialize pool parent
        GameObject poolParent = new GameObject(typeof(T).Name + " Pool");
        poolParent.transform.parent = transform;
        projectilePoolParents[typeof(T)] = poolParent.transform;

        // Instantiate projectiles
        for (int i = 0; i < size; i++)
        {
            GameObject newProjectile = Instantiate(prefab, poolParent.transform);
            newProjectile.SetActive(false);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();

            // Ensure the projectile component is not null before adding to the pool
            if (projectileComponent != null)
            {
                pool.Add(projectileComponent);
            }
        }

        projectilePools[typeof(T)] = pool;
    }

    Projectile GetProjectile<T>() where T : Projectile
    {
        if (projectilePools.TryGetValue(typeof(T), out List<Projectile> pool))
        {
            foreach (Projectile projectile in pool)
            {
                if (!projectile.gameObject.activeSelf)
                {
                    return projectile;
                }
            }

            // If no inactive projectile is found, instantiate a new one
            GameObject newProjectile = Instantiate(simpleProjectilePrefab, projectilePoolParents[typeof(T)]);
            newProjectile.SetActive(false);
            pool.Add(newProjectile.GetComponent<T>());
            return newProjectile.GetComponent<T>();
        }

        return null; // Pool not found for the specified type
    }

    // Untyped GetProjectile which will get a projectile from the base pool
    GameObject GetProjectile()
    {
        // First check for an inactive projectile at the current pointer
        for (int i=0; i<baseProjectiles.Count; ++i)
        {
            int index = (poolPointer + i) % baseProjectiles.Count;
            if (!baseProjectiles[index].activeSelf)
            {
                GameObject projectile = baseProjectiles[index];
                poolPointer = (index + 1) % baseProjectiles.Count;
                return projectile;
            }
        }

        // If there are no inactive projectiles, create a new one
        GameObject newProjectile = Instantiate(baseProjectilePrefab, projectilePoolParents[typeof(Projectile)]);
        baseProjectiles.Add(newProjectile);
        return newProjectile;
    }

    // Example method to spawn a projectile from the pool
    public GameObject SpawnProjectile<T>(Vector3 spawnPosition, Quaternion spawnRotation) where T : Projectile
    {
        Projectile projectile = GetProjectile<T>();

        if (projectile != null)
        {
            projectile.transform.position = spawnPosition;
            projectile.transform.rotation = spawnRotation;

            projectile.gameObject.SetActive(true);
        }

        return projectile.gameObject;
    }

    // Spawn a base projectile and assign it attributes from Projectile Data
    public GameObject SpawnProjectile(ProjectileData projectileData, Vector3 spawnPosition, Vector3 direction, Quaternion spawnRotation)
    {
        // Retrieve a projectile from the pool
        GameObject projectile = GetProjectile();
        projectile.name = "NEW PROJECTILE";
        projectile.transform.position = spawnPosition;
        projectile.transform.rotation = spawnRotation;
        Vector2 scale = projectile.transform.localScale;
        scale.x = projectileData.scaleX;
        scale.y = projectileData.scaleY;
        projectile.transform.localScale = scale;
        projectile.GetComponent<CircleCollider2D>().radius = projectileData.colliderRadius;

        // Set projectile visuals
        projectile.GetComponent<SpriteRenderer>().sprite = projectileData.sprite;

        // Check if explosive and replace with explsive projectile
        // TODO: Make explosive projectiles use composition, not inheritance
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileData.isExplosive && projectileComponent is not ExplosiveProjectile)
        {
            // Replace projectile component
            Destroy(projectileComponent);
            projectileComponent = projectile.AddComponent(typeof(ExplosiveProjectile)) as ExplosiveProjectile;
        }
        else if (!projectileData.isExplosive && projectileComponent is ExplosiveProjectile)
        {
            // Replace with normal projectile component
            Destroy(projectileComponent);
            projectileComponent = projectile.AddComponent(typeof(Projectile)) as Projectile;
        }
        if (projectileData.isExplosive && projectileData.explosionPrefabs.Count > 0)
        {
            projectile.GetComponent<ExplosiveProjectile>().explosivePrefabs = projectileData.explosionPrefabs;
            projectile.GetComponent<ExplosiveProjectile>().explosiveRadii = projectileData.explosionRadii;
        }
        foreach (Projectile p in projectile.GetComponents<Projectile>())
        {
            p.timeUntilDestroy = projectileData.timeUntilDestroy;

        }
        projectile.SetActive(true);

        // Set base stats
        projectileComponent.damage = projectileData.damage;
        projectileComponent.piercesLeft = projectileData.pierce;
        projectileComponent.belongsToPlayer = projectileData.belongsToPlayer;
        // projectileComponent.timeUntilDestroy = projectileData.timeUntilDestroy;
        Rigidbody2D rigid = projectile.GetComponent<Rigidbody2D>();
        rigid.velocity = direction.normalized * projectileData.speed;

        // Lobbed stuff
        if (projectileData.isLobbed)
        {
            rigid.gravityScale = projectileData.gravityScale;
        }
        else
        {
            rigid.gravityScale = 0;
        }

        // Handle bouncing
        if (projectileData.isBouncing)
        {
            projectile.GetComponent<IsBouncing>().enabled = true;
            projectile.GetComponent<IsBouncing>().invokeOnDeathOnBounce = projectileData.invokeOnDeathOnBounce;
            projectile.GetComponent<IsBouncing>().numBounces = projectileData.numBounces;
        }
        else
        {
            projectile.GetComponent<IsBouncing>().enabled = false;
        }

        // TODO: Handle Burning

        // TODO: Handle Freezing

        // TODO: Handle Homing

        return projectile;
    }
}
