using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    //REMEMBER - only use the right weapon array since this doesn't have to enable any weapons
    public class EnemyGroundAttackState : EnemyBaseState
    {
        bool isGrundAttacking;
        List<bool> appliedDamage;
        int attackIndex;
        Vector3 playerPosition;
        bool isPlayerOnGround = true;
        DamageData damageData;

        public EnemyGroundAttackState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            EventBusPlayerStatesToDeprecate.OnPlayerChangeState += OnPlayerChangeStateChanged;
            isGrundAttacking = true;
            characterAction = enemyStateMachine.AIAttributes.GroundAttack;
            EventBusPlayerController.IsGroundAttacking(enemyStateMachine, true);

            playerPosition = enemyStateMachine.GetPlayerPosition();

            animationHandler.CrossFadeInFixedTime(characterAction);

            appliedDamage = new List<bool>();

            for (int i = 0; i < characterAction.EnableRightWeapon.Length; i++)
            {
                appliedDamage.Add(false);
            }

            ConfigureDamage();
        }

        void ConfigureDamage()
        {
            damageData = new DamageData();
            damageData.damage = characterAction.Damage;
            damageData.audioImpact = characterAction.AudioImpact;
        }

        public override void Tick(float deltaTime)
        {
            if (enemyStateMachine == null) return;
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (GetPlayerDistance() > characterAction.AdjacentDistance &&
                normalizedTime >= characterAction.TimesBeforeForce[0] &&
                normalizedTime <= characterAction.TimesBeforeForce[1])
                Move(playerPosition, characterAction.Forces[0], deltaTime);

            RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);
            EnableGroundAttack(normalizedTime);

            if (normalizedTime >= 1)
            {
                if (isPlayerOnGround && enemyStateMachine.AIAttributes.HasRetreat)
                    enemyStateBlocks.SwitchToJumpBack(enemyStateMachine);
                else
                    enemyStateBlocks.CheckLocomotionStates();
            }
        }

        void OnPlayerChangeStateChanged(StateType obj)
        {
            if (obj != StateType.KnockedDown)
                isPlayerOnGround = false;

            // enemyStateBlocks.CheckLocomotionStates();
        }


        public void EnableGroundAttack(float normalizedValue)
        {
            if (characterAction.EnableRightWeapon.Length == 0) return;

            if (attackIndex >= characterAction.EnableRightWeapon.Length) return;

            if (normalizedValue >= characterAction.EnableRightWeapon[attackIndex])
            {
                EventBusPlayerController.IsGroundAttacking(enemyStateMachine, true);

                if (!appliedDamage[attackIndex])
                {
                    if (EventBusPlayerController.PlayerStateMachine.StateType == StateType.KnockedDown &&
                        GetPlayerDistance() < 2f)
                    {
                        EventBusPlayerController.InjurePlayer(enemyStateMachine, damageData);
                        EventBusPlayerController.FeedbackBasedOnDistanceFromPlayer(this,
                            enemyStateMachine.transform.position, characterAction.FeedbackType);
                    }

                    appliedDamage[attackIndex] = true;
                }

                attackIndex++;

                if (attackIndex >= characterAction.EnableRightWeapon.Length)
                {
                    attackIndex = 0;
                    EventBusPlayerController.IsGroundAttacking(enemyStateMachine, false);
                }
            }
        }

        public override void Exit()
        {
            EventBusPlayerController.IsGroundAttacking(enemyStateMachine, false);
            EventBusPlayerStatesToDeprecate.OnPlayerChangeState -= OnPlayerChangeStateChanged;
        }
    }
}