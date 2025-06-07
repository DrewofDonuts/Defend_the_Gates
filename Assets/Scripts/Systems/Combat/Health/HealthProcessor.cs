using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class HealthProcessor
    {
        CharacterData characterData;
        float healthRegenTimer = 10f;
        float staminaRegenTimer = 5f;
        float defenseRegenTimer = 10f;
        float healthTimer;
        float defenseTimer;
        float staminaTimer;

        Health health;

        public HealthProcessor(Health _health, CharacterData characterData)
        {
            this.characterData = characterData;
            health = _health;

            health.OnUseHolyWill += DamageHolyWill;
            health.OnDamageHealth += HandleIncomingDamage;
        }

        public void OnDisableHealth()
        {
            health.OnDamageHealth -= DamageHealth;
            health.OnUseHolyWill -= DamageHolyWill;
        }

        public void SetInitialCurrentHealth(float modifiedValue = 0)
        {
            if (health.CharacterAttributes == null) return;
            characterData.health = modifiedValue;
        }

        public void SetInitialCurrentDefense(float modifiedValue = 0)
        {
            characterData.defense = modifiedValue;
        }

        public void DisablingStatBars()
        {
            if (Math.Abs(characterData.health - health.CharacterAttributes.MaxHealth) < .2f)
                health.StatBars.DisableHealthSlider();

            if (Math.Abs(characterData.defense - health.CharacterAttributes.MaxDefense) < .2f)
                health.StatBars.DisableDefenseSlider();
        }

        public void LoseDefenseAfterTime(float deltaTime)
        {
            if (characterData.defense > 0)
            {
                defenseTimer += deltaTime;
                if (defenseTimer >= defenseRegenTimer)
                {
                    characterData.defense -= health.CharacterAttributes.DefenseRegenRate * deltaTime;
                    characterData.defense = Mathf.Clamp(characterData.defense, 0f,
                        health.CharacterAttributes.MaxDefense);
                    
                    health.StatBars.RegenDefenseBar(-health.CharacterAttributes.DefenseRegenRate * deltaTime);
                }
            }
        }


        public void HealHealth(float healthToAdd)
        {
            characterData.health += healthToAdd;
            characterData.health =
                Mathf.Clamp(characterData.health, 0f, health.CharacterAttributes.MaxHealth);
            health.StatBars.RegenHealthBar(healthToAdd);
        }

        public void HealDefense(float willAmountWillAmount)
        {
            defenseTimer = 0f;

            characterData.defense += willAmountWillAmount;
            characterData.defense =
                Mathf.Clamp(characterData.defense, 0f, health.CharacterAttributes.MaxDefense);
            health.StatBars.RegenDefenseBar(willAmountWillAmount);
        }


        public void HealHoly(float willItemWillAmount)
        {
            characterData.holyWill += willItemWillAmount;
            characterData.holyWill = Mathf.Clamp(characterData.holyWill, 0f, health.CharacterAttributes.MaxHolyWill);

            if (health.StatBars.HolyWillSlider != null)
                health.StatBars.RegenStaminaBar(willItemWillAmount);
        }


        public void HandleIncomingDamage(float damage)
        {
            defenseTimer = 0f;

            //If there is defense left, reduce defense. Otherwise, damage health.
            if (characterData.defense > 0)
                DamageDefense(damage);
            else
                DamageHealth(damage);

            //If there is no defense left, damage health.
            if (characterData.defense < 0)
            {
                DamageHealth(Mathf.Abs(characterData.defense));
                characterData.defense = 0;
            }

            health.ValueChanged();
        }

        public void DamageDefense(float damage)
        {
            characterData.defense -= damage;

            // health.StatBars.DamageDefenseBar(damage);
        }

        public void DamageHealth(float damage)
        {
            healthTimer = 0f;
            characterData.health = Mathf.Max(characterData.health - damage, 0);

            // health.StatBars.DamageHealthBar(damage);
        }

        void DamageHolyWill(float _usedStamina)
        {
            staminaTimer = 0f;
            characterData.holyWill = Mathf.Max(characterData.holyWill - _usedStamina, 0);

            // health.StatBars.DamageStaminaBar(_usedStamina);
        }


        IEnumerator TimeBeforeHealthRegen()
        {
            yield return new WaitForSeconds(healthRegenTimer);
        }
    }
}


// void HealthRegen(float deltaTime)
// {
//     return;
//     if (characterData.health < health.CharacterAttributes.MaxHealth)
//     {
//         healthTimer += deltaTime;
//         if (healthTimer >= healthRegenTimer)
//         {
//             characterData.health += health.CharacterAttributes.HealthRegenRate * deltaTime;
//             characterData.health = Mathf.Clamp(characterData.health, 0f,
//                 health.CharacterAttributes.MaxHealth);
//             health.StatBars.RegenHealthBar(health.CharacterAttributes.HealthRegenRate * deltaTime);
//         }
//     }
// }
//
// void DefenseRegen(float deltaTime)
// {
//     return;
//     if (characterData.defense < health.CharacterAttributes.MaxDefense)
//     {
//         defenseTimer += deltaTime;
//         if (defenseTimer >= defenseRegenTimer)
//         {
//             characterData.defense += health.CharacterAttributes.DefenseRegenRate * deltaTime;
//             characterData.defense = Mathf.Clamp(characterData.defense, 0f,
//                 health.CharacterAttributes.MaxDefense);
//             health.StatBars.RegenDefenseBar(health.CharacterAttributes.DefenseRegenRate * deltaTime);
//         }
//     }
// }
//
// void StaminaRegen(float deltaTime)
// {
//     return;
//     if (characterData.stamina < health.CharacterAttributes.MaxStamina)
//     {
//         staminaTimer += deltaTime;
//         if (staminaTimer >= staminaRegenTimer)
//         {
//             characterData.stamina += health.CharacterAttributes.StaminaRegenRate * deltaTime;
//             characterData.stamina = Mathf.Clamp(characterData.stamina, 0f,
//                 health.CharacterAttributes.MaxStamina);
//
//             if (health.StatBars.StaminaSlider != null)
//                 health.StatBars.RegenStaminaBar(health.CharacterAttributes.StaminaRegenRate * deltaTime);
//         }
//     }
// }