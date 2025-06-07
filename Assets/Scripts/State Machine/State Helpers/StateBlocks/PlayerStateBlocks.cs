using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class PlayerStateBlocks : StateBlocks
    {
        PlayerStateMachine stateMachine;
        PlayerStatsController playerStatsController;

        public PlayerStateBlocks(PlayerStateMachine stateMachine, PlayerStatsController playerStatsController)
        {
            this.stateMachine = stateMachine;
            this.playerStatsController = playerStatsController;
        }

        public void EnterAttackingState(int attackIndex = 0)
        {
            // if (stateMachine.Health.CurrentStamina <= 0) return;

            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attackIndex));


            // if (stateMachine.InputReader.IsAttack)
            // {
            //     //index 0 to initiate the first attack
            //     stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attackIndex));
            // }
        }

        public void EnterPurificationState(float momentum = 0f)
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.Purification) && !stateMachine.IsGodMode) return;


            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.Purification.CheckIfReady())
                    stateMachine.SwitchState(new PlayerPurificationState(stateMachine, momentum));
            }
            else
                stateMachine.SwitchState(new PlayerPurificationState(stateMachine, momentum));
        }

        public void EnterDefenseState()
        {
            if (stateMachine.InputReader.IsBlocking)
            {
                stateMachine.SwitchState(new PlayerDefensiveState(stateMachine));
            }
        }

        public void EnterOffensiveDodgeState()
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.OffensiveDodge.CheckIfReady())
                    stateMachine.SwitchState(new PlayerOffensiveDodgeState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerOffensiveDodgeState(stateMachine));
        }

        public void EnterDefensiveDodgeState(Vector3 movement)
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.DefensiveDodge.CheckIfReady())
                    stateMachine.SwitchState(new PlayerDefensiveDodgeState(stateMachine, movement));
            }
            else
                stateMachine.SwitchState(new PlayerDefensiveDodgeState(stateMachine, movement));
        }

        public void EnterGroundExecutionState()
        {
            if (stateMachine.PlayerComponents.GroundExecutionPointDetector.SelecClosestExecutionPoint())
                stateMachine.SwitchState(new PlayerGroundExecutionState(stateMachine));

            // stateMachine.HighlightEffect.highlighted = true;
        }

        public void EnterDrinkPotionState()
        {
            if (stateMachine.PlayerComponents.GetHealController().PotionsRemaining())
                stateMachine.SwitchState(new PlayerDrinkPotionState(stateMachine));
        }


        public void EnterBlessedGroundState()
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.BlessedGround) &&
                !stateMachine.IsGodMode) return;

            // if (!stateMachine.PlayerComponents.GetHealController().HealsRemaining()) return;

            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.BlessedGround.CheckIfReady())
                    stateMachine.SwitchState(new PlayerBlessedGroundState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerBlessedGroundState(stateMachine));
        }

        public void EnterHolyShieldState()
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.HolyShield.CheckIfReady())
                    stateMachine.SwitchState(new PlayerHolyShieldState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerHolyShieldState(stateMachine));
        }

        public void EnterHolyChargeState()
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.HolyCharge) && !stateMachine.IsGodMode) return;
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.HolyCharge.CheckIfReady())
                    stateMachine.SwitchState(new PlayerHolyChargeState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerHolyChargeState(stateMachine));
        }

        public void EnterCrusade()
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.Crusade) && !stateMachine.IsGodMode) return;
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.Crusade.CheckIfReady())
                    stateMachine.SwitchState(new PlayerCrusadeState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerCrusadeState(stateMachine));
        }

        public void EnterLeapState()
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.Leap) && !stateMachine.IsGodMode) return;

            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.Leap.CheckIfReady())
                    stateMachine.SwitchState(new PlayerLeapAttackState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerLeapAttackState(stateMachine));
        }

        public void EnterShieldBashState()
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.InputReader.IsWestButton)
                    stateMachine.SwitchState(new PlayerShieldBashState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerShieldBashState(stateMachine));
        }

        public void EnterSprintAttackState()
        {
            stateMachine.SwitchState(new PlayerSprintAttack(stateMachine));
        }

        public void EnterCleaningStrikeState()
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.CleansingStrikes.CheckIfReady())
                    stateMachine.SwitchState(new PlayerCleansingStrikesState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerCleansingStrikesState(stateMachine));
        }

        public void PlayerFallingFromLedge()
        {
            stateMachine.SwitchState(new PlayerFallingFromLedgeState(stateMachine));
        }

        public void EnterCrusadeState()
        {
            if (IsCooldownManagerActive())
            {
                if (stateMachine.PlayerCharacterAttributes.Crusade.CheckIfReady())
                    stateMachine.SwitchState(new PlayerCrusadeState(stateMachine));
            }
            else
                stateMachine.SwitchState(new PlayerCrusadeState(stateMachine));
        }

        public void EnterRangedAttackState(bool isRepeat = false, float momentum = 0)
        {
            if (!playerStatsController.CheckAbility(PlayerAbilityTypes.RangedAttack) && !stateMachine.IsGodMode) return;

            stateMachine.SwitchState(new PlayerRangedAttackState(stateMachine, isRepeat, momentum));
        }

        public void EquipRangedWeapon()
        {
            if (!stateMachine.WeaponInventory.IsRangeEquipped)
            {
                //Switch Weapon State
                stateMachine.WeaponInventory.EquipRangedWeapon();
            }
        }

        public void EquipMeleeWeapons()
        {
            if (stateMachine.WeaponInventory.IsRangeEquipped)
            {
                //equip melee weapons if Ranged, then go to attack
                stateMachine.WeaponInventory.EquipLeftAndRightMelee();
                stateMachine.WeaponHandler.LoadCurrentWeaponDamage();
            }
        }
    }

    public class StateBlocks
    {
        public bool IsCooldownManagerActive()
        {
            return CooldownManager.Instance;
        }
    }
}