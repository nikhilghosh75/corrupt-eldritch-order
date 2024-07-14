using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    /// <summary>
    /// A scriptable object whose sole purpose is to store a list of strings.
    /// 
    /// Nikhil Ghosh '24
    /// </summary>
    [CreateAssetMenu(fileName = "New String Table", menuName = "WSoft/String Table", order = 1)]
    public class StringTable : ScriptableObject, IEnumerable<string>
    {
        public List<string> data;

        public IEnumerator<string> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();

        public string RandomEntry()
        {
            int randIndex = Random.Range(0, data.Count);
            return data[randIndex];
        }
    }

}