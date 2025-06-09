using System;
using UnityEngine;

namespace Etheral.Defenses
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "Etheral/DefendTheGates/TowerData", order = 1)]
    public class TowerObject : ScriptableObject
    {
        [SerializeField] TowerData towerData;

        public TowerData TowerData => towerData;
    }

    [Serializable]
    public class TowerData
    {
        [SerializeField] string towerName;
        [SerializeField] GameObject towerPrefab;
        [SerializeField] string description;
        [SerializeField] Sprite towerIcon;
        [SerializeField] int level;
        [SerializeField] int damage;
        [SerializeField] float attackRange;
        [SerializeField] float attackSpeed;
        [SerializeField] float health;
        [SerializeField] float projectileSpeed;
        
        public string TowerName => towerName;
        public GameObject TowerPrefab => towerPrefab;
        public string Description => description;
        public Sprite TowerIcon => towerIcon;
        public int Level => level;
        public int Damage => damage;
        public float AttackRange => attackRange;
        public float AttackSpeed => attackSpeed;
        public float Health => health;
        public float ProjectileSpeed => projectileSpeed;
    }
}