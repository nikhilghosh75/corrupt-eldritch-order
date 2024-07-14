using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;
using WSoft.Audio;

public class Infection : MonoBehaviour
{
    public float lifetime;
    public int damage;
    public LayerMask damageLayers;

    public AudioEvent sizzle;

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sizzle.PlayAudio(collision.gameObject);
        DoDamage(collision.gameObject);
    }

    private void DoDamage(GameObject target)
    {
        if ((damageLayers.value & 1 << target.layer) != 0)
        {
            HealthSystem health = target.GetComponent<HealthSystem>();
            if (health)
            {
                health.Damage(damage);
            }
        }
    }
}
