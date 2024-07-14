using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtPlayerAction : EnemyFireProjectileAction
{
    PlayerHealth player;

    [SerializeField] float chargeTime;
    [SerializeField] int numAttacks;
    [SerializeField] float fireRate;
    [SerializeField] float cooldownTime;

    Coroutine fireCoroutine;
    Animator anim;

    private void Start()
    {
        PlayerHealth[] playerHealths = GameObject.FindObjectsOfType<PlayerHealth>();
        // TODO: Make this handle multiple players
        if (playerHealths.Length > 0) Debug.LogError("Multiple Players in scene");
        player = playerHealths[0];

        projectilePool = ProjectilePool.Instance;
        anim = GetComponent<Animator>();
    }

    public override void Act()
    {
        fireCoroutine = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        yield return new WaitForSeconds(chargeTime);
        for (int i=0; i<numAttacks; ++i)
        {
            if (i>0)
            {
                yield return new WaitForSeconds(fireRate);
            }
            Vector3 direction;
            if (player != null)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            else
            {
                direction = Vector2.down;
            }
            projectilePool.SpawnProjectile(projectileData, firePosition.position, direction, Quaternion.identity);
            anim.SetTrigger("Shoot");
        }
        yield return new WaitForSeconds(cooldownTime);
    }

    public override float GetActionTime()
    {
        return chargeTime + (numAttacks-1)*fireRate + cooldownTime;
    }

    public override void Interrupt()
    {
        StopCoroutine(fireCoroutine);
    }
}
