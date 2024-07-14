using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBounceAttack : EnemyFireProjectileAction
{
    [SerializeField] float actionTime;

    // List of projectiles used to destroy them at end of attack
    List<GameObject> currentProjectiles = new();

    // The time (in Time.time) that the action started
    float startTime = 0;
    Coroutine fireCoroutine;

    public override void Act()
    {
        fireCoroutine = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        startTime = Time.time;
        currentProjectiles.Add(projectilePool.SpawnProjectile(projectileData, firePosition.position, Vector2.right + Vector2.down, Quaternion.identity));

        yield return new WaitForSeconds(actionTime / 3);

        currentProjectiles.Add(projectilePool.SpawnProjectile(projectileData, firePosition.position, Vector2.left + Vector2.down, Quaternion.identity));

        yield return new WaitForSeconds(actionTime / 3);

        currentProjectiles.Add(projectilePool.SpawnProjectile(projectileData, firePosition.position, Vector2.left + Vector2.down, Quaternion.identity));

        yield return new WaitForSeconds(actionTime / 3);

        foreach (GameObject projectile in currentProjectiles)
        {
            if (projectile.activeSelf)
                projectile.SetActive(false);
        }
        currentProjectiles.Clear();
    }

    public override float GetActionTime()
    {
        return actionTime;
    }

    public override void Interrupt()
    {
        // If we were firing projectiles, we need to disable those projectiles when the boss would have
        StopCoroutine(fireCoroutine);
        StartCoroutine(InterruptRoutine());
    }

    IEnumerator InterruptRoutine()
    {
        float timeLeftForProjectiles = actionTime - (Time.time - startTime);
        
        // In the event we only interrupt for a short period of time, we might spawn new projectiles before
        // clearing out the existing one. We want those to be separate
        List<GameObject> projectilesToDisable = new List<GameObject>();
        projectilesToDisable.AddRange(currentProjectiles);
        currentProjectiles.Clear();
        
        yield return new WaitForSeconds(timeLeftForProjectiles);

        foreach (GameObject projectile in projectilesToDisable)
        {
            if (projectile.activeSelf)
                projectile.SetActive(false);
        }
    }
}
