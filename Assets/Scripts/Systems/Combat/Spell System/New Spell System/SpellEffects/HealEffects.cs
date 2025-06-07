using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Etheral
{
    [Serializable]
    public class HealEffects : BaseEffect
    {
        public bool isInstantHeal;
        public bool healCasterOnly;
        public float healAmount = 10f;
        public float activeDuration = 5f;

        bool isActive = true;
        float durationTimer;


        public override void ApplyOnTriggerEnter(Collider collision)
        {
            if (!isInstantHeal) return;
            if (!CheckIfCaster(collision)) return; 
            if (!isActive) return;

            if (collision.TryGetComponent(out IHaveHealth health))
            {
                if (!IsAffiliationSameAsCaster(health.Affiliation)) return;
                health.Heal(healAmount);

                // spellObject.StartCoroutine(HealingOverTime(health, healAmount));
            }
        }

        public override void ApplyOnStay(Collider other)
        {
            if (isInstantHeal) return;
            if (!CheckIfCaster(other)) return;
            

            if (other.TryGetComponent(out IHaveHealth health))
            {
                if (!IsAffiliationSameAsCaster(health.Affiliation)) return;
                
                Debug.Log(other.name + " is being healed by " + caster.name);
                HealOverTime(health, healAmount);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (!isActive) return;
            durationTimer += deltaTime;

            if (durationTimer >= activeDuration)
                isActive = false;
        }

        bool CheckIfCaster(Collider collision)
        {
            //if it is the caster
            if (healCasterOnly && collision.gameObject == caster.gameObject)
                return true;

            //if we do not care if it is the caster
            if (!healCasterOnly)
                return true;

            //if we want to heal only the caster, and it is not the caster
            if (healCasterOnly && collision.gameObject != caster.gameObject)
                return false;

            return false;
        }

        void HealOverTime(IHaveHealth health, float healthPerSecond)
        {
            health.Heal(healthPerSecond * Time.deltaTime);
        }
    }
}