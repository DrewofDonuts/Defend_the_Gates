using System;
using UnityEngine;

namespace Etheral
{
    public class StatHandler : MonoBehaviour
    {
        //Future state - have a "Key" for each special ability. If there is an override key for it, then apply the override.

        public StatsOverrideData statsOverrideData;


        public float GetModfiedMaxHealth(CharacterAttributes characterAttributes)
        {
            if (statsOverrideData.isHealthOverride)
                return statsOverrideData.health;

            return characterAttributes.MaxHealth;
        }

        public float GetModifiedMaxDefense(CharacterAttributes characterAttributes)
        {
            if (statsOverrideData.isDefenseOverride)
                return statsOverrideData.defense;

            return characterAttributes.MaxDefense;
        }

        public float GetModifiedMovementSpeed(CharacterAttributes characterAttributes)
        {
            if (statsOverrideData.isSpeedOverride)
                return statsOverrideData.speed;

            return characterAttributes.RunSpeed;
        }

        public float GetAIDetectionRange(AIAttributes characterAttributes)
        {
            if (statsOverrideData.isDetectionRangeOverride)
                return statsOverrideData.detectionRange;

            return characterAttributes.DetectionRange;
        }

        public float GetMeleeAttackRange(AIAttributes characterAttributes)
        {
            if (statsOverrideData.isMeleeAttackRangeOverride)
                return statsOverrideData.meleeAttackRange;

            return characterAttributes.MeleeAttackRange;
        }
        
        public float GetAttackSpeed(CharacterAttributes characterAttributes)
        {
            if (statsOverrideData.isAttackSpeedOverride)
                return statsOverrideData.attackSpeed;

            return 1f;
        }
    }


    [Serializable]
    public class StatsOverrideData
    {
        public bool isHealthOverride;
        public bool isDefenseOverride;
        public bool isSpeedOverride;
        public bool isDetectionRangeOverride;
        public bool isMeleeAttackRangeOverride;
        public bool isAttackSpeedOverride;


        public float health;
        public float defense;
        public float speed;
        public float detectionRange;
        public float meleeAttackRange;
        public float attackSpeed;
    }
}