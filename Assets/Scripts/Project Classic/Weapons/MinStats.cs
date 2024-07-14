using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinStats", menuName = "Project Classic/Weapon/Min Stats")]
public class MinStats : ScriptableObject
{
    public int minDamage;

    [Tooltip("The lower the faster since SOWeapon uses fire cooldown")]
    public float minFireRate;

    [Tooltip("Use this for light weapons")]
    public int minManaGain;

    [Tooltip("Use this for heavy weapons")]
    public int minManaDrain;
}
