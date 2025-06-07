using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class EtheralSpline : MonoBehaviour
    {
        public List<Transform> splineTransforms;
        public List<Vector3> splinePoints;
        int splineCount;
        
        
        public Vector3 WhereOnSpline(Vector3 position)
        {
            var closestPoint = Vector3.zero;
            var closestDistance = Mathf.Infinity;

            for (float t = 0; t <= 1; t += 0.01f)
            {
                var interpolatedPoint = GetInterpolatedPointOnSpline(t);
                var distance = Vector3.Distance(position, interpolatedPoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = interpolatedPoint;
                }
            }

            Debug.Log(closestPoint);

            return closestPoint;
        }


        public Vector3 GetInterpolatedPointOnSpline(float t)
        {
            // Ensure t is within the valid range
            t = Mathf.Clamp01(t);

            // Calculate the total number of segments
            int totalSegments = splinePoints.Count - 1;

            // Calculate the exact segment where the interpolated point should be
            float exactSegment = totalSegments * t;

            // Calculate the index of the segment
            int segmentIndex = Mathf.FloorToInt(exactSegment);

            // Calculate the t value within the segment
            float tWithinSegment = exactSegment - segmentIndex;

            // Ensure the segment index is within the valid range
            segmentIndex = Mathf.Clamp(segmentIndex, 0, totalSegments - 1);

            // Interpolate between the two points of the segment
            Vector3 interpolatedPoint =
                Vector3.Lerp(splinePoints[segmentIndex], splinePoints[segmentIndex + 1], tWithinSegment);

            return interpolatedPoint;
        }


#if UNITY_EDITOR

        [Button("Add Spline Points", ButtonSizes.Large), GUIColor(1f, .25f, .25f)]
        void AddSplinePoints()
        {
            splineTransforms.Clear();
            splinePoints.Clear();

            var transformsArray = GetComponentsInChildren<Transform>();

            foreach (var transformItem in transformsArray)
            {
                if (transformItem == transform) continue;
                splineTransforms.Add(transformItem);
            }

            foreach (var _splineTransform in splineTransforms)
            {
                splinePoints.Add(_splineTransform.position);
            }
        }

        [Button("Disable Mesh Renderers", ButtonSizes.Large), GUIColor(1f, .25f, .25f)]
        void DisableMeshRenderers()
        {
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }


        void OnDrawGizmos()
        {
            for (int i = 0; i < splinePoints.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(splinePoints[i], 0.3f);

                if (i < splinePoints.Count - 1)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(splinePoints[i], splinePoints[i + 1]);
                }
            }
        }


#endif
    }
}