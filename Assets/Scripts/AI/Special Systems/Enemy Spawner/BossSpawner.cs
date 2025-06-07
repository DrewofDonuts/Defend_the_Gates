using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class BossSpawner : MessengerClass
    {
        // Define the dimensions of the rectangular area
        [SerializeField] float rectWidth = 10f;
        [SerializeField] float rectHeight = 5f;


        Health bossHealth;
        RunSceneData runSceneData;
        bool spawnOnStart;

        void Start()
        {
            runSceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
            Debug.Log($"Scene data is {runSceneData.SceneName}");
            spawnOnStart = runSceneData.SpawnBossOnStart;
            keyToSend = runSceneData.BossKeyToSend;

            if (spawnOnStart)
                SpawnBoss();
        }

        void SpawnBoss()
        {
            Vector3 spawnPoint = NavMeshPosUtil.GetRandomNavMeshPosition(rectWidth, rectHeight, transform.position);

            Debug.Log($"Spawning boss at {spawnPoint}");

            var bossPrefab = runSceneData.BossSO.EnemyStateMachine;

            var boss = Instantiate(bossPrefab, spawnPoint, Quaternion.identity);
            bossHealth = boss.Health;
            bossHealth.OnDie += HandleBossDeath;
        }

        void HandleBossDeath(Health obj)
        {
            SendKey();
        }

        void OnDestroy()
        {
            bossHealth.OnDie -= HandleBossDeath;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.black;

            // Gizmos.DrawWireSphere(transform.position, spawnRadius);
            Gizmos.DrawWireCube(transform.position, new Vector3(rectWidth, 5, rectHeight));
        }
    }
}