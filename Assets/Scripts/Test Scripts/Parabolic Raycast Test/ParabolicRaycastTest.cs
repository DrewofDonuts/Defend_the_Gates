using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class ParabolicRaycastTest : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        public float height;
        public int numSteps;
        public LayerMask layerMask;
        [field: SerializeField] public LineRenderer LineRenderer { get; private set; }


        void Update()
        {
            if (ParabolicRaycast())
                Debug.Log("Collision Detected");
            else
            {
                Debug.Log("No Collision Detected");
            }
        }
        
        

        bool ParabolicRaycast()
        {
            Vector3 start = startPoint.position;
            Vector3 end = endPoint.position;
            float distance = Vector3.Distance(start, end);
            float stepSize = distance / numSteps;

            Vector3[] points = new Vector3[numSteps + 1];
            for (int i = 0; i < numSteps; i++)
            {
                float t = (float)i / numSteps;
                Vector3 currentPoint = Vector3.Lerp(start, end, t);
                currentPoint.y += height * (1 - 4 * (t - 0.5f) * (t - 0.5f));

                Vector3 nextPoint = currentPoint + (end - start).normalized * stepSize;
                nextPoint.y += height * (1 - 4 * (t + 1f / numSteps - 0.5f) * (t + 1f / numSteps - 0.5f));

                Debug.DrawLine(currentPoint, nextPoint, Color.red);
                
                Vector3 point = Vector3.Lerp(Vector3.Lerp(startPoint.position, nextPoint, t), Vector3.Lerp(currentPoint, endPoint.position, t), t);
                points[i] = point;

                LineRenderer.positionCount = numSteps;
                LineRenderer.SetPositions(points);
                
                if (Physics.Linecast(currentPoint, nextPoint, out RaycastHit hit, layerMask))
                {
                    Debug.Log(hit.collider.name);
                    return true; // Collision detected
                }
            }
            return false; // No collision detected
        }
    }
}