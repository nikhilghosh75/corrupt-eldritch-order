using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A component that triggers an audio switch when a TerrainSwitchable enters the trigger
 * Interacts with audio switches, a feature that only exists in WWise
 * Written by Morgan Elder '23
 */

#if WSOFT_WWISE
namespace WSoft.Audio
{
    public class TerrainSwitchTrigger2D : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Switch audioSwitch;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TerrainSwitchable switchable = collision.gameObject.GetComponent<TerrainSwitchable>();
            if (switchable != null)
            {
                audioSwitch.SetValue(collision.gameObject);
                switchable.currentTerrain = audioSwitch.Name;
                Debug.Log(collision.gameObject.name + " has been set to " + audioSwitch.Name + " switch by an TerrainSwitchTrigger");
            }
        }
    }
}
#endif