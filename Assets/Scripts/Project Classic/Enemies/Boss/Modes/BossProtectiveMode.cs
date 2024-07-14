using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProtectiveMode : BossMode
{
    // Reference to the shield on this object
    [SerializeField] GameObject shieldPrefab;

    GameObject shieldInstance;

    public override int ModeID{ get { return 1; } }

    public override void OnBehaviorStart()
    {
        // Make sure to set shield to max health
        shieldInstance = GameObject.Instantiate(shieldPrefab);
        shieldInstance.GetComponent<BossShield>().boss = GetComponent<BossBehavior>();
    }

    public override void OnPostAttack()
    {
        // Make Boss set shield active and reset its health if shield still exists
        if (shieldInstance)
        {
            shieldInstance.SetActive(true);
            shieldInstance.GetComponent<BossShield>().SetShieldActive();
            shieldInstance.GetComponent<EnemyHealth>().SetHealth(shieldInstance.GetComponent<EnemyHealth>().maxHealth);
        }
        // Else create a new shield
        else
        {
            shieldInstance = GameObject.Instantiate(shieldPrefab);
            shieldInstance.GetComponent<BossShield>().SetShieldActive();
            shieldInstance.GetComponent<EnemyHealth>().SetHealth(shieldInstance.GetComponent<EnemyHealth>().maxHealth);
        }
    }
}
