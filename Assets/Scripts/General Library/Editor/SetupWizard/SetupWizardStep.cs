#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SetupWizardStep
{
    protected string name = string.Empty;

    public string Name => name;

    public abstract void Do();

    public abstract bool HasBeenDone();
}

#endif