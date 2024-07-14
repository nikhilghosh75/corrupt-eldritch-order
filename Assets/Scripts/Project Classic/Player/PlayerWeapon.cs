using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using WSoft.Audio;

class ProjectileShotEvent
{
    public ProjectileShotEvent() { }
}

public class WeaponFiredEvent
{
    public SOWeapon weapon;

    public WeaponFiredEvent(SOWeapon _weapon)
    {
        weapon = _weapon;
    }
}

public class ComboWeaponStartedEvent
{

}

public class ComboWeaponStoppedEvent
{

}

public enum ProjectileType
{
    Basic,
    Arrow,
}

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    public bool generateManaOnFire = true;
    [SerializeField] [Range(0f, 1f)]
    float velocityTransferFraction = 0.0f;
    [SerializeField]
    public PlayerController playerController;
    public SOWeapon weakWeapon;
    public SOWeapon strongWeapon;
    public SOComboWeapon comboWeapon;
    public float weakAttackTimer, strongAttackTimer, comboAttackTimer;
    public GameObject weaponBone; // solely used for visual weapon swapping currently
    private GameObject currentWeak, currentStrong, currentCombo, current;
    bool isInComboState = false;

    [SerializeField]
    public SpriteRenderer sr;

    [SerializeField]
    Transform firePoint;

    ProjectilePool projectilePool;

    public float damageMultiplier = 1f; //used for damageup powerup and class damage modifier currently; could potentially be adapted for other usages as well.
    public float manaMultiplier = 1f; //used for class mana gain modifier currently
    public float bulletSizeMultiplier = 1f; //used for bullet size powerup currently
    public float bulletSpeedMultiplier = 1f; //used for bullet speed powerup currently
    public int numExtraPierces = 0; //used for pierce powerup currently
    public float cooldownMultiplier = 1f;
    public bool addRicochet = false; //used for ricochet powerup currently

    public List<WeaponMod> strongWeaponMods;
    public List<WeaponMod> weakWeaponMods;

    [SerializeField]
    public WeaponDatabase weaponDatabase;

    public AudioEvent insufficientMana;

    private WSoft.Camera.CameraShake shaker;
    private WSoft.Camera.CameraShakeSettings shakerSettings;
    public float camShakeDuration = 1f, camShakeAmplitude = 1f, camShakeFrequency = 1f;
    public Vector3 camPivotOffset;

    private void Awake()
    {
        sr.sprite = weakWeapon.weaponSprite;
        weaponDatabase.Init();
        
        SetShakeSettings();
        EventBus.Subscribe<WeaponPowerupEvent>(OnGainWeapon);
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthChanged);
    }

    private void Start()
    {
        projectilePool = ProjectilePool.Instance;

        // Allow for fallbacks if something didn't get set
        if (RunManager.Instance.initialLightWeapon != null)
        {
            SetWeapon(RunManager.Instance.initialLightWeapon);
        }
        if (RunManager.Instance.initialStrongWeapon != null)
        {
            SetWeapon(RunManager.Instance.initialStrongWeapon);
        }
    }

    private void Update()
    {
        if (weakAttackTimer > 0)
            weakAttackTimer -= Time.deltaTime;

        if (strongAttackTimer > 0)
            strongAttackTimer -= Time.deltaTime;

        if (comboAttackTimer > 0)
            comboAttackTimer -= Time.deltaTime;

        if (isInComboState)
            comboWeapon.OnUpdate(this);
    }

    public void WeakAttack()
    {
        if (weakWeapon == null) return;

        if (isInComboState)
        {
            comboWeapon.OnFire(this, weakWeapon);
            return;
        }

        if (weakAttackTimer <= 0)
        {
            weakAttackTimer = weakWeapon.fireCooldown / cooldownMultiplier;
            if(generateManaOnFire)
            {
                EventBus.Publish<GenerateManaEvent>(new GenerateManaEvent(weakWeapon.manaRecovered));
            }
            FireWeapon(weakWeapon);
        }
    }

    public void StrongAttack()
    {
        if (strongWeapon == null) return;

        if (isInComboState)
        {
            comboWeapon.OnFire(this, strongWeapon);
            return;
        }

        if (strongAttackTimer <= 0 && playerController.playerMana.currentMana >= strongWeapon.manaConsumed)
        {
            strongAttackTimer = strongWeapon.fireCooldown / cooldownMultiplier;
            playerController.playerMana.currentMana = Mathf.Clamp(playerController.playerMana.currentMana - strongWeapon.manaConsumed, 0, playerController.playerMana.maxMana);
            FireWeapon(strongWeapon);
        }

        else if (playerController.playerMana.currentMana < strongWeapon.manaConsumed)
        {
            insufficientMana.PlayAudio(gameObject);
        }
    }

    public void ComboAttack()
    {
        if (comboWeapon == null) return;

        if (playerController.playerMana.currentMana >= 99)
        {
            StartCoroutine(HandleComboWeapon());
            
            isInComboState = true;
        }
        else
        {
            insufficientMana.PlayAudio(gameObject);
        }
    }

    IEnumerator HandleComboWeapon()
    {
        if (isInComboState)
        {
            yield break;
        }

        isInComboState = true;
        comboWeapon.StartComboWeapon(this);
        playerController.playerMana.StartManaDrain(comboWeapon.drainRate);
        EventBus.Publish(new ComboWeaponStartedEvent());

        while (playerController.playerMana.currentMana >= 0)
        {
            yield return null;
        }

        comboWeapon.EndComboWeapon(this);
        isInComboState = false;
        EventBus.Publish(new ComboWeaponStoppedEvent());
    }

    public void FireWeapon(SOWeapon weapon)
    {
        EventBus.Publish(new WeaponFiredEvent(weapon));

        GameObject spawnedBullet = null;

        switch (weapon.weapon)
        {
            case Weapon.Basic:
                spawnedBullet = projectilePool.SpawnProjectile<SimpleProjectile>(firePoint.position, Quaternion.identity);
                break;

            case Weapon.MiniMachineGun:
                currentWeak = weaponBone.transform.Find("LMG").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                /*
                weaponBone.transform.Find("LMG").gameObject.SetActive(true); //this is awful, I'll refactor it when more weapon sprites are implemented on the character model
                weaponBone.transform.Find("Revolver").gameObject.SetActive(false);
                weaponBone.transform.Find("ManaExtractor").gameObject.SetActive(false);
                weaponBone.transform.Find("FlameThrower").gameObject.SetActive(false);
                */
                spawnedBullet = projectilePool.SpawnProjectile<minimachinegunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.Revolver:
                currentWeak = weaponBone.transform.Find("Revolver").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<revolverProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.ManaExtractor:
                currentWeak = weaponBone.transform.Find("ManaExtractor").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<manaextractorProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;

            case Weapon.Sniper:
                currentStrong = weaponBone.transform.Find("Sniper").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<sniperProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.Flamethrower:
                currentStrong = weaponBone.transform.Find("FlameThrower").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<flamethrowerProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.Explosive:
                currentStrong = weaponBone.transform.Find("Explosive").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<explosiveProjectile>(firePoint.position, Quaternion.identity);
                break;

            case Weapon.LaserShotgun:
                currentCombo = weaponBone.transform.Find("LaserShotgun").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<lasershotgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, -weapon.shotgunSpread*2f);
                spawnedBullet = projectilePool.SpawnProjectile<lasershotgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, weapon.shotgunSpread*2f);
                spawnedBullet = projectilePool.SpawnProjectile<lasershotgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, -weapon.shotgunSpread);
                spawnedBullet = projectilePool.SpawnProjectile<lasershotgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, weapon.shotgunSpread);
                spawnedBullet = projectilePool.SpawnProjectile<lasershotgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.ImpactGun:
                currentCombo = weaponBone.transform.Find("Impact").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<impactgunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.BouncingBlades:
                currentCombo = weaponBone.transform.Find("BouncingBlade").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<bouncingbladesProjectile>(firePoint.position, Quaternion.identity);
                break;
            case Weapon.TripleExplosiveMachineGun:
                currentCombo = weaponBone.transform.Find("TEMG").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<tripleexplosivemachinegunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, -weapon.shotgunSpread);
                spawnedBullet = projectilePool.SpawnProjectile<tripleexplosivemachinegunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                SpawnProjectile(spawnedBullet, weapon, weapon.shotgunSpread);
                spawnedBullet = projectilePool.SpawnProjectile<tripleexplosivemachinegunProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.SuperSniper:
                currentCombo = weaponBone.transform.Find("SuperSniper").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                spawnedBullet = projectilePool.SpawnProjectile<supersniperProjectile>(current.transform.GetChild(0).position, Quaternion.identity);
                break;
            case Weapon.ChainLightning:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                for (int i = 0; i < weapon.bulletsFired; i++){
                    spawnedBullet = projectilePool.SpawnProjectile<chainlightningProjectile>(firePoint.position, Quaternion.identity);
                    SpawnProjectile(spawnedBullet, weapon, weapon.shotgunSpread * i);
                }
                break;
            case Weapon.Heal:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                playerController.playerHealth.Heal(1);
                break;
            case Weapon.ManaBurst:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                spawnedBullet = projectilePool.SpawnProjectile<manaburstProjectile>(firePoint.position, Quaternion.identity);
                break;
            case Weapon.RageAura:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                for (int i = 0; i < weapon.bulletsFired; i++){
                    spawnedBullet = projectilePool.SpawnProjectile<chainlightningProjectile>(firePoint.position, Quaternion.identity);
                    SpawnProjectile(spawnedBullet, weapon, weapon.shotgunSpread * i);
                }
                break;

        }

        SpawnProjectile(spawnedBullet, weapon, 0);

        if (SettingsLoader.Settings.screenshakeEnabled)
        {
            shaker.Shake(shakerSettings); // camera will shake upon firing weapon
        }
    }

    void SpawnProjectile(GameObject spawnedBullet, SOWeapon weapon, float offsetAngle)
    {
        if (spawnedBullet != null)
        {
            EventBus.Publish(new ProjectileShotEvent());

            Projectile projectile = spawnedBullet.GetComponent<Projectile>();
            projectile.belongsToPlayer = true;

            projectile.damage = (int)(damageMultiplier * weapon.damage); //apply damage multiplier to each projectile; the multiplier needs to be a float for class purposes
            projectile.manaGenerationAmount = weapon.manaRecovered; // Add weapon-specific base mana generation (is zero for heavy and combo weapons)
            projectile.manaGenerationModifier *= manaMultiplier; //apply mana gain multiplier to each projectile, typecasting handled in projectile script
            projectile.transform.localScale *= bulletSizeMultiplier; //apply bullet size multiplier to each projectile
            projectile.timeUntilDestroy = weapon.timeUntilDestroy;
            projectile.piercesLeft = weapon.pierce + numExtraPierces;

            if (addRicochet)
            {
                IsBouncing placeholder; //if there's a way to do the following line w/ no out var, then this is unneeded
                if (!projectile.gameObject.TryGetComponent<IsBouncing>(out placeholder))
                {
                    IsBouncing isBouncing = projectile.AddComponent<IsBouncing>();
                    isBouncing.bounceByDef = false;
                    isBouncing.numBounces = 1;
                }
            }

            ApplyWeaponMods(projectile, weapon);

            SetBulletDirection(spawnedBullet, weapon, offsetAngle);
        }
    }

    void SetBulletDirection(GameObject spawnedBullet, SOWeapon weapon, float offsetAngle)
    {
        var bulletRb = spawnedBullet.GetComponent<Rigidbody2D>();

        if (playerController.aimDirection != Vector2.zero)
        {
            /* if (playerController.aimDirection.x * playerController.transform.localScale.x < 0) // check if the direction the player is facing is different from aim direction
                playerController.aimDirection.x *= -1; // if it is different, then shoot in the direction the player is facing rather than the "true" aim direction; y value same. */

            float randomAngle = Random.Range(-weapon.bulletSpread / 2f, weapon.bulletSpread / 2f);
            Vector2 randomOffset = Quaternion.Euler(0f, 0f, randomAngle + offsetAngle) * playerController.aimDirection;

            bulletRb.velocity = new Vector2(playerController.rb.velocity.x, 0) * velocityTransferFraction + weapon.bulletSpeed * randomOffset * bulletSpeedMultiplier;

            Vector2 rotation = Vector2.zero - randomOffset;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            spawnedBullet.transform.rotation = Quaternion.Euler(0f, 0f, rot + 180);
        }
        else
        {
            float randomAngle = Random.Range(-weapon.bulletSpread / 2f, weapon.bulletSpread / 2f);
            Vector2 randomOffset = Quaternion.Euler(0f, 0f, randomAngle + offsetAngle) * new Vector2(playerController.transform.localScale.x, 0).normalized;

            bulletRb.velocity = new Vector2(playerController.rb.velocity.x, 0) * velocityTransferFraction + weapon.bulletSpeed * randomOffset * bulletSpeedMultiplier;

            Vector2 rotation = Vector2.zero - randomOffset;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            spawnedBullet.transform.rotation = Quaternion.Euler(0f, 0f, rot + 180);
        }
    }

    void ApplyWeaponMods(Projectile projectile, SOWeapon weapon)
    {
        if (weapon.type == WeaponType.Weak)
        {
            foreach(WeaponMod mod in weakWeaponMods)
            {
                mod.Apply(projectile);
            }
        }
        else if (weapon.type == WeaponType.Strong)
        {
            foreach (WeaponMod mod in strongWeaponMods)
            {
                mod.Apply(projectile);
            }
        }

        if (isInComboState)
        {
            comboWeapon.OnProjectileSpawn(projectile);
        }
    }

    void OnGainWeapon(WeaponPowerupEvent e)
    {
        comboWeapon = GetComboWeapon(weakWeapon, strongWeapon);
    }

    void OnHealthChanged(UpdateHealthEvent e)
    {
        if (e.health + e.healthDelta <= 0)
        {
            if (isInComboState)
            {
                comboWeapon.EndComboWeapon(this);
            }
        }
    }

    SOComboWeapon GetComboWeapon(SOWeapon weakWeapon, SOWeapon strongWeapon)
    {
        if (weaponDatabase != null)
        {
            return weaponDatabase.GetComboWeapon(weakWeapon, strongWeapon);
        }
        else
        {
            Debug.LogWarning("WeaponDatabase not found.");
            return null;
        }
    }

    public void LoadWeapons(WeaponSaveData[] weapons)
    {
        foreach(WeaponSaveData weaponData in weapons)
        {
            SOWeapon weapon = weaponDatabase.weapons.Find(w => w.weapon == weaponData.weapon).soweapon;
            if (weapon != null)
            {
                SetWeapon(weapon);
                AddMods(weapon.type, weaponData.modNames);
            }
        }
    }

    public void SetWeapon(SOWeapon weapon)
    {
        switch (weapon.type)
        {
            case WeaponType.Weak:
                if (weakWeapon != null)
                {
                    UnequipWeakWeapon();
                }

                weakWeapon = weapon;
                break;

            case WeaponType.Strong:
                if (strongWeapon != null)
                {
                    UnequipStrongWeapon();
                }

                strongWeapon = weapon;
                break;
        }

        comboWeapon = GetComboWeapon(weakWeapon, strongWeapon);
    }

    public void AddMods(WeaponType weaponType, string[] modNames)
    {
        ModDatabase database = Resources.Load<ModDatabase>("All Mods");
        foreach (string modName in modNames)
        {
            WeaponMod mod = database.mods.Find(mod => mod.displayName == modName);
            AddMod(weaponType, mod);
        }
    }

    public void AddMod(WeaponType weaponType, WeaponMod mod)
    {
        if (weaponType == WeaponType.Weak)
        {
            weakWeaponMods.Add(mod);
            mod.OnModEquipped();
        }
        else if (weaponType == WeaponType.Strong)
        {
            strongWeaponMods.Add(mod);
            mod.OnModEquipped();
        }
    }

    void UnequipWeakWeapon()
    {
        foreach (WeaponMod mod in weakWeaponMods)
            mod.OnModUnequipped();

        weakWeaponMods.Clear();
    }

    void UnequipStrongWeapon()
    {
        foreach (WeaponMod mod in strongWeaponMods)
            mod.OnModUnequipped();

        strongWeaponMods.Clear();
    }

    public void UpdateWeaponSprite(SOWeapon weapon)
    {
        switch (weapon.weapon)
        {
            case Weapon.MiniMachineGun:
                currentWeak = weaponBone.transform.Find("LMG").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                break;
            case Weapon.Revolver:
                currentWeak = weaponBone.transform.Find("Revolver").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                break;
            case Weapon.ManaExtractor:
                currentWeak = weaponBone.transform.Find("ManaExtractor").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentWeak;
                current.SetActive(true);
                break;

            case Weapon.Sniper:
                currentStrong = weaponBone.transform.Find("Sniper").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                break;
            case Weapon.Flamethrower:
                currentStrong = weaponBone.transform.Find("FlameThrower").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                break;
            case Weapon.Explosive:
                currentStrong = weaponBone.transform.Find("Explosive").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentStrong;
                current.SetActive(true);
                break;

            case Weapon.LaserShotgun:
                currentCombo = weaponBone.transform.Find("LaserShotgun").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                break;
            case Weapon.ImpactGun:
                currentCombo = weaponBone.transform.Find("Impact").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                break;
            case Weapon.BouncingBlades:
                currentCombo = weaponBone.transform.Find("BouncingBlade").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                break;
            case Weapon.TripleExplosiveMachineGun:
                currentCombo = weaponBone.transform.Find("TEMG").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                break;
            case Weapon.SuperSniper:
                currentCombo = weaponBone.transform.Find("SuperSniper").gameObject;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                current.SetActive(true);
                break;
            case Weapon.ChainLightning:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                break;
            case Weapon.Heal:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                break;
            case Weapon.ManaBurst:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                break;
            case Weapon.RageAura:
                currentCombo = null;
                if (current != null)
                    current.SetActive(false);
                current = currentCombo;
                break;

        }
    }
    private void SetShakeSettings()
    {
        transform.parent.TryGetComponent<WSoft.Camera.CameraShake>(out shaker);
        shakerSettings.shakeDuration = camShakeDuration;
        shakerSettings.shakeAmplitude = camShakeAmplitude;
        shakerSettings.shakeFrequency = camShakeFrequency;
        shakerSettings.pivotOffset = camPivotOffset;
    }
}
