using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAuraMode : BossMode
{
    [SerializeField] GameObject auraPrefab;

    public override int ModeID { get { return 3; } }

    Subscription<BossDefeatedEvent> sub;

    public override void OnBehaviorStart()
    {
        // Subscribe to event
        sub = EventBus.Subscribe<BossDefeatedEvent>(BossDefeatedHandler);

        // Get all attacks on the boss
        EnemyFireProjectileAction[] attackComponents = GetComponents<EnemyFireProjectileAction>();

        // Make all projectiles explosive
        foreach (EnemyFireProjectileAction action in attackComponents)
        {
            if (action.projectileData)
            {
                ProjectileData newProjectileData = Instantiate(action.projectileData);

                newProjectileData.isExplosive = true;
                newProjectileData.explosionPrefabs.Add(auraPrefab);
                newProjectileData.explosionRadii.Add(action.projectileData.scaleX * auraPrefab.transform.localScale.x);

                action.projectileData = newProjectileData;
            }
        }
    }

    public override void OnPostAttack()
    {
        DebufferAura aura = auraPrefab.GetComponent<DebufferAura>();

        // Randomly select the mode to be in
        aura.debufferType = (DebufferAura.DebuffType) Mathf.FloorToInt(Random.Range(0, 4.999f));
    }

    void BossDefeatedHandler(BossDefeatedEvent e)
    {
        DebufferAura[] auras = FindObjectsOfType<DebufferAura>();
        foreach (DebufferAura aura in auras)
        {
            Destroy(aura.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BossDefeatedEvent>(sub);
    }
}
