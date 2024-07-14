using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebufferAura : MonoBehaviour
{
    public enum DebuffType
    {
        SLOW,
        MANA,
        VISION,
        SILENCE,
        PROJECTILE
    }

    private List<GameObject> affectedGameObjects = new List<GameObject>();

    public DebuffType debufferType;

    [Header("Slow")]
    public float SpeedDebuffPercentage = 0.15f;

    [Header("Mana Drain")]
    public float ManaDrainFrequency = .5f;

    [Header("Vision Debuff")]
    public LayerMask obscuredLayers;

    [Header("Silence")]

    [Header("Projectile Debuff")]
    public float shrinkPercentage = 0.5f;
    public float slowPercentage = 0.5f;

    private float timer = 0;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        debufferType = (DebuffType)Mathf.FloorToInt(Random.Range(0, 4.999f));

        switch (debufferType)
        {
            case DebuffType.SLOW:
                sr.color = Color.red;
                break;
            case DebuffType.MANA:
                sr.color = Color.blue;
                break;
            case DebuffType.VISION:
                sr.color = Color.black;
                break;
            case DebuffType.SILENCE:
                sr.color = Color.green;
                break;
            case DebuffType.PROJECTILE:
                sr.color = Color.yellow;
                break;
            default:
                break;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 35f / 255f);
    }

    private void Update()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 35f / 255f);
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer = 0;

            ParticleSystem[] particleSystems = collision.gameObject.GetComponentsInChildren<ParticleSystem>();
            if (particleSystems.Length > 0)
            {
                particleSystems[0].Play();
            }
        }

        affectedGameObjects.Add(collision.gameObject);

        switch (debufferType)
        {
            case DebuffType.SLOW:
                if (collision.gameObject.CompareTag("Player"))
                {
                    ActivateSlow(collision.gameObject);
                }
                break;
            case DebuffType.SILENCE:
                if (collision.gameObject.CompareTag("Player"))
                {
                    ActivateSilence(collision.gameObject);
                }
                break;
            case DebuffType.VISION:
                ObscureObject(collision.gameObject);
                break;
            case DebuffType.PROJECTILE:
                if (collision.gameObject.CompareTag("PlayerProjectile"))
                {
                    ShrinkProjectile(collision.gameObject);
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        affectedGameObjects.Remove(collision.gameObject);

        if (collision.gameObject.CompareTag("Player"))
        {

            ParticleSystem[] particleSystems = collision.gameObject.GetComponentsInChildren<ParticleSystem>();
            if (particleSystems.Length > 0)
            {
                particleSystems[0].Stop();
            }
        }
        switch (debufferType)
        {
            case DebuffType.SLOW:
                if (collision.gameObject.CompareTag("Player"))
                {
                    DeactivateSlow(collision.gameObject);
                }
                break;
            case DebuffType.SILENCE:
                if (collision.gameObject.CompareTag("Player"))
                {
                    DeactivateSilence(collision.gameObject);
                }
                break;
            case DebuffType.VISION:
                RevealObject(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(debufferType == DebuffType.MANA)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DrainMana(collision.gameObject);
            }
        }
    }

    private void ActivateSlow(GameObject player)
    {
        player.GetComponent<PlayerController>().groundController.groundSpeed *= (1 - SpeedDebuffPercentage);
        player.GetComponent<PlayerController>().airController.jumpingPower *= (1 - SpeedDebuffPercentage);
        player.GetComponent<Rigidbody2D>().gravityScale *= (1 - SpeedDebuffPercentage);
    }

    private void DeactivateSlow(GameObject player)
    {
        player.GetComponent<PlayerController>().groundController.groundSpeed /= (1 - SpeedDebuffPercentage);
        player.GetComponent<PlayerController>().airController.jumpingPower /= (1 - SpeedDebuffPercentage);
        player.GetComponent<Rigidbody2D>().gravityScale /= (1 - SpeedDebuffPercentage);
    }

    private void DrainMana(GameObject player)
    {
        if (timer > ManaDrainFrequency)
        {
            player.GetComponent<PlayerMana>().currentMana--;
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private void ObscureObject(GameObject actor)
    {
        Debug.Log("we are obscuring an object");
        // do not obscure other debuffers
        if (actor.GetComponent<DebufferEnemy>() != null) return;
        //only obscure other enemies, the player, and projectiles
        if (actor.CompareTag("Enemy") || actor.CompareTag("Player") || actor.CompareTag("PlayerProjectile"))
        {
            Renderer[] renderers = actor.GetComponents<SpriteRenderer>();
            Debug.Log(renderers.Length);
            foreach (Renderer renderer in renderers)
            {
                renderer.sortingLayerName = "Background";
                renderer.sortingOrder = -999;
            }
        } 
    }

    private void RevealObject(GameObject actor)
    {
        if (actor.GetComponent<DebufferEnemy>() != null) return;
        if (!(actor.CompareTag("Enemy") || actor.CompareTag("Player") || actor.CompareTag("PlayerProjectile"))) return;
        Renderer[] renderers = actor.GetComponents<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            switch (actor.tag)
            {
                case "Enemy":
                    renderer.sortingLayerName = "Enemy";
                    renderer.sortingOrder = 0;
                    break;
                case "Player":
                    renderer.sortingLayerName = "Player";
                    renderer.sortingOrder = 0;
                    break;
                case "Projectile":
                    renderer.sortingLayerName = "Projectiles";
                    renderer.sortingOrder = 0;
                    break;
                default:
                    break;
            }
        }
    }

    private void ActivateSilence(GameObject player)
    {
        player.GetComponent<PlayerController>().debuffed = true;
        //TODO: still need to disable double jump
    }

    private void DeactivateSilence(GameObject player)
    {
        player.GetComponent<PlayerController>().debuffed = false;
    }

    private void ShrinkProjectile(GameObject projectile)
    {
        projectile.transform.localScale = projectile.transform.localScale * shrinkPercentage;
        //TODO: not sure if projectiles are correctly slowed yet
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity * slowPercentage;
    }

    private void OnDestroy()
    {
        foreach(GameObject actor in affectedGameObjects)
        {
            switch (debufferType)
            {
                case DebuffType.SLOW:
                    if (actor.CompareTag("Player"))
                    {
                        DeactivateSlow(actor);
                    }
                    break;
                case DebuffType.SILENCE:
                    if (actor.CompareTag("Player"))
                    {
                        DeactivateSilence(actor);
                    }
                    break;
                case DebuffType.VISION:
                    RevealObject(actor);
                    break;
                default:
                    break;
            }
        }
    }
}
