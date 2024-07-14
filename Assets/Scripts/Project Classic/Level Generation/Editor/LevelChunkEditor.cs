#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelChunk))]
public class LevelChunkEditor : Editor
{
    LevelChunk chunk;

    void OnSceneGUI()
    {
        chunk = (LevelChunk)target;

        DrawStartPoint();
        DrawEndPoint();
    }

    void DrawStartPoint()
    {
        Handles.color = Color.white;
        Vector2 newPosition = Handles.FreeMoveHandle(chunk.spawnPoint,0.4f, Vector2.zero, Handles.CylinderHandleCap);

        if (Vector2.SqrMagnitude(newPosition - (Vector2)chunk.spawnPoint) > 0.01f)
        {
            chunk.spawnPoint = newPosition;
        }
    }

    void DrawEndPoint()
    {
        Handles.color = Color.red;
        Vector2 newPosition = Handles.FreeMoveHandle(chunk.endPoint, 0.4f, Vector2.zero, Handles.CylinderHandleCap);

        if (Vector2.SqrMagnitude(newPosition - (Vector2)chunk.endPoint) > 0.01f)
        {
            chunk.endPoint = newPosition;
        }
    }
}
#endif
