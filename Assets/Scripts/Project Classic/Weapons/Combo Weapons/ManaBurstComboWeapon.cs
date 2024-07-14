using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.FirstPerson;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Mana Burst")]
public class ManaBurstComboWeapon : SOComboWeapon
{
    class ManaBurstParticle
    {
        public GameObject gameObject;
        public float angle;
        public float targetAngle;
    }

    public float radius;
    public float manaBurstRate;
    public float moveTime;
    public float rotationSpeed;
    public float manaBurstCooldown;
    public float launchSpeed;
    public float manaDrain;
    public int manaBurstDamage;
    public GameObject manaBurstPrefab;
    public LayerMask enemiesLayerMask;
    public LayerMask levelLayerMask;

    Coroutine coroutine;
    float lastManaBurstTime = 0;

    List<ManaBurstParticle> manaBursts = new List<ManaBurstParticle>();

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.StopCoroutine(coroutine);

        foreach(ManaBurstParticle particle in manaBursts)
        {
            Destroy(particle.gameObject);
        }
    }

    public override void StartComboWeapon(PlayerWeapon playerWeapon)
    {
        coroutine = playerWeapon.StartCoroutine(HandleManaBurst());
        lastManaBurstTime = 0;
    }

    public override void OnFire(PlayerWeapon playerWeapon, SOWeapon firedWeapon)
    {
        if (manaBursts.Count == 0)
            return;

        if (Time.time < lastManaBurstTime + manaBurstCooldown)
            return;

        GameObject launchedParticle = manaBursts[0].gameObject;
        float angle = manaBursts[0].angle;

        GameObject enemyToTarget = FindEnemy(angle);
        if (enemyToTarget == null)
            return;

        Vector2 direction = enemyToTarget.transform.position - launchedParticle.transform.position;
        Vector2 velocity = direction * launchSpeed * playerWeapon.bulletSpeedMultiplier;
        launchedParticle.GetComponent<Collider2D>().enabled = true;
        launchedParticle.GetComponent<Rigidbody2D>().velocity = velocity;
        launchedParticle.GetComponent<Projectile>().damage = (int)(manaBurstDamage * playerWeapon.damageMultiplier);

        PlayerController.Instance.playerMana.ConsumeMana(manaDrain);

        manaBursts.RemoveAt(0);
        lastManaBurstTime = Time.time;
    }

    IEnumerator HandleManaBurst()
    {
        manaBursts = new List<ManaBurstParticle>();
        float timeUntilSpawn = -1f;
        while(true)
        {
            timeUntilSpawn -= Time.deltaTime;
            if (timeUntilSpawn <= 0.0f)
            {
                SpawnManaBurst();
                timeUntilSpawn = manaBurstRate;
            }

            int i = 0;
            foreach(ManaBurstParticle manaBurst in manaBursts)
            {
                manaBurst.targetAngle += rotationSpeed * Time.deltaTime;
                manaBurst.angle += rotationSpeed * Time.deltaTime;

                if (manaBurst.targetAngle > 360f)
                    manaBurst.targetAngle -= 360f;
                if (manaBurst.angle > 360f)
                    manaBurst.angle -= 360f;

                if (Mathf.Abs(manaBurst.targetAngle - manaBurst.angle) > 0.5f)
                {
                    manaBurst.angle = Mathf.Lerp(manaBurst.angle, manaBurst.targetAngle, 0.4f);
                }

                float angle = manaBurst.angle * Mathf.Deg2Rad;
                Vector3 playerPosition = PlayerController.Instance.transform.position;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

                manaBurst.gameObject.transform.position = playerPosition + offset;
                i++;
            }

            yield return null;
        }
    }

    void SpawnManaBurst()
    {
        GameObject spawnedMana = Instantiate(manaBurstPrefab);
        spawnedMana.GetComponent<Projectile>().timeUntilDestroy = 99999;
        spawnedMana.GetComponent<Collider2D>().enabled = false;
        
        for(int i = 0; i < manaBursts.Count; i++)
        {
            float newAngle = ((float)i / (manaBursts.Count + 1)) * 360f;
            manaBursts[i].targetAngle = newAngle;
        }

        ManaBurstParticle particle = new ManaBurstParticle();
        particle.gameObject = spawnedMana;
        particle.angle = ((float)manaBursts.Count / (manaBursts.Count + 1)) * 360f;
        particle.targetAngle = particle.angle;

        manaBursts.Add(particle);
    }

    GameObject FindEnemy(float angle)
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerPosition, 25f, enemiesLayerMask);

        foreach(Collider2D enemy in enemies)
        {
            Vector2 enemyPosition = enemy.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(playerPosition, enemyPosition - playerPosition, 25f, levelLayerMask);
            if (hit.collider == null)
            {
                return enemy.gameObject;
            }
        }

        return null;
    }
}
