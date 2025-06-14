using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class Generator : Structure
    {
        [SerializeField] int resourceBonusPerWave = 50;

        [Header("Debug")]
        [ReadOnly]
       public int currentResourceBonusPerWave;


        void Start()
        {
            currentResourceBonusPerWave = resourceBonusPerWave;
            RegisterWthResourceManager();
        }

        void RegisterWthResourceManager()
        {
            if (ResourceManager.Instance == null)
            {
                Debug.LogError("ResourceManager instance is not set. Please ensure it is initialized before using the Generator.");
                return;
            }
            
            ResourceManager.Instance.RegisterGenerator(this);
        }

        void OnDisable()
        {
            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.UnregisterGenerator(this);
            }
        }

        public override void HandleDestroyed()
        {
            IsDestroyed = true;
        }

        public int GetResourceBonus()
        {
            return IsDestroyed ? 0 : currentResourceBonusPerWave;
        }
    }
}