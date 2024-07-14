using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Rage Aura")]
public class RageAuraComboWeapon : SOComboWeapon
{
    public float damageMultiplier;
    public float fireRateMultiplier;
    public float speedMultiplier;

    public Material material;
    public ScriptableRendererData rendererData;

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        PlayerController player = playerWeapon.GetComponent<PlayerController>();

        playerWeapon.damageMultiplier /= damageMultiplier;
        playerWeapon.cooldownMultiplier /= fireRateMultiplier;
        player.groundController.ChangeSpeed(1 / speedMultiplier);

        if (material)
        {
            foreach (ScriptableRendererFeature feature in rendererData.rendererFeatures)
            {
                FullScreenPassRendererFeature fullScreenPass = feature as FullScreenPassRendererFeature;
                if (fullScreenPass)
                {
                    fullScreenPass.passMaterial = null;
                }
            }
        }
    }

    public override void StartComboWeapon(PlayerWeapon playerWeapon)
    {
        PlayerController player = PlayerController.Instance;

        playerWeapon.damageMultiplier *= damageMultiplier;
        playerWeapon.cooldownMultiplier *= fireRateMultiplier;
        player.groundController.ChangeSpeed(speedMultiplier);

        if (material)
        {
            foreach (ScriptableRendererFeature feature in rendererData.rendererFeatures)
            {
                FullScreenPassRendererFeature fullScreenPass = feature as FullScreenPassRendererFeature;
                if (fullScreenPass)
                {
                    fullScreenPass.passMaterial = material;
                }
            }
        }
    }
}
