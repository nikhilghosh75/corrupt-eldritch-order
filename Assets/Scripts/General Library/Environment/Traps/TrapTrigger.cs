using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Math;

/*
 * A class that handles the triggering the traps
 * Written by Allie Lavacek '24, Brandon Fox '24
 * 
 * User Guide:
        1. Place TrapTrigger script onto a gameobject with a 2d collider set to isTrigger
            i. This trigger will be responsible for activating the basic trap
 */
namespace WSoft.Environment.Traps
{
    public class TrapTrigger : MonoBehaviour
    {
        [Tooltip("Only triggers objects on these layers.")]
        public LayerMask triggerLayers;
        public BasicTrap[] traps;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayermaskFunctions.IsInLayerMask(triggerLayers, collision.gameObject.layer))
            {
                TriggerTraps(collision);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (LayermaskFunctions.IsInLayerMask(triggerLayers, collision.gameObject.layer))
            {
                TriggerTraps(collision);
            }
        }

        void TriggerTraps(Collider2D collision)
        {
            for (int i = 0; i < traps.Length; i++)
            {
                traps[i].SetPlayer(collision.gameObject);
                traps[i].ActivateTrap();
            }
        }
    }
}