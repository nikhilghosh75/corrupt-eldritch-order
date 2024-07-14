using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMod : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public virtual void Apply(Projectile projectile)
    {

    }

    public virtual void OnModEquipped()
    {

    }

    public virtual void OnModUnequipped()
    {

    }
}
