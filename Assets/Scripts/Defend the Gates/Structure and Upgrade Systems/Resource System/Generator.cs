using System;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class Generator : Structure
    {
        [SerializeField] int resourceBonusPerWave = 50;

        int currentResourceBonusPerWave;


        void Start()
        {
            currentResourceBonusPerWave = resourceBonusPerWave;
        }
        
        public override void HandleDestroyed()
        {
            IsDestroyed = true;
        }

        public int GetResourceBonus()
        {
            return currentResourceBonusPerWave;
        }
    }
}