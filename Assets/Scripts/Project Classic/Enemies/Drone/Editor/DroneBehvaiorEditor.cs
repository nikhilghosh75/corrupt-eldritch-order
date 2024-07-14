using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DroneBehavior))]
public class DroneBehvaiorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DroneBehavior bossBehavior = (DroneBehavior)target;
        EnemyAction enemyAction = bossBehavior.CurrentAction;

        EditorGUILayout.Space();
        if (enemyAction != null)
            EditorGUILayout.LabelField("Current Action: " + enemyAction.GetType().Name);
        else
            EditorGUILayout.LabelField("Current Action: None");
    }
}
