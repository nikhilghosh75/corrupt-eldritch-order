#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BossBehavior), true)]
public class BossBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BossBehavior bossBehavior = (BossBehavior)target;
        EnemyAction enemyAction = bossBehavior.CurrentAction;

        EditorGUILayout.Space();
        if (enemyAction != null)
            EditorGUILayout.LabelField("Current Action: " + enemyAction.GetType().Name);
        else
            EditorGUILayout.LabelField("Current Action: None");
    }
}
#endif