using UnityEngine;

namespace Etheral
{
    public class PlayerBlockState : PlayerBaseState
    {
        readonly int BlockImpact = Animator.StringToHash("BlockImpact");

        public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Health.SetBlocking(true);
            stateMachine.Animator.CrossFadeInFixedTime(BlockImpact, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);


             direction = CalculateMovementAgainstCamera();
            FaceMovementDirection(direction, deltaTime);


            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "BlockImpact");

            if (normalizedTime >= 1f)
            {
                ReturnToLocomotion();
            }
        }


        public override void Exit()
        {
            stateMachine.Health.SetBlocking(false);
        }
    }
}