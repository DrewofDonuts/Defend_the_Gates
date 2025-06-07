using System;
using UnityEngine;

namespace Etheral
{
    
public class FadeCanvasActivator : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FadeCanvas fadeCanvas))
        {
            Debug.Log($"FadeCanvasActivator: {gameObject.name} triggered by {other.gameObject.name}");
            fadeCanvas.FadeToOne();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FadeCanvas fadeCanvas))
        {
            fadeCanvas.FaadeToZero();
        }
    }
}
}
