using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeLineRenderer : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] LineRenderer lineRenderer2;

    void Start()
    {
        lineRenderer1.SetPositions(new Vector3[] {
            start.position,
            end.position
        });
        lineRenderer2.SetPositions(new Vector3[] {
            start.position,
            end.position
        });
    }
}
