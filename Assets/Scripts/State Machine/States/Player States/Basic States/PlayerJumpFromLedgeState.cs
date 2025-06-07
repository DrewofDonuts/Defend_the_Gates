using UnityEngine;

namespace Etheral
{
    public class PlayerJumpFromLedgeState : PlayerBaseState
    {
        bool hasLanded;

        public PlayerJumpFromLedgeState(PlayerStateMachine stateMachine, ClimbAndParkourData climbAndParkourData) :
            base(
                stateMachine)
        {
            climbAndParkour = climbAndParkourData;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(climbAndParkour.animName, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            MoveWhenFalling(stateMachine.transform.forward * 2.4f, deltaTime);

            float jumpNormalizedTime = GetNormalizedTime(stateMachine.Animator, climbAndParkour.animName);

            if (jumpNormalizedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerFallingFromLedgeState(stateMachine));
            }
        }


        public override void Exit()
        {
            stateMachine.GetCharComponents().GetCC().enabled = true;
            stateMachine.Animator.applyRootMotion = false;
        }
    }
    
}