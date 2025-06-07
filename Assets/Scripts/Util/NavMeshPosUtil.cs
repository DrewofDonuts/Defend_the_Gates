using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    public static class NavMeshPosUtil
    {
        
        public static Vector3 GetRandomNavMeshPosition(float rectWidth, float rectHeight, Vector3 transform)
        {
            // Vector3 randomDirection = Random.insideUnitSphere * spawnRadius; // Generate a random point in the sphere
            // randomDirection += transform.position; // Offset by the spawner's position


            // Generate a random point within the rectangle
            float randomX = Random.Range(-rectWidth / 2, rectWidth / 2);
            float randomZ = Random.Range(-rectHeight / 2, rectHeight / 2);
            Vector3 randomDirection = new Vector3(randomX, 0, randomZ);

            randomDirection += transform;


            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, Mathf.Max(rectWidth, rectHeight), NavMesh.AllAreas))
            {
                return hit.position; // Return the valid point on the NavMesh
            }

            return Vector3.zero; // Return zero if no valid point is found
        }
    }
}