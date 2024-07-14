using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBurstFireAction : EnemyFireProjectileAction
{
    //reference to player transform
    [SerializeField] Transform player;

    [SerializeField] int bullet_speed = 30;

    [SerializeField] int numBursts = 3;
    [SerializeField] int shotsPerBurst = 3;
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] float timeBetweenBursts = 0.5f;

    Coroutine attackCoroutine;

    private void Awake()
    {
        player = FindObjectOfType<PlayerHealth>().transform;
    }

    public override void Act()
    {
        attackCoroutine = StartCoroutine(Attack());
    }

    public override float GetActionTime()
    {
        return 1 + numBursts*timeBetweenBursts + numBursts*shotsPerBurst*fireDelay + cooldownTime;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.0f);
        
        for (int i = 0; i < numBursts; ++i)
        {
            Vector3 target = player.position;
            for (int j = 0; j < shotsPerBurst; ++j)
            {
                projectilePool.SpawnProjectile(projectileData, firePosition.position, target - firePosition.position, Quaternion.identity);
                yield return new WaitForSeconds(fireDelay);
            }
            yield return new WaitForSeconds(timeBetweenBursts);
        }

        yield return new WaitForSeconds(cooldownTime);
    }

    public override void Interrupt()
    {
        StopCoroutine(attackCoroutine);
    }
}
