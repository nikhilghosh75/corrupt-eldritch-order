using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Command that resets player health
 * Kennedy McCarthy 
 */

namespace WSoft.Tools.Console
{
    public class ResetHealth : ConsoleCommand
    {
        public ResetHealth()
        {
            commandWord = "resethealth";
        }

        public override List<string> GetValidArgs() { return new List<string>(); }

        public override bool Process(string[] args)
        {
            GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playerArray)
            {
                PlayerHealth healthScript = player.GetComponent<PlayerHealth>();
                if (healthScript != null)
                {
                    healthScript.ResetHealth();
                    return true;
                }
            }

            Debug.Log("Health script couldn't be found in player");
            return false;
        }
    }
}
