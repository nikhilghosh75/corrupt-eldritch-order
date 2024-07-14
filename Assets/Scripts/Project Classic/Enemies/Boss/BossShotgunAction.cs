using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgunAction : EnemyFireProjectileAction
{
    //reference to player's transform
    [SerializeField] Transform player_transform;

    [SerializeField] float shotgun_angle = 45.0f;

    [SerializeField] int numAttacks = 3;
    [SerializeField] float chargeTime = 2;
    [SerializeField] float fireDelay = 1;

    Coroutine attackCoroutine;

    private void Awake()
    {
        player_transform = PlayerController.Instance.transform;
    }

    public override void Act()
    {
        attackCoroutine = StartCoroutine(Attack());
    }

    public override float GetActionTime()
    {
        return chargeTime + fireDelay * (numAttacks-1) + cooldownTime;
    }

    private IEnumerator Attack()
    {
        //get the player's position at the start of the attack
        Vector2 target = player_transform.position;
        //charge attack
        for (int i=0; i<numAttacks; ++i)
        {
            if (i == 0)
                yield return new WaitForSeconds(chargeTime);
            else
                yield return new WaitForSeconds(fireDelay);
            FireWave(target);
        }

        yield return new WaitForSeconds(cooldownTime);
    }

    private void FireWave(Vector2 target)
    {
        //fire 5 evenly spaced bullets between the shotgun angle
        FireBulletAtTarget(target, -shotgun_angle / 2, 10);
        FireBulletAtTarget(target, -shotgun_angle / 4, 10);
        FireBulletAtTarget(target, 0, 10);
        FireBulletAtTarget(target, shotgun_angle / 4, 10);
        FireBulletAtTarget(target, shotgun_angle / 2, 10);
    }

    public override void Interrupt()
    {
        StopCoroutine(attackCoroutine);
    }
}
