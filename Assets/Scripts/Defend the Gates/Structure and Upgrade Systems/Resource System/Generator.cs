using System;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class Generator : MonoBehaviour, IStructure
    {
        [SerializeField] int resourceBonusPerWave = 50;
        [SerializeField] DefensesHealth defensesHealth;
        public bool IsDestroyed { get; private set; }

        int currentResourceBonusPerWave;


        void Start()
        {
            defensesHealth.OnDie += HandleGeneratorDestroyed;
            currentResourceBonusPerWave = resourceBonusPerWave;
        }

        void OnDisable()
        {
            if (defensesHealth != null)
            {
                defensesHealth.OnDie -= HandleGeneratorDestroyed;
            }
        }

        void HandleGeneratorDestroyed()
        {
            IsDestroyed = true;
        }
        
        public int GetResourceBonus()
        {
            return currentResourceBonusPerWave;

        }
    }
    
}