/*
 * This script allows a function call passing in a target
 * gameobject which deals damage to a valid health component.
 * The function can be attached to gameobject passing events.
 * @ Evan Brisita '18
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Combat
{
    public class Damage : MonoBehaviour
    {
        public void DealDamage(GameObject target, int damage)
        {
            //Try to get health from collision
            HealthSystem health = target.GetComponent<HealthSystem>();

            if (health)
            {
                //If a health component was found on the collided-with object, apply damage
                health.Damage(damage);
            }
        }
    }
}