using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Math;

namespace WSoft.Environment
{
    public class FlipSwitchOnCollide : MonoBehaviour
    {
        [Tooltip("Only triggers objects on these layers.")]
        public LayerMask triggerLayers;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (LayermaskFunctions.IsInLayerMask(triggerLayers, collision.gameObject.layer))
            {
                if (collision.gameObject.GetComponent<SwitchController>())
                    collision.gameObject.GetComponent<SwitchController>().Flip();
            }
        }
    }
}