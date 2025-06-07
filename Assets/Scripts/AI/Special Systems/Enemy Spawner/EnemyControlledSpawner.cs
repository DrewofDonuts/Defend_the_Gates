using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class EnemyControlledSpawner : MonoBehaviour
    {
        [Header("Enemy Spawner")]
        [SerializeField] List<EnemySpawnData> enemySpawnDataList;
        [SerializeField] int numberOfEnemies = 5; // Number of enemies to spawn
        [SerializeField] float spawnRadius = 20f; // Radius to spawn within
        [SerializeField] float minSpawnDistance = 2f; // Minimum distance between spawn points
        [SerializeField] bool spawnOnStart = true;
        [SerializeField] bool isLine;
        [SerializeField] Vector3 distanceFromSpawner;
        [SerializeField] float delayBetweenSpawns = 0.2f;

        [Header("References")]
        [SerializeField] EnemyGroup enemyGroup;
        [SerializeField] AIHealth aiHealth;
        [SerializeField] SpawnEffects spawnerPrefab;

        public List<EnemyStateMachine> spawnedEnemies { get; private set; } = new();


        List<Vector3> spawnPoints = new(); // Track spawn points

        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            if (spawnOnStart)
                StartCoroutine(ShootSpawner());

            // SpawnEnemies();
        }

        //Spawn enemies over time
        IEnumerator ShootSpawner(int overridEnemiesToSpawn = 0)
        {
            int spawnedCount = 0;

            var enemiesToSpawn = overridEnemiesToSpawn > 0 ? overridEnemiesToSpawn : numberOfEnemies;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();

                if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
                {
                    spawnPoints.Add(randomPosition); // Save the spawn point

                    var enemyPrefab = GetRandomEnemyPrefab();

                    var spawnedShooter = Instantiate(spawnerPrefab, transform.position, transform.rotation);

                    spawnedShooter.Init(enemyPrefab, transform, randomPosition, aiHealth, isLine);

                    if (enemyGroup != null)
                        enemyPrefab.transform.parent = enemyGroup.transform;

                    spawnedCount++;
                }

                if (enemyGroup != null)
                    enemyGroup.AddEnemies();
                yield return new WaitForSeconds(delayBetweenSpawns);
            }

            if (enemyGroup != null)
                enemyGroup.AddEnemies();
        }


        //Use to spawn all spawners at on time
        void ShootAllSpawners(int overridEnemiesToSpawn = 0)
        {
            Debug.Log("Spawning enemies");
            int spawnedCount = 0;
            Debug.Log($"Spawning {spawnedCount} enemies");

            var enemiesToSpawn = overridEnemiesToSpawn > 0 ? overridEnemiesToSpawn : numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();

                if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
                {
                    spawnPoints.Add(randomPosition); // Save the spawn point

                    var enemyPrefab = GetRandomEnemyPrefab();


                    var spawnedShooter = Instantiate(spawnerPrefab, transform.position, transform.rotation);
                    spawnedShooter.Init(enemyPrefab, transform, randomPosition, aiHealth, isLine);

                    if (enemyGroup != null)
                        enemyPrefab.transform.parent = enemyGroup.transform;

                    spawnedCount++;

                    Debug.Log($"Spawned {enemyPrefab} enemies");
                }

                if (enemyGroup != null)
                    enemyGroup.AddEnemies();
            }
        }

        IEnumerator SpawnEnemies(int overridEnemiesToSpawn = 0)
        {
            Debug.Log("Spawning enemies");
            int spawnedCount = 0;
            Debug.Log($"Spawning {spawnedCount} enemies");

            var enemiesToSpawn = overridEnemiesToSpawn > 0 ? overridEnemiesToSpawn : numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();

                if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
                {
                    spawnPoints.Add(randomPosition); // Save the spawn point

                    var enemyPrefab = GetRandomEnemyPrefab();
                    var spawnedEnemy = Instantiate(enemyPrefab, randomPosition, transform.rotation);

                    if (isLine)
                    {
                        spawnedEnemy.GetAIComponents().GetLineController().SetTarget(aiHealth, transform);
                    }
                    
                    if (enemyGroup != null)
                        spawnedEnemy.transform.parent = enemyGroup.transform;

                    spawnedCount++;

                    Debug.Log($"Spawned {enemyPrefab} enemies");
                }

                yield return new WaitForSeconds(delayBetweenSpawns);

                if (enemyGroup != null)
                    enemyGroup.AddEnemies();
            }
        }


        [Button("GetRandomEnemyPrefab")]
        EnemyStateMachine GetRandomEnemyPrefab()
        {
            float totalRatio = 0;
            foreach (var data in enemySpawnDataList)
            {
                totalRatio += data.spawnRatio;
            }

            float randomValue = Random.Range(0, totalRatio);
            float cumulativeRatio = 0;

            foreach (var data in enemySpawnDataList)
            {
                cumulativeRatio += data.spawnRatio;
                if (randomValue < cumulativeRatio)
                {
                    Debug.Log($"Returning {data.enemySO}");
                    return data.enemySO.EnemyStateMachine;
                }
            }

            return null; // Fallback, should not happen if ratios are set correctly
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            foreach (Vector3 point in spawnPoints)
            {
                Gizmos.DrawSphere(point, .15f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        [Button("Get Random Nav Mesh Position")]
        Vector3 GetRandomNavMeshPosition()
        {
            // First, get a random direction on the XZ plane
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

            // Apply the minimum distance and random additional distance
            float totalDistance = distanceFromSpawner.magnitude + Random.Range(0, spawnRadius);
            Vector3 targetPosition = transform.position + (randomDirection * totalDistance);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, spawnRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.zero;
        }

        [ContextMenu("Start Spawner")]
        public void StartSpawner(int spawnOverrideNumber = 0)
        {
            StartCoroutine(ShootSpawner(spawnOverrideNumber));
        }
    }
}