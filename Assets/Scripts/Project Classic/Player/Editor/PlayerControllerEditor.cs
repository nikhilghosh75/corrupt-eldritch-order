#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerSubController subController = ((PlayerController)target).currentController;
        if (subController != null)
        {
            EditorGUILayout.LabelField(subController.name);
        }
    }
}
#endif