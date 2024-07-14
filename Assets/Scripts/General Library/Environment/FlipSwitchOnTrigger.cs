using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Math;

namespace WSoft.Environment
{
    public class FlipSwitchOnTrigger : MonoBehaviour
    {
        [Tooltip("Only triggers objects on these layers.")]
        public LayerMask triggerLayers;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayermaskFunctions.IsInLayerMask(triggerLayers, collision.gameObject.layer))
            {
                if (collision.gameObject.GetComponent<SwitchController>())
                    collision.gameObject.GetComponent<SwitchController>().Flip();
            }
        }
    }
}