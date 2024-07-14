using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Powerup Database", menuName = "Powerup Database")]
public class PowerupDatabase : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public GameObject prefab;
        public string name;
    }

    public List<Data> powerups;
}
