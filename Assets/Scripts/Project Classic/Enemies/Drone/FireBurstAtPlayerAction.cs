using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBurstAtPlayerAction : FireAtPlayerAction
{
    [SerializeField] float chargeTime = 1f;
    [SerializeField] int numShots = 3;
    [SerializeField] float shotDelay = 0.2f;

    public override void Act()
    {
        StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire()
    {
        yield return new WaitForSeconds(chargeTime);
        for (int i = 0; i < numShots; ++i)
        {
            base.Act();
            yield return new WaitForSeconds(shotDelay);
        }
    }

    public override float GetActionTime()
    {
        return chargeTime + numShots*shotDelay + 1f;
    }
}
