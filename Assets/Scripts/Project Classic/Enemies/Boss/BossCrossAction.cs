using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrossAction : EnemyFireProjectileAction
{
    [SerializeField] float fireRate;
    [SerializeField] float actionTime;
    [SerializeField] float rotationSpeed;

    Coroutine fireCoroutine;

    public override void Act()
    {
        fireCoroutine = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        float currentAngle = 0;
        float startTime = Time.time;

        while (Time.time - startTime < actionTime)
        {
            for (int i = 0; i < 4; ++i)
            {
                float angle = i * Mathf.PI / 2 + currentAngle;
                Vector2 fireDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                projectilePool.SpawnProjectile(projectileData, firePosition.position, fireDirection, Quaternion.identity);
            }

            currentAngle += rotationSpeed * fireRate;

            yield return new WaitForSeconds(1/fireRate);
        }

        yield return new WaitForSeconds(cooldownTime);
    }

    public override float GetActionTime()
    {
        return actionTime + cooldownTime;
    }

    public override void Interrupt()
    {
        StopCoroutine(fireCoroutine);
    }
}
