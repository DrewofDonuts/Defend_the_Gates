using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    /*
     * The ActionProcessor class is responsible for handling the actions of a character in the game.
     * It manages the application of forces, casting of spells, and enabling/disabling of weapons.
     *
     * How to use this class:
     *
     * Enter()
     * 1. Instantiate an ActionProcessor object use the SetReferencesForForceSpellsWeapons method to pass references
     * 2. Use the PassRightBaseDamage and PassLeftBaseDamage methods to set the base damage for the right and left weapons
     * Tick()
     * 3. Use the ApplyForceTimes method
     * 4. Use the RightWeaponTimes and LeftWeaponTimes methods
     * 5. Use the CastSpell method in your update loop to cast spells at the specified times
     * Exit()
     * 6. Use the ClearAll method when you want to reset all the actions.
     */

    public class ActionProcessor
    {
        StateMachine stateMachine;
        CharacterAction characterAction;


        int currentForceIndex;

        int currentSpellIndex;
        int currentProjectileIndex;

        int enableRightWeaponIndex;
        int disableRightWeaponIndex;

        int enableLeftWeaponIndex;
        int disableLeftWeaponIndex;

        List<bool> alreadyAppliedForces = new();
        List<bool> alreadyCastSPell = new();
        List<bool> alreadyFiredProjectile = new();
        List<bool> alreadyEnabledRightWeapon = new();
        List<bool> alreadyEnabledLeftWeapon = new();
        bool alreadyAppliedStamina;

        public  void SetupActionProcessorForThisAction(StateMachine _stateMachine, CharacterAction _characterAction,
            float bonusDamage = 0)
        {
            characterAction = _characterAction;
            stateMachine = _stateMachine;

            if (CheckForMissingReferences()) return;

            if (characterAction.TimesBeforeForce.Length > 0)
                InitializeForcesList();

            if (characterAction.TimesBeforeSpells.Length > 0)
                InitializeSpellsList();

            if (characterAction.TimesBeforeProjectile.Length > 0)
                InitializeProjectilesList();

            if (characterAction.EnableRightWeapon.Length > 0)
            {
                InitialRightWeaponList();
                PassRightBaseDamage(bonusDamage);
            }

            if (characterAction.EnableLeftWeapon.Length > 0)
            {
                InitializeLeftWeaponList();
                PassLeftBaseDamage(bonusDamage);
            }

            if (characterAction.IsUseStamina)
                InitializeStaminaField();
        }

        void InitializeStaminaField()
        {
            alreadyAppliedStamina = false;
        }

        public void ApplyStamina(float normalizedValue)
        {
            if (characterAction.TimeBeforeStamina <= 0) return;

            if (normalizedValue >= characterAction.TimeBeforeStamina)
            {
                if (!alreadyAppliedStamina)
                {
                    stateMachine.Health.UseStamina(characterAction.StaminaCost);
                    alreadyAppliedStamina = true;
                }
            }
        }


        public void PassRightBaseDamage(float bonusDamage)
        {
            if (CheckForMissingReferences()) return;
            stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(characterAction, bonusDamage, stateMachine.charID);
        }


        public void PassLeftBaseDamage(float bonusDamage)
        {
            if (CheckForMissingReferences()) return;

            stateMachine.WeaponHandler._currentLeftHandDamage.SettAttackStatDamage(characterAction, bonusDamage, stateMachine.charID);
        }

        public void InitializeSpellsList()
        {
            for (var i = 0; i < characterAction.TimesBeforeSpells.Length; i++)
            {
                alreadyCastSPell.Add(false);
            }
        }

        public void InitializeProjectilesList()
        {
            for (var i = 0; i < characterAction.TimesBeforeProjectile.Length; i++)
            {
                alreadyFiredProjectile.Add(false);
            }
        }

        public void InitializeForcesList()
        {
            for (var i = 0; i < characterAction.TimesBeforeForce.Length; i++)
            {
                alreadyAppliedForces.Add(false);
            }
        }

        public void InitialRightWeaponList()
        {
            for (var i = 0; i < characterAction.EnableRightWeapon.Length; i++)
            {
                alreadyEnabledRightWeapon.Add(false);
            }
        }

        void InitializeLeftWeaponList()
        {
            for (var i = 0; i < characterAction.EnableLeftWeapon.Length; i++)
            {
                alreadyEnabledLeftWeapon.Add(false);
            }
        }

        public void ClearAll()
        {
            alreadyAppliedForces.Clear();
            alreadyCastSPell.Clear();
            alreadyFiredProjectile.Clear();

            currentForceIndex = 0;
            currentSpellIndex = 0;
            enableRightWeaponIndex = 0;
            disableRightWeaponIndex = 0;
            enableLeftWeaponIndex = 0;
            disableLeftWeaponIndex = 0;
        }

        public void ApplyForceTimes(float normalizedValue, bool isAdjacent = false)
        {
            if (currentForceIndex >= characterAction.TimesBeforeForce.Length) return;
            float currentForce;


            if (normalizedValue >= characterAction.TimesBeforeForce[currentForceIndex])
            {
                if (!alreadyAppliedForces[currentForceIndex])
                {
                    currentForce = isAdjacent
                        ? characterAction.Forces[currentForceIndex] / 3
                        : characterAction.Forces[currentForceIndex];

                    alreadyAppliedForces[currentForceIndex] = ApplyForce(alreadyAppliedForces[currentForceIndex],
                        currentForce);
                }

                currentForceIndex++;

                if (currentForceIndex >= alreadyAppliedForces.Count)
                    currentForceIndex = 0;
            }
        }


        public bool ApplyForce(bool _alreadyAppliedForce, float _force)
        {
            if (!_alreadyAppliedForce)
            {
                stateMachine.AddForce(stateMachine.transform.forward * _force);
            }

            return true;
        }


        public void RightWeaponTimes(float normalizedValue)
        {
            EnablingRightWeapon(normalizedValue);
            DisablingRightWeapon(normalizedValue);
        }

        public void LeftWeaponTimes(float normalizedValue)
        {
            EnablingLeftWeapon(normalizedValue);
            DisablingLeftWeapon(normalizedValue);
        }

        public void EnablingRightWeapon(float normalizedValue)
        {
            if (characterAction.EnableRightWeapon.Length == 0) return;

            if (enableRightWeaponIndex >= characterAction.EnableRightWeapon.Length) return;

            if (normalizedValue >= characterAction.EnableRightWeapon[enableRightWeaponIndex])
            {
                if (!alreadyEnabledRightWeapon[enableRightWeaponIndex])
                {
                    stateMachine.WeaponHandler.EnableRightWeapon();
                    alreadyEnabledRightWeapon[enableRightWeaponIndex] = true;
                }

                enableRightWeaponIndex++;

                if (enableRightWeaponIndex >= characterAction.EnableRightWeapon.Length)
                    enableRightWeaponIndex = 0;
            }
        }

        public void EnablingLeftWeapon(float normalizedValue)
        {
            if (characterAction.EnableLeftWeapon.Length == 0) return;

            if (enableLeftWeaponIndex >= characterAction.EnableLeftWeapon.Length) return;

            if (normalizedValue >= characterAction.EnableLeftWeapon[enableLeftWeaponIndex])
            {
                if (!alreadyEnabledLeftWeapon[enableLeftWeaponIndex])
                {
                    stateMachine.WeaponHandler.EnableLeftWeapon();
                    alreadyEnabledLeftWeapon[enableLeftWeaponIndex] = true;
                }

                enableLeftWeaponIndex++;

                if (enableLeftWeaponIndex >= characterAction.EnableLeftWeapon.Length)
                    enableLeftWeaponIndex = 0;
            }
        }

        public void DisablingRightWeapon(float normalizedValue)
        {
            if (characterAction.DisableRightWeapon.Length == 0) return;


            if (disableRightWeaponIndex >= characterAction.DisableRightWeapon.Length) return;

            if (normalizedValue >= characterAction.DisableRightWeapon[disableRightWeaponIndex])
            {
                stateMachine.WeaponHandler.DisableRightWeapon();
                disableRightWeaponIndex++;

                if (disableRightWeaponIndex > characterAction.DisableRightWeapon.Length)
                {
                    disableRightWeaponIndex = 0;
                }
            }
        }

        public void DisablingLeftWeapon(float normalizedValue)
        {
            if (characterAction.DisableLeftWeapon.Length == 0) return;
            if (disableLeftWeaponIndex >= characterAction.DisableLeftWeapon.Length) return;

            if (normalizedValue >= characterAction.DisableLeftWeapon[disableLeftWeaponIndex])
            {
                stateMachine.WeaponHandler.DisableLeftWeapon();
                disableLeftWeaponIndex++;

                if (disableLeftWeaponIndex >= characterAction.DisableLeftWeapon.Length)
                    disableLeftWeaponIndex = 0;
            }
        }


        public void FireProjectiles(float normalizedValue, float aimAccuracyModifier)
        {
            if (currentProjectileIndex >= characterAction.TimesBeforeProjectile.Length) return;

            if (normalizedValue >= characterAction.TimesBeforeProjectile[currentProjectileIndex])
            {
                if (!alreadyFiredProjectile[currentProjectileIndex])
                {
                    alreadyFiredProjectile[currentProjectileIndex] = true;
                    stateMachine.WeaponHandler.ReleaseProjectile(aimAccuracyModifier);
                }

                currentProjectileIndex++;

                if (currentProjectileIndex > alreadyFiredProjectile.Count)
                    currentProjectileIndex = 0;
            }
        }


        public void CastSpells(float normalizedValue)
        {
            if (currentSpellIndex >= characterAction.TimesBeforeSpells.Length) return;

            if (normalizedValue >= characterAction.TimesBeforeSpells[currentSpellIndex])
            {
                if (!alreadyCastSPell[currentSpellIndex])
                {
                    alreadyCastSPell[currentSpellIndex] = true;
                    stateMachine.GetCharComponents().GetSpellHandler().CastSpell(characterAction, stateMachine);
                }

                currentSpellIndex++;

                if (currentSpellIndex > alreadyCastSPell.Count)
                    currentSpellIndex = 0;
            }
        }

        public void CastSpellWithoutNormalizedValue()
        {
            stateMachine.GetCharComponents().GetSpellHandler().CastSpell(characterAction, stateMachine);
        }

        string noStateMachine =
            "StateMachine is null in Action Processor\n Did you forget to set the stateMachine in the Action Processor first?";

        void StateMachineError()
        {
            Debug.LogError(noStateMachine);
        }

        string noCharacterAction =
            "Character Action is null in Action Processor\n " +
            "Did you forget to set the characterAction in the Action Processor first?";

        void NoCharacterAction()
        {
            Debug.LogError(noCharacterAction);
        }

        bool CheckForMissingReferences()
        {
            if (stateMachine == null)
            {
                StateMachineError();
                return true;
            }

            if (characterAction == null)
            {
                NoCharacterAction();
                return true;
            }

            return false;
        }
    }
}