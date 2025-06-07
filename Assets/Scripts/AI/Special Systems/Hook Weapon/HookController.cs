using System;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HookController : MonoBehaviour
{
    public Transform hookBase;
   public Hook hook;
    // public GameObject hook;
    public LineRenderer lineRenderer;
    public Transform startingPosition;


    void Start()
    {
        hook.OnComplete += DisableLineRender;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = false;
    }

    void DisableLineRender()
    {
        lineRenderer.enabled = false;
    }

    void OnDisable()
    {
        hook.OnComplete -= DisableLineRender;
    }


    [Button("Throw Hook")]
    public void ThrowHook()
    {
        hook.EnableProjectile();
        // hook.EnableProjectile(true);
        lineRenderer.enabled = true;
    }


    void Update()
    {
        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        if (hook != null)
        {
            lineRenderer.SetPosition(0, hookBase.position);
            lineRenderer.SetPosition(1, hook.transform.position);
        }
    }
}