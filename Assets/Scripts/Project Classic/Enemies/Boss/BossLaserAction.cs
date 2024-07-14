using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAction : EnemyFireProjectileAction
{
    [SerializeField] float actionTime = 3f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] int laserDamage = 1;
    [SerializeField] public float rotationSpeed = 1f;
    [SerializeField] float laserStartUpTime = 2f;
    [SerializeField] BossAnimationController animationController;
    [SerializeField] Transform laserStartPosition;

    // These are the number of explosive projectiles to spawn
    [SerializeField] int numProjectilesToSpawn = 5;

    PlayerHealth player;
    Coroutine attackCoroutine;
    GameObject laserInstance;

    private void Awake()
    {
        player = FindObjectOfType<PlayerHealth>();
        laserPrefab.GetComponent<Laser>().damage = 1;
    }

    public override void Act()
    {
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        laserInstance = Instantiate(laserPrefab, laserStartPosition.position, Quaternion.identity);
        float startTime = Time.time;
        float idealLaserAngle = GetIdealLaserAngle();

        // Start with laser not damaging
        laserInstance.GetComponent<Laser>().damagesPlayer = false;
        laserInstance.GetComponentInChildren<SpriteRenderer>().color = Color.black;

        laserInstance.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * idealLaserAngle);
        laserInstance.transform.localScale = new Vector3(50, 1, 1);

        animationController.Aim = true;
        animationController.AimVal = animationController.RadiansToBossAimAngle(idealLaserAngle);

        yield return new WaitForSeconds(laserStartUpTime);

        laserInstance.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        laserInstance.GetComponent<Laser>().damagesPlayer = true;
        while (Time.time - startTime < actionTime)
        {
            float currentLaserRadians = Mathf.Deg2Rad * laserInstance.transform.rotation.eulerAngles.z;
            float currentPlayerRadians = GetIdealLaserAngle();

            // Find the right angle to rotate
            float angleDifference = currentPlayerRadians - currentLaserRadians;
            // Rotate Counter Clockwise
            if (angleDifference > 0 && angleDifference < Mathf.PI || angleDifference < 0 &&  angleDifference < -Mathf.PI)
            {
                laserInstance.GetComponent<Rigidbody2D>().angularVelocity = rotationSpeed;
            }
            else
            {
                laserInstance.GetComponent<Rigidbody2D>().angularVelocity = -rotationSpeed;
            }

            yield return new WaitForFixedUpdate();
        }

        // Now spawn projectiles which will trigger explosions
        Vector3 laserDirection = (laserInstance.GetComponent<BoxCollider2D>().bounds.center - laserInstance.transform.position).normalized;
        float explosionIncrement = 50 / (numProjectilesToSpawn + 1);
        for (int i=0; i<numProjectilesToSpawn; ++i)
        {
            Vector3 spawnPosition = laserInstance.transform.position + laserDirection * (i + 1) * explosionIncrement;
            // Spawn each explosion on the projectile data
            for (int j=0; j<projectileData.explosionPrefabs.Count; ++j)
            {
                GameObject explosionInstance = GameObject.Instantiate(projectileData.explosionPrefabs[j], spawnPosition, Quaternion.identity);
                float radius = projectileData.explosionRadii[j];
                explosionInstance.transform.localScale = new Vector3(radius, radius, 1);
            }
        }

        Destroy(laserInstance);
        laserInstance = null;
        animationController.Aim = false;

        yield return new WaitForSeconds(cooldownTime);
    }

    float GetIdealLaserAngle()
    {
        Vector2 vectorToPlayer = player.transform.position - firePosition.transform.position;

        // Get the angle for that vector
        float angle;
        if (vectorToPlayer.x == 0)
            angle = Mathf.PI / 2;
        else
        {
            angle = Mathf.Atan(vectorToPlayer.y / vectorToPlayer.x);
            if (angle < 0)
            {
                angle += 2 * Mathf.PI;
            }
        }
            
        if (vectorToPlayer.x > 0)
            return angle;
        else
            return (angle + Mathf.PI) % (Mathf.PI * 2);
    }

    public override float GetActionTime()
    {
        return laserStartUpTime + actionTime + cooldownTime;
    }

    public override void Interrupt()
    {
        StopCoroutine(attackCoroutine);

        Destroy(laserInstance);
        laserInstance = null;
    }
}
