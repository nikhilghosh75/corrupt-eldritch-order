using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A component that indicates the terrain of this game object can be switched
 * Interacts with audio switches, a feature that only exists in WWise
 * Written by Morgan Elder '23
 */

#if WSOFT_WWISE
namespace WSoft.Audio
{
    public class TerrainSwitchable : MonoBehaviour
    {
        public string currentTerrain;
    }
}
#endif