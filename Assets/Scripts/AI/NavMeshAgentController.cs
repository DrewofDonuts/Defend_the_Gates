using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class NavMeshAgentController : MonoBehaviour
    {
        [SerializeField] NavMeshAgent agent;


        public Transform testDestination;


        void Awake()
        {
            DisableAgentUpdate();
        }

        public Vector3 GetSteeringTarget()
        {
            return agent.steeringTarget;
        }

        public void ResetNavAgent()
        {
            // if (!agent.isOnNavMesh) return;

            agent.enabled = true;

            agent.isStopped = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }


        public NavMeshAgent GetAgent() => agent;

        public bool GetIsUpdatePosition()
        {
            //Disabled 05/03/25
            // if (!GetIsOnNavMesh())
            //     ResetNavAgent();
            return agent.updatePosition;
        }

        public bool GetIsEnabled()
        {
            return agent.enabled;
        }

        public bool GetIsOnNavMesh()
        {
            return agent.isOnNavMesh;
        }

        public Vector3 GetDesiredVelocityNormalized()
        {
            //Disabled 05/03/25
            // if (!GetIsOnNavMesh())
            //     ResetNavAgent();
            return agent.desiredVelocity.normalized;
        }

        public void DisableAgentComponent()
        {
            agent.enabled = false;
        }

        public void DisableAgentUpdate()
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }

        public void SetRotation(bool updateRotation)
        {
            if (agent.isOnNavMesh)
                agent.updateRotation = updateRotation;
        }

        public void EnableAgentUpdate()
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
        }

        public void SetIsStopped(bool isStopped)
        {
            if (agent.isOnNavMesh)
                agent.isStopped = isStopped;
        }

        public void SetDestination(Vector3 destination)
        {
            if (agent.isOnNavMesh)
                agent.destination = destination;

            // agent.SetDestination(destination);
        }


        public void SetNextPosition(Vector3 nextPosition)
        {
            if (agent.isOnNavMesh)
                agent.nextPosition = nextPosition;
        }

        public void SetVelocityToKeepInSyncWithCC(Vector3 velocity)
        {
            agent.velocity = velocity;
        }

        public void ResetPath()
        {
            agent.ResetPath();
        }

        public Vector3 GetRandomPointInFront(float distance)
        {
            Vector3 randomDirection = agent.transform.right * distance;
            randomDirection += new Vector3(Random.Range(-distance, distance), 0, Random.Range(-distance, distance));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(agent.transform.position + randomDirection, out hit, distance, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.zero;
        }

        public void SetAgentSpeed(float speed)
        {
            agent.speed = speed;
        }
    }
}