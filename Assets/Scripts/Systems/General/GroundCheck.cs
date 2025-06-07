using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class GroundCheck : MonoBehaviour
    {
        public LayerMask groundLayers;
        public float distanceToGround = 0.2f;
        public Vector3 leftRayOffeset = new Vector3(-.15f, .25f, 0);
        public Vector3 rightRayOffset = new Vector3(.15f, .25f, 0);

        public float boxCastLength = 0.1f;
        public Collider m_Collider;
        public Vector3 direction;
        RaycastHit m_Hit;

        public float m_MaxDistance = 1f;
        bool m_HitDetect;

        public bool IsGrounded(out RaycastHit hit)
        {
            hit = new RaycastHit();

            Ray rayLeft = new Ray(transform.position + leftRayOffeset, Vector3.down);
            Ray rayRight = new Ray(transform.position + rightRayOffset, Vector3.down);
            RaycastHit[] hits = new RaycastHit[10]; // Array to store the hits
            float radius = 0.1f; // Radius of the sphere

            int numHits = Physics.SphereCastNonAlloc(rayLeft, radius, hits, distanceToGround, groundLayers);
            numHits += Physics.SphereCastNonAlloc(rayRight, radius, hits, distanceToGround, groundLayers);

            Debug.DrawRay(rayLeft.origin, rayLeft.direction * distanceToGround, Color.red);

            if (hits.Length > 0)
                hit = hits[0];

            return numHits > 0;
        }

        public float GetDistanceToGround()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayers))
            {
                return hit.distance;
            }
            else
            {
                return Mathf.Infinity;
            }
        }

        // void OnDrawGizmos()
        // {
        //     Gizmos.color = (IsGrounded()) ? Color.red : Color.blue;
        //     Gizmos.DrawWireSphere(transform.position + leftRayOffeset, distanceToGround);
        // }

        void TestGrounded()
        {
            Ray ray = new Ray(transform.position, Vector3.down);

            Debug.DrawRay(ray.origin, ray.direction * distanceToGround, Color.red); // Visualize the ray

            Debug.Log(Physics.Raycast(ray, distanceToGround, groundLayers));
        } // Log the result

        void Update()
        {
            // TestGrounded();
        }
    }
}