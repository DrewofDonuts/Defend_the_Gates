﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral.DefendTheGates
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Etheral/Defend The Gates/TowerData", order = 1)]
    public class TowerObject : StructureObject
    {
        [SerializeField] TowerData towerData;

        public override IUpgradable UpgradeData => towerData;
    }

    [Serializable]
    public class TowerData : IUpgradable
    {
        [SerializeField] string towerName;
        [SerializeField] GameObject towerPrefab;
        [SerializeField] string description;
        [SerializeField] Sprite towerIcon;
        [SerializeField] int level;
        [SerializeField] int damage;
        [SerializeField] float attackRange;
        [SerializeField] float attackSpeed;
        [SerializeField] float maxMaxHealth;
        [SerializeField] float projectileSpeed;

        // public string TowerName => towerName;
        public string Name => towerName;

        // public GameObject TowerPrefab => towerPrefab;
        public GameObject Prefab => towerPrefab;
        public string Description => description;
        public Sprite TowerIcon => towerIcon;
        public int Level => level;
        public int Damage => damage;
        public float AttackRange => attackRange;
        public float AttackSpeed => attackSpeed;
        public float MaxHealth => maxMaxHealth;
        public float ProjectileSpeed => projectileSpeed;
    }
}