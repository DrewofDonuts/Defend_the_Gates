using System;
using System.Collections;
using System.Collections.Generic;
using Defend_the_Gates.AI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral.DefendTheGates
{
    public class EnemyWaveOrchestrator : MonoBehaviour, IGameStateListener
    {
        [Header("Wave Settings")]
        [SerializeField] List<SwarmInfo> waves;

        [Header("References")]
        [SerializeField] EnemyWaveSpawner enemySpawner;

        [Header("Debug")]
        public int currentWaveIndex;


        public GameState CurrentGameState { get; set; }

        void Start()
        {
            RegisterListener();
        }

        public void OnGameStateChanged(GameState newGameState)
        {
            CurrentGameState = newGameState;

            if (CurrentGameState is GameState.WavePhase)
                StartCoroutine(SpawnWaveSequence());
        }

        IEnumerator SpawnWaveSequence()
        {
            var wave = waves[currentWaveIndex];
            wave.currentSwarmCount = 0;

            //current wave repeats for wave.wavesToSpawn times
            while (wave.currentSwarmCount < wave.swarmsToSpawn)
            {
                wave.currentSwarmCount++;
                StartWave(wave);
                yield return new WaitForSeconds(wave.timeBetweenSwarms);
            }

            // Move to the next wave after completing the current one
            currentWaveIndex++;
        }

        void StartWave(SwarmInfo swarm)
        {
            enemySpawner.SpawnEnemies(swarm);
        }


        public void RegisterListener()
        {
            GameStateManager.Instance?.RegisterListener(this);
        }

        public void UnregisterListener()
        {
            GameStateManager.Instance?.UnregisterListener(this);
        }
    }

    [Serializable]
    public class SwarmInfo
    {
        public int enemyCount;
        [FormerlySerializedAs("wavesToSpawn")] public int swarmsToSpawn = 1;
        [FormerlySerializedAs("currentWaveCount")] public int currentSwarmCount;
        [FormerlySerializedAs("timeBetweenWaves")] public float timeBetweenSwarms;
        public EnemySpawnDataObject enemySpawnDataObject;
    }
}