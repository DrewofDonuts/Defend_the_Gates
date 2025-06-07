using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] PlayerAttributes playerAttributes;
        [SerializeField] StatsBinder statsBinder;
        [SerializeField] List<PlayerAbilityTypes> startingAbilities;
        
        public StatsBinder StatsBinder => statsBinder;
        
        [ReadOnly]
        public float baseHealth;
        [ReadOnly]
        public float baseArmor;
        [ReadOnly]
        public float baseAttackSpeed;
        [ReadOnly]
        public float baseMovementSpeed;
        [ReadOnly]
        public float baseAttackDamage;


        void Start()
        {
            SetBaseStats();
            statsBinder.OnNewCharacter += InitializeStats;
        }

        public void UpdateXPInRunBinder(int xp)
        {
            //Future logic to modify xp  based on game state
            statsBinder.playerAbilityAndResourceData.GatheredResources.AddXP(xp);
        }

        void OnDisable()
        {
            EventBusPlayerController.OnGetXP -= UpdateXPInRunBinder;
            statsBinder.OnNewCharacter -= InitializeStats;
        }


        public void SetBaseStats()
        {
            baseHealth = playerAttributes.MaxHealth;
            baseArmor = playerAttributes.MaxDefense;
            baseAttackSpeed = 1f;
            baseMovementSpeed = playerAttributes.RunSpeed;
            baseAttackDamage = 0;
        }
        
         void InitializeStats()
         {
             statsBinder.playerAbilityAndResourceData.playerStatsData.unlockedAbilities = startingAbilities;
            statsBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus = 0;
            statsBinder.playerAbilityAndResourceData.playerStatsData.willBonus = 0;
            statsBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus = 0;
            statsBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus = 0;
            statsBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier = 0;
        }

        public void ReceiveRewardData(PlayerStatsData playerStatsData)
        {
            if (playerStatsData.maxHealthBonus > 0)
                statsBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus +=
                    playerStatsData.maxHealthBonus;
            if (playerStatsData.willBonus > 0)
                statsBinder.playerAbilityAndResourceData.playerStatsData.willBonus += playerStatsData.willBonus;
            if (playerStatsData.attackSpeedBonus > 0)
                UpdateAttackSpeed(playerStatsData.attackSpeedBonus);
            if (playerStatsData.movementSpeedBonus > 0)
                CalculateUpdatedMovementSpeedByPercentage(playerStatsData.movementSpeedBonus);
            if (playerStatsData.attackDamageModifier > 0)
                UpdateAttackDamageBonus(playerStatsData.attackDamageModifier);
            if (playerStatsData.aimAccuracyBonus > 0)
                UpdateAimAccuracy(playerStatsData.aimAccuracyBonus);
        }


        #region Update Stats
        void CalculateUpdatedMovementSpeedByPercentage(float movementSpeedBonus)
        {
            Debug.Log("Movement Speed Bonus: " + movementSpeedBonus);
            statsBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus += movementSpeedBonus;
        }

        void UpdateAttackSpeed(float bonusAttackSpeed)
        {
            statsBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus += bonusAttackSpeed;
        }

        void UpdateAttackDamageBonus(float attackDamageBonus)
        {
            statsBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier +=
                Mathf.Max(
                    statsBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier * attackDamageBonus,
                    attackDamageBonus);
        }

        void UpdateAimAccuracy(float aimAccuracy)
        {
            statsBinder.playerAbilityAndResourceData.playerStatsData.aimAccuracyBonus += aimAccuracy;
        }
        
        public void UpdateAvailableAbilities(PlayerAbilityTypes abilityType)
        {
            if (!startingAbilities.Contains(abilityType))
            {
                statsBinder.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Add(abilityType);
            }
        }

        
        //Old method for Rogue-lite, where the player could select a single ability. May be used in the future.
        public void UpdateAbility(PlayerAbilityTypes abilityType)
        {
            statsBinder.playerAbilityAndResourceData.playerStatsData.currentAbility = abilityType;
        }
        #endregion


        #region Get Stats
        public float GetAttackSpeed() =>
            baseAttackSpeed +
            baseAttackSpeed * statsBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus;

        public float GetMovementSpeed() =>
            baseMovementSpeed + baseMovementSpeed *
            statsBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus;

        public float GetMaxHealth() =>
            statsBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus + baseHealth;

        public float GetMaxArmor() => statsBinder.playerAbilityAndResourceData.playerStatsData.willBonus + baseArmor;

        public float GetBonusAttackDamage() =>
            statsBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier;

        public float GetAimAccuracy() => statsBinder.playerAbilityAndResourceData.playerStatsData.aimAccuracyBonus;
        public int GetAtonement() => statsBinder.playerAbilityAndResourceData.GatheredResources.CurrentAtonement;
        public int GetMaxAtonement() => statsBinder.playerAbilityAndResourceData.GatheredResources.MaxAtonement;


        //need to pick one of these three ways of checking abilities, based on design. 
        
        public bool CheckAbility(PlayerAbilityTypes abilityType) => 
            statsBinder.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Contains(abilityType);


        public PlayerAbilityTypes GetCurrentAbility() =>
            statsBinder.playerAbilityAndResourceData.playerStatsData.currentAbility;
        #endregion
    }
}