using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class HumanBombController : MonoBehaviour
    {
        [SerializeField] float spawnRadius = 20f; // Radius to spawn within
        [SerializeField] float minSpawnDistance = 2f; // Minimum distance between spawn points

        // [SerializeField] GameObject[] humanBombToSpawn;
        [SerializeField] HumanBomb[] humanBombs;

        Transform lastSpawnLocation;
        List<Vector3> spawnPoints = new(); // Track spawn points

        void Start()
        {
            for (int i = 0; i < humanBombs.Length; i++)
            {
                ObjectPoolManager.Instance.GetPool(humanBombs[i], 10);
            }
        }


        [Button("Spawn Human Bomb")]
        public void SpawnHumanBomb(int count)
        {
            var enemiesToSpawn = count;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();

                if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
                {
                    spawnPoints.Add(randomPosition); // Save the spawn point

                    var randomHumanBomb = humanBombs[Random.Range(0, humanBombs.Length)];

                    // Instantiate(randomHumanBomb, randomPosition, Quaternion.identity);

                    HumanBomb bomb = ObjectPoolManager.Instance.GetObject(randomHumanBomb);
                    bomb.transform.position = randomPosition;
                    bomb.transform.rotation = Quaternion.identity;
                }
            }

            spawnPoints.Clear(); // Clear the list for the next spawn
        }

        bool IsFarFromOtherSpawnPoints(Vector3 position)
        {
            foreach (Vector3 point in spawnPoints)
            {
                if (Vector3.Distance(point, position) < minSpawnDistance)
                {
                    return false; // Position is too close to an existing spawn point
                }
            }

            return true; // Position is valid
        }


        Vector3 GetRandomNavMeshPosition()
        {
            Vector3 randomDirection =
                Random.insideUnitSphere * spawnRadius; // Generate a random point in the sphere
            randomDirection += transform.position; // Offset by the spawner's position

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
            {
                return hit.position; // Return the valid point on the NavMesh
            }

            return Vector3.zero; // Return zero if no valid point is found
        }
    }
}

