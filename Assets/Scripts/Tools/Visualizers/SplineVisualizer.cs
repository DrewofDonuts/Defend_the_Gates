using System;
using UnityEngine;
using UnityEngine.Splines;

public class SplineVisualizer : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int resolution = 50;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawSpline();
    }

 void DrawSpline()
    {
        if (splineContainer == null)
        {
            Debug.LogWarning("SplineContainer not assigned!");
            return;
        }

        Spline spline = splineContainer.Spline;
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.useWorldSpace = true; // <- important!

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 localPos = spline.EvaluatePosition(t);
            Vector3 worldPos = splineContainer.transform.TransformPoint(localPos);
            lineRenderer.SetPosition(i, worldPos);
        }
    }
}