using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class PlayerCleansingStrikesState : PlayerBaseState
    {
        public PlayerCleansingStrikesState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            characterAction = GetCharacterAction(stateMachine.PlayerCharacterAttributes.CleansingStrikes);

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);

            actionProcessor.InitializeForcesList();
            actionProcessor.InitializeSpellsList();

        }


        public override void Tick(float deltaTime)
        {
            if (stateMachine.PlayerComponents.LockOnController.SelectTarget())
            {
                HasTarget();
                RotateTowardsTarget();
            }

            Move(deltaTime);
            var rotation = CalculateMovementAgainstCamera();

            FaceMovementDirection(rotation, deltaTime);

            var normalizedValue = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);


            if (normalizedValue >= 1)
                ReturnToLocomotion();

            actionProcessor.ApplyForceTimes(normalizedValue);
            actionProcessor.RightWeaponTimes(normalizedValue);
            actionProcessor.CastSpells(normalizedValue);


            //Disabled for Testing
            // ApplyForces(normalizedValue);
            // CastSpell(normalizedValue);


            // RightWeaponTimes(normalizedValue);
        }

        public override void Exit()
        {
            StartCooldown();
        }
    }
}