using UnityEngine;

namespace Etheral
{
    public class PlayerDismountFromHangState : PlayerBaseClimbingState
    {
        bool hasLanded;

        public PlayerDismountFromHangState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime("JumpFromHang", 0.2f);

        }

        public override void Tick(float deltaTime)
        {
            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "JumpFromHang");

            if (normalizedTime >= 1f)
            {
                playerBlocks.PlayerFallingFromLedge();
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.ForceReceiver.ResetForces();

        }
    }
}
