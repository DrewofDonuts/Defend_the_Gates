using System.Collections;
using System.Collections.Generic;
using Etheral.Combat;
using UnityEngine;

namespace Etheral
{
    public class PlayerExecutingState : PlayerBaseState
    {
        EnemyStateMachine _enemyStateMachine;
        CharacterAction _characterAction;
        ExecutionProcessor _executionProcessor;

        public PlayerExecutingState(PlayerStateMachine stateMachine, EnemyStateMachine enemyStateMachine) : base(
            stateMachine)
        {
            _enemyStateMachine = enemyStateMachine;
        }

        public override void Enter()
        {
            HasTarget();

            var index = Random.Range(0, stateMachine.PlayerCharacterAttributes.SwordShieldExecutions.Length);
            // var index = 0;

            _characterAction = stateMachine.PlayerCharacterAttributes.SwordShieldExecutions[index];
            _executionProcessor = new ExecutionProcessor(stateMachine, _characterAction, _enemyStateMachine);

            StartBulletTime(.10f, 0.10f);
            stateMachine.Health.SetSturdy(true);

            animationHandler.CompensateForTimeScale();
            animationHandler.SetRootMotion(true);

            stateMachine.Animator.CrossFadeInFixedTime("Execution" + (index + 1), CrossFadeDuration);

            _enemyStateMachine.HandleExecution(index + 1);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, _characterAction.AnimationName);
            
            _executionProcessor.EnablingAudio(normalizedTime);
            _executionProcessor.EnablingVFX(normalizedTime);

            if (normalizedTime >= 1f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            animationHandler.SetRootMotion(false);
            animationHandler.ResetAnimatorSpeed();
            stateMachine.Health.SetSturdy(false);
            EndBulletTime();
        }
    }
}


// void PassAttackBasedDamageRight()
// {
//     _stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(attack.Damage,
//         attack.KnockBackForce,
//         attack.KnockDownForce, attack.IsShieldBreak, attack.AttackType);
// }
//
// void PassAttackBasedDamageLeft()
// {
//     _stateMachine.WeaponHandler._currentLeftHandDamage.SettAttackStatDamage(attack.Damage,
//         attack.KnockBackForce,
//         attack.KnockDownForce, attack.IsShieldBreak, attack.AttackType);
// }