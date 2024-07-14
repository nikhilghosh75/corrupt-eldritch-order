using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MaxStats", menuName = "Project Classic/Weapon/Max Stats")]
public class MaxStats : ScriptableObject
{
    public int maxDamage;

    [Tooltip("The lower the faster since SOWeapon uses fire cooldown")]
    public float maxFireRate;

    [Tooltip("Use this for light weapons")]
    public int maxManaGain;

    [Tooltip("Use this for heavy weapons")]
    public int maxManaDrain;
}
