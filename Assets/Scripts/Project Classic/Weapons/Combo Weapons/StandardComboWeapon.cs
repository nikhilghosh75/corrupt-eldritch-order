using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Standard")]
public class StandardComboWeapon : SOComboWeapon
{
    public SOWeapon weapon;

    public Material material;
    public ScriptableRendererData rendererData;

    float comboWeaponFireTime;

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.SetWeapon(playerWeapon.weakWeapon);

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
        comboWeaponFireTime = -1;
        playerWeapon.UpdateWeaponSprite(weapon);

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

    public override void OnFire(PlayerWeapon playerWeapon, SOWeapon firedWeapon)
    {
        if (Time.time > comboWeaponFireTime)
        {
            playerWeapon.FireWeapon(weapon);
            comboWeaponFireTime = Time.time + weapon.fireCooldown;
        }
    }
}
