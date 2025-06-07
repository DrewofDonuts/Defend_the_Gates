using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Casts three raycasts from a given origin in a specified direction.
// </summary>
// <param name="origin">The starting point of the raycasts.</param>
// <param name="direction">The direction in which the raycasts will be cast.</param>
// <param name="spacing">The space between each raycast.</param>
// <param name="transform">The transform component of the object casting the raycasts.</param>
// <param name="hits">The list of RaycastHit objects that the raycasts hit.</param>
// <param name="distance">The maximum distance the raycasts can travel.</param>
// <param name="layers">The layers that the raycasts can hit.</param>
// <returns>Returns true if any of the raycasts hit an object, false otherwise.</returns>

namespace Etheral
{
    public class PhysicsUtil
    {
        public static bool ThreeRaycasts(Vector3 origin, Vector3 direction, float spacing, Transform transform,
            out List<RaycastHit> hits, float distance, LayerMask layers, bool debugDraw = false)
        {
            bool centerHitFound = Physics.Raycast(origin, Vector3.down, out var centerHit, distance, layers);
            bool leftHitFound = Physics.Raycast(origin - transform.right * spacing, Vector3.down, out var leftHit,
                distance, layers);
            bool rightHitFound = Physics.Raycast(origin + transform.right * spacing, Vector3.down, out var rightHit,
                distance, layers);

            //add the hits to a list
            hits = new List<RaycastHit> { centerHit, leftHit, rightHit };

            //if any of the raycasts hit an object, add the hit to the list
            bool hitFound = centerHitFound || leftHitFound || rightHitFound;
            
            if(hitFound && debugDraw)
            {
                Debug.DrawLine(origin, centerHit.point, Color.red);
                Debug.DrawLine(origin - transform.right * spacing, leftHit.point, Color.red);
                Debug.DrawLine(origin + transform.right * spacing, rightHit.point, Color.red);
            }

            return hitFound;
        }
    }
}