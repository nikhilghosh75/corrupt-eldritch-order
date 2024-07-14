using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockwaveMode : BossMode
{
    [SerializeField] GameObject shockwavePrefab;

    public override int ModeID => 4;

    Subscription<BossDefeatedEvent> sub;

    public override void OnBehaviorStart()
    {
        sub = EventBus.Subscribe<BossDefeatedEvent>(BossDefeatHandler);

        EnemyFireProjectileAction[] attacks = GetComponents<EnemyFireProjectileAction>();
        foreach (EnemyFireProjectileAction a in attacks)
        {
            if (a.projectileData)
            {
                ProjectileData newProjectileData = Instantiate(a.projectileData);

                newProjectileData.isExplosive = true;
                newProjectileData.explosionPrefabs.Add(shockwavePrefab);
                newProjectileData.explosionRadii.Add(0f);

                a.projectileData = newProjectileData;
            }
        }
    }

    private void OnDestroy()
    {
        if (sub != null)
        {
            EventBus.Unsubscribe<BossDefeatedEvent>(sub);
        }
    }

    void BossDefeatHandler(BossDefeatedEvent e)
    {
        Shockwave[] shockwaves = GameObject.FindObjectsOfType<Shockwave>();
        foreach (Shockwave shockwave in shockwaves)
        {
            Destroy(shockwave.gameObject);
        }
    }
}
