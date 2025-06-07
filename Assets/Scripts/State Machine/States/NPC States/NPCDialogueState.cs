namespace Etheral
{
    public class NPCDialogueState : NPCBaseState
    {
        string animationName;

        public NPCDialogueState(NPCStateMachine stateMachine, string animationName) : base(stateMachine)
        {
            this.animationName = animationName;
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(animationName);
            stateMachine.Health.SetIsInvulnerable(true);
            stateMachine.OnChangeStateMethod(StateType.Idle);
            stateMachine.Health.SetIsInvulnerable(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime("Dialogue");

            if (normalizedTime >= 1)
            {
                stateMachine.SwitchState(new NPCIdleState(stateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}