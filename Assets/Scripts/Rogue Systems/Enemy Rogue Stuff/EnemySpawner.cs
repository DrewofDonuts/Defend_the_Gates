using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class EnemySpawner : MessengerClass
    {
        [Header("Enemy Spawner")]
        int numberOfEnemies;

        // [SerializeField] float spawnRadius = 5f;

        // Define the dimensions of the rectangular area
        [SerializeField] float rectWidth = 10f;
        [SerializeField] float rectHeight = 5f;
        float minSpawnDistance;
        bool spawnOnStart;

        [Header("Behaviors after spawn")]
        [MaxValue(1)]
        [SerializeField] float chanceToPatrol = 0.5f;
        [SerializeField] bool isLineRendererToSpawn;

        [Header("References")]
        [SerializeField] SpawnEffects spawnerPrefab;

        List<Vector3> spawnPoints = new(); // Track spawn points

        [Header("Debug")]
        [ReadOnly]
        public List<Health> spawnedEnemies = new(); // Track spawned enemies
        RunSceneData runSceneData;
        
        WaitForSeconds wait = new(.15f);

        bool stopSpawning;

        IEnumerator Start()
        {
            runSceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);

            numberOfEnemies = runSceneData.NumberOfEnemies;
            minSpawnDistance = runSceneData.MinSpawnDistance;
            spawnOnStart = runSceneData.SpawnOnStart;
            keyToReceive = runSceneData.EnemySpawnKeyToReceive;
            keyToSend = runSceneData.EnemySpawnKeyToSend;

            yield return new WaitForSeconds(2f);
            if (spawnOnStart)
                SpawnEnemies();
        }

        protected override void HandleReceivingKey()
        {
            base.HandleReceivingKey();

            // SpawnEnemies();
            stopSpawning = true;
        }

        public void SpawnEnemies(int overrideEnemiesToSpawn = -1)
        {
            StartCoroutine(SpawnEnemyCoroutine(overrideEnemiesToSpawn));
        }

        IEnumerator SpawnEnemyCoroutine(int overridEnemiesToSpawn = -1)
        {
            var enemiesToSpawn = overridEnemiesToSpawn != -1 ? overridEnemiesToSpawn : numberOfEnemies;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                yield return wait;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            if (stopSpawning) return;

            Vector3 randomPosition = GetRandomNavMeshPosition();

            if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
            {
                spawnPoints.Add(randomPosition); // Save the spawn point

                var enemyPrefab = GetRandomEnemyPrefab();

                // var spawnedEnemy = ObjectPoolManager.Instance.GetObject(enemyPrefab, randomPosition, Quaternion.identity,10);
                var spawnedEnemy = Instantiate(enemyPrefab, randomPosition, transform.rotation);

                if (spawnedEnemy.Health != null)
                {
                    spawnedEnemy.Health.OnDie += HandleSpawnDeath;
                    spawnedEnemies.Add(spawnedEnemy.Health);
                }

                if (isLineRendererToSpawn)
                {
                    spawnedEnemy.GetAIComponents().GetLineController().SetTarget(transform);
                }
            }
        }


        [ContextMenu(" Spawn Points")]
        public void SpawnEnemy(int overridEnemiesToSpawn = -1)
        {
            if (stopSpawning) return;
            int spawnedCount = 0;
            var enemiesToSpawn = overridEnemiesToSpawn != -1 ? overridEnemiesToSpawn : numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector3 randomPosition = GetRandomNavMeshPosition();

                if (randomPosition != Vector3.zero && IsFarFromOtherSpawnPoints(randomPosition))
                {
                    spawnPoints.Add(randomPosition); // Save the spawn point

                    var enemyPrefab = GetRandomEnemyPrefab();

                    // var spawnedEnemy = ObjectPoolManager.Instance.GetObject(enemyPrefab, randomPosition, Quaternion.identity,10);
                    var spawnedEnemy = Instantiate(enemyPrefab, randomPosition, transform.rotation);

                    if (spawnedEnemy.Health != null)
                    {
                        spawnedEnemy.Health.OnDie += HandleSpawnDeath;
                        spawnedEnemies.Add(spawnedEnemy.Health);
                    }

                    if (isLineRendererToSpawn)
                    {
                        spawnedEnemy.GetAIComponents().GetLineController().SetTarget(transform);
                    }

                    spawnedCount++;
                }
            }
        }

        void HandleSpawnDeath(Health obj)
        {
            DeRegisterEvents(obj);
            spawnedEnemies.Remove(obj);

            if (spawnedEnemies.Count == 0)
            {
                SendKey();
            }
        }

        void DeRegisterEvents(Health enemy)
        {
            enemy.OnDie -= HandleSpawnDeath;
        }

        EnemyStateMachine GetRandomEnemyPrefab()
        {
            float totalRatio = 0;
            foreach (var data in runSceneData.EnemySpawnDataList)
            {
                totalRatio += data.spawnRatio;
            }

            float randomValue = Random.Range(0, totalRatio);
            float cumulativeRatio = 0;

            foreach (var data in runSceneData.EnemySpawnDataList)
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

        [Button("Spawn Enemies Test")]
        void SpawnEnemiesTest()
        {
            SpawnEnemy(1);
        }

        [Button("Get Random Nav Mesh Position")]
        Vector3 GetRandomNavMeshPosition()
        {
            return NavMeshPosUtil.GetRandomNavMeshPosition(rectWidth, rectHeight, transform.position);
        }
    }

    [Serializable]
    public class EnemySpawnData
    {
        public EnemyPrefabSO enemySO;
        [MaxValue(100)] [MinValue(0)]
        public float spawnRatio;
    }
}