using System;
using Etheral.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    [Serializable]
    public class StatBars
    {
        [Tooltip("Healthbar UI slider")]
        [field: SerializeField] public Slider HealthSlider { get; private set; }
        [field: SerializeField] public Slider DefenseSlider { get; private set; }
        [field: SerializeField] public Slider HolyWillSlider { get; private set; }
        Health health;


        public void DisableHealthSlider() => HealthSlider.gameObject.SetActive(false);

        public void DisableDefenseSlider() => DefenseSlider.gameObject.SetActive(false);
        
        public void StartingMethod(Health _health)
        {
            SetHealth(_health);
            health.OnCurrentStatChanged += UpdateAllBars;

            if (health.CharacterAttributes == null)
            {
                HealthSlider.gameObject.SetActive(false);
                DefenseSlider.gameObject.SetActive(false);

                if (HolyWillSlider != null)
                    HolyWillSlider.gameObject.SetActive(false);

                return;
            }

            HealthSlider.gameObject.SetActive(true);
            DefenseSlider.gameObject.SetActive(true);

            if (HolyWillSlider != null)
                HolyWillSlider.gameObject.SetActive(true);

            SetMaxHealthBar(_health.CharacterAttributes.MaxHealth);
            SetMaxDefenseBar(_health.CharacterAttributes.MaxDefense);

            if (HolyWillSlider != null)
                SetMaxStaminaBar(_health.CharacterAttributes.MaxHolyWill);
            
        }

        void SetHealth(Health _health) => health = _health;


        void UpdateAllBars()
        {
            HealthSlider.value = health.CurrentHealth;
            DefenseSlider.value = health.CurrentDefense;
            
            
            if (HolyWillSlider != null)
                HolyWillSlider.value = health.CurrentHolyCharge;
        }


        void SetMaxStaminaBar(float maxStamina)
        {
            HolyWillSlider.maxValue = maxStamina;
            // StaminaSlider.value = Health.CurrentStamina;
        }

        public void SetMaxHealthBar(float maxHealth)
        {
            HealthSlider.maxValue = maxHealth;
            // HealthSlider.value = Health.CurrentHealth;
        }

        public void SetMaxDefenseBar(float maxDefense)
        {
            DefenseSlider.maxValue = maxDefense;
            // DefenseSlider.value = health.CurrentDefense;
        }
        
        public void RegenStaminaBar(float staminaRegen)
        {
            HolyWillSlider.value += staminaRegen;
        }

        public void RegenDefenseBar(float defenseRegen)
        {
            DefenseSlider.value += defenseRegen;

            //
            // if (Math.Abs(Health.playerData.defense - Health.CharacterAttributes.MaxDefense) <= 1f)
            // {
            //     DefenseSlider.gameObject.SetActive(false);
            // }
        }

        public void RegenHealthBar(float healthRegen)
        {
            HealthSlider.value += healthRegen;

            // if (Math.Abs(Health.playerData.health - Health.CharacterAttributes.MaxHealth) < 1f)
            // {
            //     HealthSlider.gameObject.SetActive(false);
            // }
        }

        public void LoadHealthAndDefenseSliders(Slider healthSlider, Slider defenseSlider)
        {
            HealthSlider = healthSlider;
            DefenseSlider = defenseSlider;
        }

        public void LoadStaminaSlider(Slider stiminaSlider)
        {
            HolyWillSlider = stiminaSlider;
        }

        public void LoadHealth(Health health)
        {
            this.health = health;
        }

        public void OnDisableHealth()
        {
            health.OnCurrentStatChanged -= UpdateAllBars;
        }
    }
}