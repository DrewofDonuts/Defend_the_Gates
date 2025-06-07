using UnityEngine;

namespace Etheral
{
    public class PlayerFallingFromLedgeState : PlayerBaseState
    {
        bool hasLanded;
        Vector3 forwardMomentum;
        float airControl = 1.5f;


        public PlayerFallingFromLedgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.GetCharComponents().GetCC().enabled = true;
            stateMachine.Animator.applyRootMotion = false;


            stateMachine.Animator.CrossFadeInFixedTime("Fall", 0.2f);
            forwardMomentum = stateMachine.transform.forward * 2.4f;
            stateMachine.ForceReceiver.SetIsFallingForJump(true);
        }


        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (!stateMachine.ForceReceiver.IsGrounded())
            {
                var direction = CalculateMovementAgainstCamera();
                MoveWhenFalling(forwardMomentum + direction * airControl, deltaTime);
                FaceMovementDirection(direction, deltaTime);
            }
            
            if (stateMachine.ForceReceiver.IsGrounded() && !hasLanded)
            {
                stateMachine.Animator.CrossFadeInFixedTime("JumpLanding", 0.2f);
                hasLanded = true;
            }

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "JumpLanding");

            if (normalizedTime >= 1f && hasLanded)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            stateMachine.ForceReceiver.SetIsFallingForJump(false);
            stateMachine.ForceReceiver.ResetForces();
        }
    }
}