using System;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    [CreateAssetMenu(fileName = "GeneratorObject", menuName = "Etheral/Defend The Gates/GeneratorObject", order = 1)]
    public class GeneratorObject : StructureObject
    {
        [SerializeField] GeneratorData generatorData;
        public override IUpgradable UpgradeData => generatorData;
    }

    [Serializable]
    public class GeneratorData : IUpgradable
    {
        [SerializeField] string generatorName;
        [SerializeField] GameObject generatorPrefab;
        [SerializeField] string description;
        [SerializeField] Sprite generatorIcon;
        [SerializeField] int level;
        [SerializeField] int resourceBonusPerWave;
        [SerializeField] float health;

        public string Name => generatorName;
        public string Description => description;
        public Sprite GeneratorIcon => generatorIcon;
        public int Level => level;
        public GameObject Prefab => generatorPrefab;
        public int ResourceBonusPerWave => resourceBonusPerWave;
        public float Health => health;
    }
}