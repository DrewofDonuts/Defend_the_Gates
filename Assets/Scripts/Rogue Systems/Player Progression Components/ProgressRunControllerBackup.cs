using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class ProgressRunControllerBackup : MonoBehaviour
    {
        [SerializeField] PlayerAttributes playerAttributes;
        [SerializeField] RunBinder runBinder;

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
            runBinder.OnNewRun += InitializeStats;
            SetBaseStats();
        }

        public void UpdateXPInRunBinder(int xp)
        {
            //Future logic to modify xp  based on game state
            runBinder.playerAbilityAndResourceData.GatheredResources.AddXP(xp);
        }

        void OnDisable()
        {
            runBinder.OnNewRun -= InitializeStats;
            EventBusPlayerController.OnGetXP -= UpdateXPInRunBinder;
        }

        public void InitializeStats()
        {
            runBinder.playerAbilityAndResourceData.playerStatsData.currentAbility = PlayerAbilityTypes.None;
            runBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus = 0;
            runBinder.playerAbilityAndResourceData.playerStatsData.willBonus = 0;
            runBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus = 0;
            runBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus = 0;
            runBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier = 0;
        }

        public void SetBaseStats()
        {
            baseHealth = playerAttributes.MaxHealth;
            baseArmor = playerAttributes.MaxDefense;
            baseAttackSpeed = 1f;
            baseMovementSpeed = playerAttributes.RunSpeed;
            baseAttackDamage = 0;
        }


        public void ReceiveRewardData(PlayerStatsData playerStatsData)
        {
            if (playerStatsData.maxHealthBonus > 0)
                runBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus += playerStatsData.maxHealthBonus;
            if (playerStatsData.willBonus > 0)
                runBinder.playerAbilityAndResourceData.playerStatsData.willBonus += playerStatsData.willBonus;
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
            runBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus += movementSpeedBonus;
        }

        void UpdateAttackSpeed(float bonusAttackSpeed)
        {
            runBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus += bonusAttackSpeed;
        }

        void UpdateAttackDamageBonus(float attackDamageBonus)
        {
            runBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier +=
                Mathf.Max(runBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier * attackDamageBonus, attackDamageBonus);
        }

        void UpdateAimAccuracy(float aimAccuracy)
        {
            runBinder.playerAbilityAndResourceData.playerStatsData.aimAccuracyBonus += aimAccuracy;
        }

        public void UpdateAbility(PlayerAbilityTypes abilityType)
        {
            runBinder.playerAbilityAndResourceData.playerStatsData.currentAbility  = abilityType;
        }
        #endregion


        #region Get Stats
        public float GetAttackSpeed() =>
            baseAttackSpeed + baseAttackSpeed * runBinder.playerAbilityAndResourceData.playerStatsData.attackSpeedBonus;

        public float GetMovementSpeed() =>
            baseMovementSpeed + baseMovementSpeed * runBinder.playerAbilityAndResourceData.playerStatsData.movementSpeedBonus;

        public float GetMaxHealth() => runBinder.playerAbilityAndResourceData.playerStatsData.maxHealthBonus + baseHealth;
        public float GetMaxArmor() => runBinder.playerAbilityAndResourceData.playerStatsData.willBonus + baseArmor;
        public float GetBonusAttackDamage() => runBinder.playerAbilityAndResourceData.playerStatsData.attackDamageModifier;
        public float GetAimAccuracy() => runBinder.playerAbilityAndResourceData.playerStatsData.aimAccuracyBonus;
        public int GetAtonement() => runBinder.playerAbilityAndResourceData.GatheredResources.CurrentAtonement;
        public int GetMaxAtonement() => runBinder.playerAbilityAndResourceData.GatheredResources.MaxAtonement;

        public bool CheckAbility(PlayerAbilityTypes abilityType) =>   runBinder.playerAbilityAndResourceData.playerStatsData.currentAbility  == abilityType;
        public PlayerAbilityTypes GetCurrentAbility() =>   runBinder.playerAbilityAndResourceData.playerStatsData.currentAbility ;
        #endregion
    }
}