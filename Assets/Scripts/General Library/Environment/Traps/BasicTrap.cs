using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An inheritable parent class for all traps
 * Written by Allie Lavacek and edited by Brandon Fox
 * 
 * User Guide:
 *  1. Static = trap does not change states and is always active
 *  2. Dynamic = trap indefinitely changes states between active and inactive
 *  3. Triggered = trap only activates when triggered by player 
 */
namespace WSoft.Environment.Traps
{
    public class BasicTrap : MonoBehaviour
    {
        public enum TrapType
        {
            Static,
            Dynamic,
            Triggered,
        };

        protected GameObject player;

        [Header("Trap settings")]
        public TrapType Type = TrapType.Static;
        public int damage = 1;
        public float activationDelay = .5f;
        public float durationActive = 1f;
        public bool triggered;

        public void SetPlayer(GameObject playerIn)
        {
            player = playerIn;
        }

        virtual public void ActivateTrap()
        {

        }

        virtual public IEnumerator TriggerTrap()
        {
            triggered = true;
            yield return new WaitForSeconds(activationDelay);
            Shoot();
            yield return new WaitForSeconds(durationActive);
            triggered = false;
        }

        virtual public IEnumerator DynamicTrap()
        {
            while (true)
            {
                triggered = true;
                yield return new WaitForSeconds(activationDelay);
                Shoot();
                yield return new WaitForSeconds(durationActive);
                triggered = false;
            }
        }

        virtual public void Shoot() { }
    }
}