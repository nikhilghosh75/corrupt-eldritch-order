using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnergizedMode : BossMode
{
    [Tooltip("Multiplier applied to boss's speed andcceleration")]
    [SerializeField] float speedMultiplier = 1.2f;
    [Tooltip("Multiplier applied to boss's size")]
    [SerializeField] float sizeMultiplier = 0.8f;
    [Tooltip("Multiplier applied to boss's attack cooldowns")]
    [SerializeField] float cooldownMultiplier = 0.5f;
    [Tooltip("Multiplier applied to projectile speed")]
    [SerializeField] float projectileSpeedMultiplier = 1.5f;

    [SerializeField] GameObject particlePrefab;

    GameObject particleInstance;

    public override int ModeID { get { return 2; } }

    public override void OnBehaviorStart()
    {
        particleInstance = GameObject.Instantiate(particlePrefab, transform);
        particleInstance.SetActive(true);
        
        // Grab components
        BossHoverAction bossHoverAction = GetComponent<BossHoverAction>();
        BossRepositionAction bossRepositionAction = GetComponent<BossRepositionAction>();
        EnemyFireProjectileAction[] attackActions = GetComponents<EnemyFireProjectileAction>();

        // Modify speed on hover and repostion actions
        bossHoverAction.maxSpeed *= speedMultiplier;
        bossHoverAction.constant *= speedMultiplier;
        bossRepositionAction.acceleration *= speedMultiplier;
        bossRepositionAction.maxVelocity *= speedMultiplier;

        // Modify size
        transform.localScale *= sizeMultiplier;

        // Modify cooldowns
        bossHoverAction.hoverTime *= cooldownMultiplier;
        foreach (EnemyFireProjectileAction action in attackActions)
        {
            action.cooldownTime *= cooldownMultiplier;

            // Speed up projectiles speed
            if (action.projectileData)
            {
                ProjectileData newProjectileData = Instantiate(action.projectileData);
                newProjectileData.speed *= projectileSpeedMultiplier;
                action.projectileData = newProjectileData;
            }

            // Speed up laser
            if (action is BossLaserAction)
            {
                (action as BossLaserAction).rotationSpeed *= projectileSpeedMultiplier;
            }

            // Speed up ball of fire
            if (action is BossBallsOfFireAction)
            {
                (action as BossBallsOfFireAction).rotationSpeed *= projectileSpeedMultiplier;
            }
        }
    }
}
