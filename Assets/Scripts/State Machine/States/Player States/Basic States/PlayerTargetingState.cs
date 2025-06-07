using System;
using UnityEngine;

namespace Etheral
{
    [Obsolete("Can likelky remove. Old method of handling player defensive state")]
    public class PlayerTargetingState : PlayerBaseState
    {

        static readonly int TargetingStateHash = Animator.StringToHash("TargetingBlendTree");
        static readonly int ForwardHash = Animator.StringToHash("TargetingForward");
        static readonly int RightHash = Animator.StringToHash("TargetingRight");


        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.InputReader.CancelEvent += OnCancel;
            stateMachine.InputReader.EastButtonEvent += OnEastButton;

            stateMachine.InputReader.OnTargetEvent += OnCancel;

            stateMachine.Animator.CrossFadeInFixedTime(TargetingStateHash, 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (stateMachine.InputReader.IsAttack)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
                return;
            }

            if (stateMachine.InputReader.IsBlocking)
            {
                stateMachine.SwitchState(new PlayerDefensiveState(stateMachine));
                return;
            }

            if (stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget == null)
            {
                stateMachine.SwitchState(new PlayerOffensiveState(stateMachine));
            }

            Vector3 movement = CalculateMovement(deltaTime);
            Move(movement * stateMachine.PlayerCharacterAttributes.StrafeSpeed, deltaTime);

            UpdateAnimator(deltaTime);

            RotateTowardsTarget();

            //not moving
            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                //idle
                stateMachine.Animator.SetFloat(ForwardHash, 0, AnimatorDampTime, deltaTime);
                stateMachine.Animator.SetFloat(RightHash, 0, AnimatorDampTime, deltaTime);
                return;
            }
        }

        void UpdateAnimator(float deltaTime)
        {
            var movementX = stateMachine.InputReader.MovementValue.x;
            var movementY = stateMachine.InputReader.MovementValue.y;

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                //idle
                stateMachine.Animator.SetFloat(ForwardHash, 0, AnimatorDampTime, deltaTime);
                stateMachine.Animator.SetFloat(RightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                stateMachine.Animator.SetFloat(ForwardHash, movementY, AnimatorDampTime, deltaTime);
                stateMachine.Animator.SetFloat(RightHash, movementX, AnimatorDampTime, deltaTime);
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.CancelEvent -= OnCancel;
            stateMachine.InputReader.EastButtonEvent -= OnEastButton;
            stateMachine.InputReader.OnTargetEvent -= OnCancel;
        }

        void OnEastButton()
        {
            if (stateMachine.InputReader.MovementValue == Vector2.zero) return;
            stateMachine.SwitchState(new PlayerOffensiveDodgeState(stateMachine));
        }

        void OnCancel()
        {
            stateMachine.PlayerComponents.LockOnController.Cancel();
            stateMachine.SwitchState(new PlayerOffensiveState(stateMachine));
        }

        Vector3 CalculateMovement(float deltaTime)
        {
            var movement = new Vector3();

            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

            return movement;
        }
    }
}