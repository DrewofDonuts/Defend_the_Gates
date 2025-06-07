using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class PatrolController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] NavMeshAgentController agentController;
        [SerializeField] WaypointsHandler waypointsHandler;
        
        [Header("Patrol Radius")]
        [Tooltip("Radius of the patrol area from start position")]
        [SerializeField] float patrolRadius = 10f;
        
        [Header("Times")]
        [FormerlySerializedAs("waitTime")]
        [Tooltip("Time before moving to next waypoint")]
        [SerializeField] float minWaitTime = 1f;
        [SerializeField] float maxWaitTime = 3f;
        
        [Header("Patrol Settings")]
        [SerializeField] bool randomPatrol = true;
        [SerializeField] bool thisIsAPatrollingEnemy;

        Vector3 startPosition;
        Vector3 targetPoint;
        bool waiting;
        bool patrolling; // Patrol state
        bool patrollingWaypoints; // Patrol state
        int currentWaypointIndex = 0;

        public bool GetIfThisIsAPatrollingEnemy() => thisIsAPatrollingEnemy;


        void Start()
        {
            startPosition = transform.position;
            agentController.GetAgent().angularSpeed = 360;
        }

        // Call this method to resume patrolling
        public void StartPatrolling(bool isRandomPatrolOverride = false)
        {
            if (randomPatrol || isRandomPatrolOverride)
            {
                patrolling = true;
                SetNewRandomDestination();
            }
            else if (waypointsHandler != null)
            {
                patrollingWaypoints = true;
                SetNextWaypoint();
            }
        }

        void Update()
        {
            if (!thisIsAPatrollingEnemy) return;

            if (patrolling)
                Patrol();

            if (patrollingWaypoints)
                PatrolWaypoints();
        }

        void Patrol()
        {
            if (!agentController.GetAgent().pathPending && agentController.GetAgent().remainingDistance <= 1f)
            {
                if (!waiting)
                {
                    StartCoroutine(WaitAndMove());
                }
            }
        }

        IEnumerator WaitAndMove()
        {
            waiting = true;
            agentController.SetIsStopped(true);
            var randomWaitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(minWaitTime);
            SetNewRandomDestination();
            waiting = false;
            agentController.SetIsStopped(false);

        }
        
        void SetNewRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += startPosition;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                // Raycast to check ground and slope
                if (Physics.Raycast(hit.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit groundHit, 2f))
                {
                    float groundDistance = groundHit.distance;
                    float slopeAngle = Vector3.Angle(groundHit.normal, Vector3.up);

                    if (groundDistance <= 1f && slopeAngle <= 30f) // Only accept flat-ish surfaces
                    {
                        targetPoint = hit.position;
                        agentController.SetDestination(targetPoint);
                    }
                }
            }
        }



        // void SetNewRandomDestination()
        // {
        //     Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        //     randomDirection += startPosition;
        //
        //     if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        //     {
        //         // Simple safety check: cast a ray slightly ahead of hit.point
        //         if (Physics.Raycast(hit.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit groundHit, 2f))
        //         {
        //             float groundDistance = groundHit.distance;
        //             if (groundDistance <= 1f) // If close to ground, it's safe
        //             {
        //                 targetPoint = hit.position;
        //                 agentController.SetDestination(targetPoint);
        //             }
        //         }
        //     }
        // }


        void SetNextWaypoint()
        {
            if (waypointsHandler.GetWaypoints().Count == 0) return;

            targetPoint = waypointsHandler.GetWaypoints()[currentWaypointIndex].position;
            agentController.SetDestination(targetPoint);

            // Move to next waypoint, loop back to start if at end
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointsHandler.GetWaypoints().Count;
        }

        public Vector3 GetTargetPoint() => targetPoint;

        // Call this method to stop patrolling
        public void StopPatrolling()
        {
            patrolling = false;
            patrollingWaypoints = false;
            agentController.ResetPath(); // Clears the current path to stop movement
        }

        IEnumerator WaitAndMoveToNextWaypoint()
        {
            waiting = true; 
            agentController.SetIsStopped(true);
            var randomWaitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(randomWaitTime);
            SetNextWaypoint();
            waiting = false;
            agentController.SetIsStopped(false);
        }

        void PatrolWaypoints()
        {
            if (!agentController.GetAgent().pathPending && agentController.GetAgent().remainingDistance <= 1f)
            {
                if (!waiting)
                {
                    StartCoroutine(WaitAndMoveToNextWaypoint());
                }
            }
        }


        void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}