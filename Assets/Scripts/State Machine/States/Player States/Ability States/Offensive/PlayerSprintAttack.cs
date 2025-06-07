using System;
using System.Collections;
using System.Collections.Generic;
using Etheral;
using UnityEngine;

namespace Etheral
{
    public class PlayerSprintAttack : PlayerBaseState
    {
        // CharacterAction _characterAction;
        float _acceleration = 0.1f;
        float _movementSpeed;

        public PlayerSprintAttack(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _movementSpeed = stateMachine.PlayerCharacterAttributes.SprintSpeed - 4;
            characterAction = stateMachine.PlayerCharacterAttributes.SprintAttack;

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);

            var playEmote = UnityEngine.Random.Range(0, 2);

            if (playEmote == 0)
                stateMachine.CharacterAudio.PlayRandomEmote(stateMachine.CharacterAudio.AudioLibrary.AttackEmote);
            stateMachine.Health.SetSturdy(true);
        }


        public override void Tick(float deltaTime)
        {
            OnRotateTowardsTarget();

            // Move(deltaTime);
            _movementSpeed += Mathf.Min(_acceleration * deltaTime,
                stateMachine.PlayerCharacterAttributes.SprintSpeed + 4);


            var normalizedTime = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);


            Move(stateMachine.transform.forward * 3, deltaTime);


            if (normalizedTime >= .10f)
            {
                stateMachine.ForceReceiver.ResetForces();

                // alreadyAppliedForce = actionProcessor.ApplyForce(alreadyAppliedForce, characterAction.Force);
            }

            actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.RightWeaponTimes(normalizedTime);
            actionProcessor.LeftWeaponTimes(normalizedTime);


            if (normalizedTime is > .70f and < 1f)
                playerBlocks.EnterAttackingState();

            if (normalizedTime >= 1)
            {
                ReturnToLocomotion();
            }
        }


        public override void Exit()
        {
            stateMachine.Health.SetSturdy(true);
        }
    }
}