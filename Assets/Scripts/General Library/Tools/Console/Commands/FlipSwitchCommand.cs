using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Environment;

namespace WSoft.Tools.Console
{
    public class FlipSwitchCommand : ConsoleCommand
    {
        public FlipSwitchCommand()
        {
            commandWord = "flipswitch";
        }

        public override List<string> GetValidArgs()
        {
            return new List<string>() { "", "all" };
        }

        public override bool Process(string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "all")
                {
                    foreach (SwitchController controller in Object.FindObjectsOfType<SwitchController>())
                        controller.Flip();
                }
            }
            else
            {
                float minDistance = Mathf.Infinity;
                int minIndex = -1;
                Vector3 playerPosition = UnityEngine.Camera.main.transform.position;
                SwitchController[] controllers = Object.FindObjectsOfType<SwitchController>();

                for (int i = 0; i < controllers.Length; i++)
                {
                    float distance = Vector3.Distance(playerPosition, controllers[i].transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minIndex = i;
                    }
                }

                controllers[minIndex].Flip();
            }

            return true;
        }
    }
}