namespace Etheral
{
    public class PlayerClimbsOnTopFromHangingState : PlayerBaseClimbingState
    {
        bool hasMounted;

        public PlayerClimbsOnTopFromHangingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.ToggleGravity(true);
            stateMachine.GetCharComponents().GetCC().enabled = true;
            stateMachine.Animator.CrossFadeInFixedTime("ClimbOverWall", 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "ClimbOverWall");

            if (normalizedTime >= 1f && !hasMounted)
            {
                hasMounted = true;
                stateMachine.Animator.CrossFadeInFixedTime("CrouchedToStand", 0.2f);
            }

            var crouchedToStandNormalizedTime = GetNormalizedTime(stateMachine.Animator, "CrouchedToStand");


            if (crouchedToStandNormalizedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerOffensiveState(stateMachine, true));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Animator.applyRootMotion = false;
        }
    }
}