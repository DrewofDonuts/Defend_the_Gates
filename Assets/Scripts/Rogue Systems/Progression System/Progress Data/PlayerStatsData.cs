using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable]
    public class PlayerStatsData
    {
         [Header("Abilities")]
        public List<PlayerAbilityTypes> unlockedAbilities = new();
        [Header("Abilities")]
        public PlayerAbilityTypes currentAbility;

        [Header("Stats")]
        public float maxHealthBonus;
        public float willBonus;
        public float holyBonus;
        public float attackSpeedBonus;
        public float movementSpeedBonus;
        public float attackDamageModifier;
        public float maxAmmoBonus;
        public float aimAccuracyBonus;
    }

    [Serializable]
    public class PermanentResources
    {
        [SerializeField] int addedAtonementCost;

        [SerializeField] int maxAtonement = 100;
        [SerializeField] int currentAtonement;
        public int CurrentAtonement => currentAtonement;
        public int AddedAtonementCost => addedAtonementCost;
        public int MaxAtonement => maxAtonement;

        public void IncreaseCostAndMax(int cost)
        {
            addedAtonementCost += cost;
            maxAtonement += cost;
        }

        public void AddXP(int xp)
        {
            currentAtonement = Math.Min(currentAtonement + xp, maxAtonement);
        }
    }
}