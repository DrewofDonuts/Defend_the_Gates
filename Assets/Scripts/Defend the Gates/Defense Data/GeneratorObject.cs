using System;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    [CreateAssetMenu(fileName = "GeneratorObject", menuName = "Defend The Gates/GeneratorObject", order = 1)]
    public class GeneratorObject : ScriptableObject
    {
        [SerializeField] GeneratorData generatorData;
        public GeneratorData GeneratorData => generatorData;
    }

    [Serializable]
    public class GeneratorData
    {
        [SerializeField] string generatorName;
        [SerializeField] GameObject generatorPrefab;
        [SerializeField] string description;
        [SerializeField] Sprite generatorIcon;
        [SerializeField] int level;
        [SerializeField] int resourceBonusPerWave;
        [SerializeField] float health;

        public string GeneratorName => generatorName;
        public GameObject GeneratorPrefab => generatorPrefab;
        public string Description => description;
        public Sprite GeneratorIcon => generatorIcon;
        public int Level => level;
        public int ResourceBonusPerWave => resourceBonusPerWave;
        public float Health => health;
    }
}