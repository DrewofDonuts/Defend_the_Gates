using UnityEngine;

namespace Etheral
{
    public class PlayerShieldBreakState : PlayerBaseState
    {
        readonly int ShieldBreak = Animator.StringToHash("ShieldBreak");

        public PlayerShieldBreakState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ShieldBreak, CrossFadeDuration);
           stateMachine.Health.SetIsInvulnerable(true);

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "ShieldBreak");

            if (normalizedTime >= 1f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetIsInvulnerable(false);

        }
    }
}