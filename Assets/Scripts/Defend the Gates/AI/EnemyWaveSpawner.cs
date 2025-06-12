using System.Collections;
using System.Collections.Generic;
using Etheral;
using Etheral.DefendTheGates;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Defend_the_Gates.AI
{
    public class EnemyWaveSpawner : MonoBehaviour
    {
        [Header("Enemy Spawner")]
        int numberOfEnemies;


        [Tooltip("Define the dimensions of the rectangular area")]
        [SerializeField] float rectWidth = 10f;
        [Tooltip("Define the dimensions of the rectangular area")]
        [SerializeField] float rectHeight = 5f;
        
        
        float minSpawnDistance;
        bool spawnOnStart;

        [Header("Debug")]
        [ReadOnly]
        public List<Health> spawnedEnemies = new(); // Track spawned enemies

        List<Vector3> spawnPoints = new(); // Track spawn points
        WaitForSeconds waitTimeBeforeSpawn = new(.15f);

        bool stopSpawning;


        public void SpawnEnemies(int numberToSpawn, EnemySpawnDataObject spawnData )
        {
            if (stopSpawning) return;
            
            StartCoroutine(SpawnEnemyCoroutine(numberToSpawn, spawnData));
        }
        

        IEnumerator SpawnEnemyCoroutine(int numberToSpawn, EnemySpawnDataObject spawnDataObject)
        {
            var enemiesToSpawn = numberToSpawn;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                yield return waitTimeBeforeSpawn;
                SpawnEnemy(spawnDataObject);
            }
        }

        void SpawnEnemy(EnemySpawnDataObject spawnDataObject)
        {
            if (stopSpawning) return;

            Vector3 randomPosition = GetRandomNavMeshPosition();

            if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
            {
                spawnPoints.Add(randomPosition); // Save the spawn point

                var enemyPrefab = GetRandomEnemyPrefab(spawnDataObject);

                // var spawnedEnemy = ObjectPoolManager.Instance.GetObject(enemyPrefab, randomPosition, Quaternion.identity,10);
                var spawnedEnemy = Instantiate(enemyPrefab, randomPosition, transform.rotation);
                spawnedEnemy.SetIsGateAttacking(true);

                if (spawnedEnemy.Health != null)
                {
                    spawnedEnemy.Health.OnDie += HandleSpawnDeath;
                    spawnedEnemies.Add(spawnedEnemy.Health);
                }
            }
        }

        void HandleSpawnDeath(Health obj)
        {
            DeRegisterEvents(obj);
            spawnedEnemies.Remove(obj);
        }

        void DeRegisterEvents(Health enemy)
        {
            enemy.OnDie -= HandleSpawnDeath;
        }

        EnemyStateMachine GetRandomEnemyPrefab(EnemySpawnDataObject enemySpawnSpawnData)
        {
            float totalRatio = 0;
            foreach (var data in enemySpawnSpawnData.EnemySpawnData)
            {
                totalRatio += data.spawnRatio;
            }

            float randomValue = Random.Range(0, totalRatio);
            float cumulativeRatio = 0;

            foreach (var data in enemySpawnSpawnData.EnemySpawnData)
            {
                cumulativeRatio += data.spawnRatio;
                if (randomValue < cumulativeRatio)
                {
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

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (Vector3 point in spawnPoints)
            {
                Gizmos.DrawSphere(point, .15f);
            }

            Gizmos.color = Color.blue;

            // Gizmos.DrawWireSphere(transform.position, spawnRadius);
            Gizmos.DrawWireCube(transform.position, new Vector3(rectWidth, 5, rectHeight));
        }


        Vector3 GetRandomNavMeshPosition()
        {
            return NavMeshPosUtil.GetRandomNavMeshPosition(rectWidth, rectHeight, transform.position);
        }
    }
}