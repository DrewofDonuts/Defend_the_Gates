using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class EnemyStateBlocks : StateBlocks
    {
        float timer;
        readonly EnemyBaseState enemyBaseState;
        readonly EnemyStateMachine enemyStateMachine;

        public EnemyStateBlocks(EnemyStateMachine _enemyStateMachine, EnemyBaseState _enemyBaseState)
        {
            enemyStateMachine = _enemyStateMachine;
            enemyBaseState = _enemyBaseState;
        }

        #region Check From Other States
        public bool CheckAttacksFromLocomotionState()
        {
            if (enemyStateMachine.AITestingControl.blockSwitchState) return false;
            if (enemyStateMachine.AITestingControl.blockAttack) return false;


            if (enemyBaseState.IsDistanceMeleeAndIsReady() && enemyStateMachine.AIAttributes.isMelee)
            {
                SwitchToDistanceMelee();
                return true;
            }

            if (enemyBaseState.IsInMeleeRange() && enemyStateMachine.AIAttributes.isMelee)
            {
                SwitchToMeleeAttack(0);
                return true;
            }


            if (enemyBaseState.IsInRangedRange() && enemyStateMachine.AIAttributes.isRanged)
            {
                SwitchToRangedAttack();
                return true;
            }

            return false;
        }

        public void CheckLocomotionFromImpactState()
        {
            if (enemyStateMachine.AITestingControl.idleAndImpactOnly)
            {
                SwitchToBaseState();
                return;
            }

            if (enemyBaseState.CheckPriorityAndTokenBeforeActions())
            {
                if (CanStrafe())
                    SwitchToStrafe();
                else if (enemyBaseState.IsInChaseRangeTarget())
                    SwitchToChase();
            }
            else
                SwitchToBaseState();
        }


        public void CheckLocomotionStates()
        {
            if (enemyStateMachine.AITestingControl.idleAndImpactOnly)
            {
                SwitchToBaseState();
                return;
            }

            if (CanStrafe())
                SwitchToStrafe();
            else
                SwitchToBaseState();

            //commented out to consolidate where behavior logic is executed
            // if (!enemyBaseState.IsInChaseRangeTarget())
            //     SwitchToIdle();
            // else if (CanStrafe())
            //     SwitchToStrafe();
            // else if (enemyBaseState.IsInChaseRangeTarget())
            //     SwitchToChase();
        }
        #endregion

        public void ResetTimer() => timer = 0;


        public bool CheckAttackCooldown()
        {
            return enemyStateMachine.AIAttributes.BasicAttacks.All(action => action.CheckIfReady());

            // _stateMachine.CharacterAttributes.Attacks.Length == 0 ||
            // !_enemyBaseState.IsInMeleeRange() && !_stateMachine.CharacterAttributes.isMelee;
        }


        #region Switch To Other State
        public void SwitchToSpawnEnemyState(int spawnNumber = 0)
        {
            enemyStateMachine.SwitchState(new EnemySpawnEnemyState(enemyStateMachine, spawnNumber));
        }

        public void SwitchToSpecialAttack(int attackIndex)
        {
            if (enemyBaseState.overrides?.OverrideSpecialAttackState == true)
                enemyBaseState.overrides.SpecialAttackOverrideState(enemyStateMachine, attackIndex);
        }

        public void SwitchToChase()
        {
            // if (enemyBaseState.IsInChaseRangeTarget())
            enemyStateMachine.SwitchState(new EnemyChaseState(enemyStateMachine));
        }


        public void SwitchToBaseState()
        {
            if (enemyBaseState.overrides?.IsGateAttacking == true &&
                !enemyBaseState.aiComponents.GetAIGateHandler().IsAllGatesDestroyed)
            {
                SwitchToMoveToGate();
                return;
            }

            if (enemyBaseState.overrides?.IsOverrideIdleState == true)
                enemyBaseState.overrides.IdleOverrideState(enemyStateMachine);
            else
                enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
        }

        public void SwitchToStartingState(AIComponentHandler _aiComponents)
        {
            AIComponentHandler aiComponents = _aiComponents;

            if (aiComponents.GetOverrideStateController().CheckIfStartingStateOverride())
                aiComponents.GetOverrideStateController().SwitchToOverrideStartingState(enemyStateMachine);
            else if (aiComponents.GetOverrideStateController().CheckIfIdleOverride())
                aiComponents.GetOverrideStateController().SwitchToOverrideIdleState(enemyStateMachine);
            else
                enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
        }

        void SwitchToMoveToGate()
        {
            enemyStateMachine.SwitchState(new EnemyMoveToGateState(enemyStateMachine));
        }

        public void TryToStrafe(float timeBeforeStrafe, float deltaTime)
        {
            if (enemyStateMachine.AITestingControl.blockSwitchState) return;

            if (enemyStateMachine.AIAttributes.CanStrafe && enemyBaseState.IsInStrafeRange())
            {
                timer += deltaTime;
                if (timer >= timeBeforeStrafe)
                    SwitchToStrafe();
            }
        }

        public void SwitchToGroundAttack()
        {
            enemyStateMachine.SwitchState(new EnemyGroundAttackState(enemyStateMachine));
        }

        public void SwitchToStrafe()
        {
            if (!enemyStateMachine.AIAttributes.CanStrafe) return;

            if (enemyBaseState.IsInStrafeRange())
                enemyStateMachine.SwitchState(new EnemyStrafeState(enemyStateMachine));
        }

        public void SwitchToMeleeAttack(int attackIndex)
        {
            EquipMeleeWeapons();

            if (enemyBaseState.overrides?.OverrideAttackState == true)
                enemyBaseState.overrides.AttackOverrideState(enemyStateMachine, attackIndex);
            else
                enemyStateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine, attackIndex));
        }

        void SwitchToDistanceMelee()
        {
            EquipMeleeWeapons();
            if (enemyBaseState.overrides?.OverrideAttackState == true)
                enemyBaseState.overrides.RangedMeleeOverrideState(enemyStateMachine);
            else
                enemyStateMachine.SwitchState(new EnemyDistanceMeleeState(enemyStateMachine));
        }

        public void SwitchToRangedAttack()
        {
            EquipRangedWeapon();
            if (enemyBaseState.overrides?.OverrideRangedState == true)
            {
                Debug.Log("Switching to Ranged Override State");
                enemyBaseState.overrides.RangedOverrideState(enemyStateMachine);
            }
            else
                enemyStateMachine.SwitchState(new EnemyRangedAttackState(enemyStateMachine));
        }

        public void SwitchToRetreat()
        {
            if (enemyBaseState.overrides?.OverrideRetreatState == true)
                enemyBaseState.overrides.RetreatOverrideState(enemyStateMachine);
            else
                enemyStateMachine.SwitchState(new EnemyRetreatState(enemyStateMachine));
        }

        public void SwitchToJumpBack(bool isCounterAction)
        {
            if (enemyBaseState.overrides?.OverrideRetreatState == true)
                enemyBaseState.overrides.RetreatOverrideState(enemyStateMachine);
            else
                enemyStateMachine.SwitchState(new EnemyJumpBackState(enemyStateMachine, isCounterAction));
        }
        #endregion

        void EquipRangedWeapon()
        {
            if (enemyStateMachine.AIAttributes.EnemyType is EnemyType.RangedMelee &&
                !enemyStateMachine.WeaponInventory.IsRangeEquipped)
            {
                //Switch Weapon State
                enemyStateMachine.WeaponInventory.EquipRangedWeapon();
            }
        }

        void EquipMeleeWeapons()
        {
            if (enemyStateMachine.AIAttributes.EnemyType is EnemyType.RangedMelee &&
                enemyStateMachine.WeaponInventory.IsRangeEquipped)
            {
                //equip melee weapons if Ranged, then go to attack
                enemyStateMachine.WeaponInventory.EquipLeftAndRightMelee();
                enemyStateMachine.WeaponHandler.LoadCurrentWeaponDamage();
            }
        }

        public void ToggleBlock(bool isBlocking)
        {
            enemyStateMachine.Health.SetBlocking(isBlocking);
            Debug.Log("Toggle Block");
        }

        public bool CanStrafe()
        {
            return enemyStateMachine.AIAttributes.CanStrafe && enemyBaseState.IsInStrafeRange();
        }

        public void SwitchToHeal()
        {
            enemyStateMachine.SwitchState(new EnemyHealState(enemyStateMachine));
        }
    }
}