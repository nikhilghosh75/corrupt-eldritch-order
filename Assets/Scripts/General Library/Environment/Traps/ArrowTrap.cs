using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using WSoft.Audio;
using WSoft.Combat;

/*
 * The arrow trap
 * Written by Brandon Fox '24
 * 
 * User Guide:
 *  1. Place Arrow trap at desired location
 *  2. Set the direction to fire to the desired direction
 *  3. If you want to change the size of the trap, you will have to do that manually
 *  4. For the triggered mode, you will need to grab the GenericTriggerTrap from the temp prefabs
 *      4a. This prefab has a list of traps that it controls. Simply throw what traps you want it to control into it
 *      4b. To adjust the size and location of the trigger, simply move the GenericTrapTrigger and modifiy its size
 *          and the underlying collider will visually scale appropriately
 */

namespace WSoft.Environment.Traps
{
    public class ArrowTrap : BasicTrap
    {
        public float projectileSpeed = 1;
        public GameObject arrowPrefab;
        public GameObject firePoint;
        private Transform firePointTran;
        public AudioEvent OnArrowShoot;
        public Vector2 directionToFire;

        private void Awake()
        {
            if (Type == TrapType.Static)
            {
                Type = TrapType.Dynamic;
            }
            triggered = false;
        }

        private void Start()
        {
            firePointTran = firePoint.transform;
            if (Type == TrapType.Dynamic) { StartCoroutine(DynamicTrap()); }
        }

        public override void ActivateTrap()
        {
            if (Type != TrapType.Triggered) { return; }
            else if (triggered) { return; }
            else { StartCoroutine(TriggerTrap()); }
        }

        public override void Shoot()
        {
            OnArrowShoot.PlayAudio(gameObject);

            GameObject arrow = Instantiate(arrowPrefab, firePointTran.position, firePointTran.rotation);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.AddForce(directionToFire * projectileSpeed, ForceMode2D.Impulse);
        }

    }
}