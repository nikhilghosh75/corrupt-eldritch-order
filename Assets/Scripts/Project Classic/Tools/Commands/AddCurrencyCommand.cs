using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class AddCurrencyCommand : ConsoleCommand
{
    public AddCurrencyCommand()
    {
        commandWord = "addcurrency";
    }
    public override List<string> GetValidArgs()
    {
        return new List<string>() { "permanent", "run" };
    }

    public override bool Process(string[] args)
    {
        if (args.Length != 2)
        {
            Debug.LogError("Requires two arguments");
            return false;
        }

        int amount;
        if (!int.TryParse(args[1], out amount))
        {
            Debug.LogError("arg1 is not an int");
            return false;
        }

        if (args[0] == "permanent")
        {
            RunManager.Instance.AddCurrency(amount, 0);
        }
        else if (args[0] == "run")
        {
            RunManager.Instance.AddCurrency(0, amount);
        }
        else
        {
            Debug.LogError("not a valid type of currency");
            return false;
        }
        return true;
    }
}
