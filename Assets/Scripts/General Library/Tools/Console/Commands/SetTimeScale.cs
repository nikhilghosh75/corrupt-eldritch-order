using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Tools.Console
{
    public class SetTimeScale : ConsoleCommand
    {
        public SetTimeScale()
        {
            commandWord = "settimescale";
        }

        public override List<string> GetValidArgs() { return new List<string>(); }

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                Debug.LogError("SetTimeScale requires a float as an argument");
                return false;
            }

            float newTimeScale = float.Parse(args[0]);

            // Setting Time.timeScale to be negative can cause unintended results
            if (newTimeScale >= -0.00001f)
            {
                Time.timeScale = newTimeScale;
                return true;
            }

            Debug.LogError("SetTimeScale requires a positive float or 0 as an argument");
            return false;
        }
    }
}