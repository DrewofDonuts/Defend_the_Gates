using System.Collections.Generic;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class ResourceManager : MonoBehaviour
    {
        public int CurrentResources { get; private set; }
        public static ResourceManager Instance { get; private set; }


         List<Generator> activeGenerators = new();

        [SerializeField] private int baseResourcesPerWave = 100;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterGenerator(Generator generator)
        {
            if (!activeGenerators.Contains(generator))
                activeGenerators.Add(generator);
        }

        public void UnregisterGenerator(Generator generator)
        {
            activeGenerators.Remove(generator);
        }

        public void GrantWaveResources()
        {
            int totalGain = baseResourcesPerWave;

            foreach (var gen in activeGenerators)
            {
                if(gen.IsDestroyed) continue;
                totalGain += gen.GetResourceBonus();
            }

            CurrentResources += totalGain;
            Debug.Log($"Wave complete. Gained {totalGain} resources. Total: {CurrentResources}");
        }

        public bool TrySpendResources(int amount)
        {
            if (CurrentResources < amount) return false;

            CurrentResources -= amount;
            return true;
        }
  


    }
}