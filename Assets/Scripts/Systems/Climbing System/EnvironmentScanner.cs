using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class EnvironmentScanner : MonoBehaviour
    {
        [BoxGroup("Obstacle Detection")]
        [SerializeField] Vector3 forwardRayOffset = new Vector3(0, .25f, 0);

        [BoxGroup("Obstacle Detection")]
        [SerializeField] float forwardRayLength = .8f;

        [BoxGroup("Obstacle Detection")]
        [SerializeField] LayerMask obstacleLayer;

        [BoxGroup("Obstacle Detection")]
        [SerializeField] float heightRayLength = 5f;

        [BoxGroup("Ledge Detection")]
        [SerializeField] LayerMask ledgeLayers;

        [FormerlySerializedAs("ledgeRayLength")]
        [BoxGroup("Ledge Detection")]
        [Min(10)]
        [SerializeField] float ledgeRayHeightLength = 10f;

        [BoxGroup("Ledge Detection")]
        [SerializeField] double ledgeHeightThreshold = .75f;

        [BoxGroup("Ledge Detection")]
        [SerializeField] float distanceToCheckLedge = .15f;

        [BoxGroup("Climbing Detection")]
        [SerializeField] float climbRayLength = 1.5f;

        [BoxGroup("Climbing Detection")]
        [SerializeField] LayerMask climbLayer;

        [BoxGroup("Climbing Detection")]
        [SerializeField] float offsetHeight = 1.5f;

        [BoxGroup("Climbing Detection")]
        [SerializeField] float raySpacing = .15f;

        [BoxGroup("Climbing Detection")]
        [SerializeField] int numberOfRays = 15;

        [BoxGroup("Climbing Down Detection")]
        [SerializeField] float ledgeCheckVerticalOffset = .1f;

        [BoxGroup("Climbing Down Detection")]
        [SerializeField] float ledgeCheckForwardOffset = 2f;

        [BoxGroup("Climbing Down Detection")]
        [SerializeField] float ledgeCheckLength = 3f;
        
        public ObstacleHitData ObstacleCheck()
        {
            var hitData = new ObstacleHitData();

            //Used to check for obstacles in front of the player
            var forwardOrigin = transform.position + forwardRayOffset;

            hitData.forwardHitFound = Physics.Raycast(transform.position + forwardRayOffset, transform.forward,
                out hitData.forwardHit, forwardRayLength, obstacleLayer);

            //Draws the ray in the editor
            Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength,
                (hitData.forwardHitFound) ? Color.red : Color.blue);


            //Used to check for obstacles above the player
            if (hitData.forwardHitFound)
            {
                //heightOrigin is the point where the forward ray hit + heightRaylength
                var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;

                //heightHitFound is true if the ray hits an obstacle
                hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightHit,
                    heightRayLength, obstacleLayer);

                Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength,
                    (hitData.heightHitFound) ? Color.red : Color.white);
            }

            return hitData;
        }

        public bool ClimbLedgeCheck(Vector3 dir, out RaycastHit ledgeHit)
        {
            // Initialize the out parameter with a new RaycastHit
            ledgeHit = new RaycastHit();

            // If the direction vector is zero, return false
            if (dir == Vector3.zero)
            {
                return false;
            }

            var origin = transform.position + Vector3.up * offsetHeight;

            // Define an offset vector
            var spaceBetweenRays = new Vector3(0, raySpacing, 0);

            // Loop to create multiple rays
            for (int i = 0; i < numberOfRays; i++)
            {
                // Draw a debug ray from the origin in the direction of dir
                Debug.DrawRay(origin + spaceBetweenRays * i, dir);

                // Perform a RayCast from the origin in the direction of dir
                if (Physics.Raycast(origin + spaceBetweenRays * i, dir, out RaycastHit hit, climbRayLength, climbLayer))
                {
                    ledgeHit = hit;
                    return true;
                }
            }

            // If no hit was found in the loop, return false
            return false;
        }

        public bool LeapOrSwingCheck(out RaycastHit hit)
        {
            hit = new RaycastHit();
            var origin = transform.position + Vector3.up * .2f;
            
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, 1f, climbLayer))
            {
                hit = hitInfo;
                return true;
            }

            Debug.DrawRay(origin, Vector3.down * 1f, Color.cyan);
            
            return false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, .3f);
        }


        public bool DropLedgeCheck(out RaycastHit ledgeHit)
        {
            ledgeHit = new RaycastHit();
            var origin = transform.position + transform.forward * 1 + Vector3.down * 0.1f;

            //Debug.DrawRay(origin, -transform.forward * 1f, Color.cyan);
            if (Physics.SphereCast(origin, 0.1f, -transform.forward, out RaycastHit hit, 1f + 0.4f, climbLayer))
            {
                ledgeHit = hit;
                return true;
            }

            return false;
        }

        public bool ObstacleLedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
        {
            ledgeData = new LedgeData();

            if (moveDir == Vector3.zero)
                return false;

            //TODO - additional ledge detection to not fall off ledges, so that obstacle ledges are only for ledge actions. Simply need another raycast
            //to check ledgeLayers, and return ledgeDetected to just obstacle layers

            //Ray to detect if there is a ledge in front of the player
            var origin = transform.position + moveDir * distanceToCheckLedge + Vector3.up;

            if (PhysicsUtil.ThreeRaycasts(origin, Vector3.down, .25f, transform, out List<RaycastHit> hits,
                    ledgeRayHeightLength, ledgeLayers, true))
            {
                //Filter out the hits that are below the ledgeHeightThreshold
                var validHits = hits.Where(h => transform.position.y - h.point.y > ledgeHeightThreshold).ToList();

                if (validHits.Count > 0)
                {
                    //Whatever ray is hitting the ledge, we want to use that point as the origin
                    var surfaceOrigin = validHits[0].point;

                    //Setting y value just below the player's position
                    surfaceOrigin.y = transform.position.y - 0.1f;


                    Debug.DrawRay(surfaceOrigin, -moveDir * 2, Color.yellow);

                    //Calculate the height of the ledge by subtracting the hit point from the player's position
                    var height = transform.position.y - validHits[0].point.y;
                    Debug.Log($"Height: {height}");

                    //Ray to get normal of the ledge and get LedgeData
                    if (Physics.Raycast(surfaceOrigin, transform.position - surfaceOrigin, out RaycastHit surfaceHit, 1,
                            obstacleLayer))
                    {
                        Debug.DrawLine(surfaceOrigin, transform.position, Color.cyan);

                        ledgeData.height = height;
                        ledgeData.angle = Vector3.Angle(transform.forward, surfaceHit.normal);
                        ledgeData.surfaceHit = surfaceHit;
                    }
                    return true;
                }
            }
            return false;
        }
    }

    public struct LedgeData
    {
        public float height;
        public float angle;
        public RaycastHit surfaceHit;
    }

    public struct ObstacleHitData
    {
        public bool forwardHitFound;
        public bool heightHitFound;
        public RaycastHit forwardHit;
        public RaycastHit heightHit;
    }
}