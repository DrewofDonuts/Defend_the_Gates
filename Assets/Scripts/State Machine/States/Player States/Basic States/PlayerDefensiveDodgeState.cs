using System.Collections;
using System.Collections.Generic;
using Etheral;
using UnityEngine;

namespace Etheral
{
    public class PlayerDefensiveDodgeState : PlayerBaseState
    {
        Vector3 dodgingDirectionInput;
        Transform strafeCamera;
        Vector3 strafeRelativeToCamera;

        float remainingDodgeTime = 1f;

        static readonly int ForwardDodge = Animator.StringToHash("DodgeForward");
        static readonly int RightDodge = Animator.StringToHash("DodgeRight");
        static readonly int RollBlendTreeHash = Animator.StringToHash("RollBlendTree");

        public PlayerDefensiveDodgeState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(
            stateMachine)
        {
            this.dodgingDirectionInput = dodgingDirectionInput;
        }

        public override void Enter()
        {
            strafeCamera = stateMachine.PlayerComponents.MainCameraTransform;
            characterAction = stateMachine.PlayerCharacterAttributes.DefensiveDodge;

            animationHandler.CrossFadeInFixedTime(RollBlendTreeHash);


            remainingDodgeTime = stateMachine.PlayerCharacterAttributes.DashDuration;
            // CooldownManager.Instance.StartCooldown(characterAction);
            StartCooldown();
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovementAgainstCamera();
            Move(movement * stateMachine.PlayerCharacterAttributes.DefensiveDodge.Forces[0], deltaTime);

            // RotateTowardsTarget(50);
            ConvertAnimation();

            remainingDodgeTime -= deltaTime;

            if (remainingDodgeTime <= 0f)
            {
                ReturnToLocomotion();
            }
        }

        void ConvertDirection()
        {
            var cameraForward = strafeCamera.forward;
            var cameraRight = strafeCamera.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            strafeRelativeToCamera = cameraForward * dodgingDirectionInput.y +
                                     cameraRight * dodgingDirectionInput.x;
        }

        void ConvertAnimation() //TODO should live in AnimatorController
        {
            float targetVertical = stateMachine.InputReader.MovementValue.y;
            float targetHorizontal = stateMachine.InputReader.MovementValue.x;

            var cameraUp = strafeCamera.up * targetVertical;
            var cameraRight = strafeCamera.right * targetHorizontal;

            cameraUp.y = 0;
            cameraRight.y = 0;

            var movement = (cameraRight + cameraUp);
            strafeRelativeToCamera = stateMachine.transform.InverseTransformDirection(movement);

            float localX = strafeRelativeToCamera.x;
            float localZ = strafeRelativeToCamera.z;


            stateMachine.Animator.SetFloat(RightDodge, localX);
            stateMachine.Animator.SetFloat(ForwardDodge, localZ);
        }

        public override void Exit()
        {
            stateMachine.Health.SetBlocking(false);
        }
    }
}