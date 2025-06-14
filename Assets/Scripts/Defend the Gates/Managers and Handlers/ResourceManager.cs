using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class ResourceManager : MonoBehaviour, IGameStateListener, IInitialize
    {
        public int CurrentResources { get; private set; }
        public static ResourceManager Instance { get; private set; }
        public GameState CurrentGameState { get; }


        List<Generator> activeGenerators = new();

        [SerializeField] int baseResourcesPerWave = 100;
        public event Action<int> OnResourcesChanged; 

        // public int LastWaveResources { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Initialize()
        {
            RegisterListener();
        }

        public void OnGameStateChanged(GameState newGameState)
        {
            if (newGameState == GameState.BasePhase)
            {
                GrantWaveResources();
            }
        }

        public void RegisterListener()
        {
            if(GameStateManager.Instance == null)
            {
                Debug.LogError("GameStateManager is not initialized. Cannot register ResourceManager listener.");
                return;
            }
            GameStateManager.Instance.RegisterListener(this);
        }

        public void UnregisterListener()
        {
            GameStateManager.Instance.UnregisterListener(this);
        }

        public void GrantWaveResources()
        {
            // Only grant resources if the game is in the base phase and a wave has been completed
            if (GameStateManager.Instance.CurrentWave == 0) return;

            int totalGain = baseResourcesPerWave;

            foreach (var gen in activeGenerators)
            {
                totalGain += gen.GetResourceBonus();
            }

            CurrentResources += totalGain;
            OnResourcesChanged?.Invoke(totalGain);
       
            Debug.Log($"Wave complete. Gained {totalGain} resources. Total: {CurrentResources}");
        }

        public bool TrySpendResources(int amount)
        {
            if (CurrentResources < amount) return false;

            CurrentResources -= amount;
            return true;
        }

        public void RegisterGenerator(Generator generator)
        {
            Debug.Log($"Attempting to register generator: {generator.name}");
            if (!activeGenerators.Contains(generator))
                activeGenerators.Add(generator);
        }

        public void UnregisterGenerator(Generator generator)
        {
            activeGenerators.Remove(generator);
        }
    }
}