using System;
using System.Collections.Generic;
using Defend_the_Gates.AI;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class EnemyDefenseModeController : MonoBehaviour, IGameStateListener
    {
        [Header("Wave Settings")]
        [SerializeField] List<WaveInfo> waves;

        [Header("References")]
        [SerializeField] EnemyWaveSpawner enemySpawner;
        
        [Header("Debug")]
        public int currentWave;
        
        
        public GameState CurrentGameState { get; set; }

        void Start()
        {
            RegisterListener();
        }

        public void OnGameStateChanged(GameState newGameState)
        {
            CurrentGameState = newGameState;
            
            if(CurrentGameState is GameState.WavePhase)
                StartWave();

        }

        void StartWave()
        {
            enemySpawner.SpawnEnemies(waves[currentWave].enemyCount, waves[currentWave].enemySpawnDataObject);
            currentWave++;
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
    public class WaveInfo
    {
        public int enemyCount;
        public EnemySpawnDataObject enemySpawnDataObject;
    }
}