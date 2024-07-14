using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

/*
 * An input processor that enables the input of a certain input to be reversed
 * Note that this unfortunately should not be in a namespace, as it would break a lot of functionality
 * Written by Nikhil Ghosh '24
 */

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class InvertibleProcessor : InputProcessor<Vector2>
{
    public bool invertX;
    public bool invertY;

    public override Vector2 Process(Vector2 value, InputControl control)
    {
        if (invertX)
            value.x = -value.x;
        if (invertY)
            value.y = -value.y;

        return value;
    }

#if UNITY_EDITOR
    static InvertibleProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<InvertibleProcessor>();
    }
}
