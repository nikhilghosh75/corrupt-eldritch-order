using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WSoft.Combat;
using WSoft.Math;

public class EnemyBurnedEvent
{

}

public class EnemyFrozenEvent
{

}

public class EnemyHealth : BasicHealth
{
    [SerializeField]
    GameObject explosion, infection;
    [SerializeField]
    GameObject burnObject;

    public bool buffed = false;
    public float explosionDelay;

    bool burning = false;
    float timeBetweenDamages;
    int numBurns;

    public bool isDying = false;

    // Store the starting alpha so it doesn't get messed up
    float startingAlpha;

    protected override void Initialize()
    {
        base.Initialize();
        burnObject.SetActive(false);
        startingAlpha = GetComponentInChildren<SpriteRenderer>().color.a;
    }

    public bool Heal(int health)
    {
        return ApplyHeal(health);
    }

    public void TakeDamage(int damage)
    {
        ApplyDamage(damage);
        
        if (SettingsLoader.Settings.particlesEnabled)
        {
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            if (particleSystems.Length > 0)
            {
                particleSystems[0].Play();
            }
        }

        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            if (sr.color == new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f))
            {
                if (GetComponent<SpriteRenderer>() != null)
                {
                    GetComponent<SpriteRenderer>().color = new Color(sr.color.r, sr.color.g, sr.color.b, startingAlpha);
                }
            }
        }
        
        StartCoroutine(DamageAnimation());
    }

    public void Destroy()
    {
        if (gameObject.GetComponent<InfestationEnemy>() || buffed)
        {
            StartCoroutine(Explosion());
        }
        if (gameObject.GetComponent<InfestationEnemy>())
        {
            Instantiate(infection, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            Debug.Log("Infected!");
        }
        Destroy(gameObject);
        EventBus.Publish(new EnemyKilledEvent(this));
    }

    public void DestroyIn(float seconds)
    {
        isDying = true;
        GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("Projectile");
        Debug.Log(isDying + "HAS BEEN SET");
        Invoke("Destroy", seconds);
    }

    public void StartBurning(float _timeBetweenDamages, int _numBurns)
    {
        timeBetweenDamages = _timeBetweenDamages;
        numBurns = _numBurns;
        StartCoroutine(PerformBurn());
        EventBus.Publish(new EnemyBurnedEvent());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        GameObject exp = Instantiate(explosion, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        exp.GetComponent<Explode>().activatedByPlayer = true;
        Debug.Log("Exploded!");
    }

    IEnumerator PerformBurn()
    {
        burning = true;
        burnObject.SetActive(true);

        for(int i = 0; i < numBurns; i++)
        {
            yield return new WaitForSeconds(timeBetweenDamages);
            ApplyDamage(1);
            StartCoroutine(DamageAnimation());
        }

        burnObject.SetActive(false);
        burning = false;
    }

    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in srs)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        }
        yield return new WaitForSeconds(0.05f);
        foreach (SpriteRenderer sr in srs)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startingAlpha);
        }
        yield return new WaitForSeconds(0.05f);
        foreach (SpriteRenderer sr in srs)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        }
        yield return new WaitForSeconds(0.05f);
        foreach (SpriteRenderer sr in srs)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startingAlpha);
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(burning)
        {
            if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.StartBurning(timeBetweenDamages, numBurns);
            }
        }
    }
}
