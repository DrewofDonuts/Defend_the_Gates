using System;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class ItemSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AIHealth aiHealth;

        [Header("Item Prefabs ")]
        [SerializeField] WillItem defenseWill;
        [SerializeField] WillItem staminaWill;
        [SerializeField] WillItem holyWill;

        // [SerializeField] PickUpItem arrows;

        [Header("Spawn Chances")]
        [MaxValue(1)]
        [SerializeField] float stamOrDefWillSpawnChance = .25f;
        [MaxValue(1)]
        [SerializeField] float holyWillSpawnChance = .10f;

        // [MaxValue(1)]
        // [SerializeField] float arrowsSpawnChance = .05f;

        [Header("Increase Chance Increment")]
        [SerializeField] float amountToIncrease = .05f;

        [ReadOnly]
        public float increaseChance;
        float lastTimeHit;


        void Start()
        {
            if (aiHealth == null) return;

            aiHealth.OnTakeHit += HandleTakeHit;
            aiHealth.OnDie += HandleDie;
        }

        void OnDisable()
        {
            if (aiHealth == null) return;

            aiHealth.OnTakeHit -= HandleTakeHit;
            aiHealth.OnDie -= HandleDie;
        }

        void HandleDie(IHaveHealth health)
        {
            if (EventBusPlayerController.PlayerStateMachine == null)
                return;
            var healCtrlr = EventBusPlayerController.PlayerStateMachine.PlayerComponents.GetHealController();
            if (healCtrlr.healsRemaining == healCtrlr.MaxHeals) return;

            var chance = UnityEngine.Random.Range(0f, 1f);

            if (chance < holyWillSpawnChance)
            {
                SpawnHolyWill();
                increaseChance = 0;
            }
            else
                increaseChance += .1f;

            lastTimeHit = Time.time;
        }

        void SpawnHolyWill()
        {
            var will = Instantiate(holyWill, transform.position, Quaternion.identity);
            will.SpawnAfterMoving(transform.position);
        }

        void HandleTakeHit(IDamage obj)
        {
            if (increaseChance > 0)
            {
                if (Mathf.Abs(Time.time - lastTimeHit) > 5)
                    increaseChance = 0;
            }

            var chance = UnityEngine.Random.Range(0f, 1f);

            // if (HasChanceToSpawnArrows() && chance < arrowsSpawnChance + increaseChance)
            // {
            //     SpawnArrow();
            //     increaseChance = 0;
            // }
            if (chance < stamOrDefWillSpawnChance + increaseChance)
            {
                SpawnStaminaOrDefenseWill();
                increaseChance = 0;
            }
            else
                increaseChance += amountToIncrease;

            lastTimeHit = Time.time;
        }

        // void SpawnArrow()
        // {
        //     var spawnedArrows = Instantiate(arrows, transform.parent.position, Quaternion.identity);
        //     spawnedArrows.SpawnAfterMoving(transform.position);
        // }

        void SpawnStaminaOrDefenseWill()
        {
            var will = Instantiate(DetermineWillToSpawn(), transform.position, Quaternion.identity);
            will.SpawnAfterMoving(transform.position);
        }

        bool HasChanceToSpawnArrows()
        {
            var currentAmmo = EventBusPlayerController.PlayerStateMachine.PlayerComponents.GetAmmoController()
                .GetCurrentAmmo;

            return currentAmmo < 5;
        }

        WillItem DetermineWillToSpawn()
        {
            if (EventBusPlayerController.PlayerStateMachine == null)
                return default;

            var playerHealth = EventBusPlayerController.PlayerStateMachine.Health;

            // Calculate needs (1.0 = full need, 0.0 = no need)
            float defenseNeed = 1.0f - playerHealth.CurrentDefense / playerHealth.MaxDefense;
            float staminaNeed = 1.0f - playerHealth.CurrentHolyCharge / playerHealth.MaxHolyCharge;

            // Normalize needs for weighted probability
            // Calculate the weight for each stat  based on its proportion of the total need
            // // If totalNeed is 0, default to .5
            float totalNeed = defenseNeed + staminaNeed;
            float defenseWeight = totalNeed > 0 ? defenseNeed / totalNeed : .5f;
            float staminaWeight = totalNeed > 0 ? staminaNeed / totalNeed : .5f;

            // Randomly determine which Will  to spawn
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue < defenseWeight)
            {
                return defenseWill;
            }

            return staminaWill;
        }
    }
}


//Code if we include three stats, such as Health
// public WillItem SpawnPowerup()
// {
//     // Calculate needs (1.0 = full need, 0.0 = no need)
//     float defenseNeed = 1.0f - (PlayerDefense / MaxDefense);
//     float staminaNeed = 1.0f - (PlayerStamina / MaxStamina);
//     float holyNeed = (MaxHolySpells - CurrentHolySpells) / (float)MaxHolySpells; // Discrete scaling for Holy
//
//     // Normalize needs for weighted probability
//     float totalNeed = defenseNeed + staminaNeed + holyNeed;
//     float defenseWeight = totalNeed > 0 ? defenseNeed / totalNeed : 1.0f / 3.0f;
//     float staminaWeight = totalNeed > 0 ? staminaNeed / totalNeed : 1.0f / 3.0f;
//     float holyWeight = totalNeed > 0 ? holyNeed / totalNeed : 1.0f / 3.0f;
//
//     // Randomly determine which powerup to spawn
//     float randomValue = UnityEngine.Random.Range(0f, 1f);
//     if (randomValue < defenseWeight)
//     {
//         return WillItem.DefensePowerup;
//     }
//     else if (randomValue < defenseWeight + staminaWeight)
//     {
//         return WillItem.StaminaPowerup;
//     }
//     else
//     {
//         return WillItem.HolyPowerup;
//     }
// }