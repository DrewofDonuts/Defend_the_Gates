namespace Etheral
{
    public class NpcKnockedDownState : NPCBaseState
    {
        public NpcKnockedDownState(CompanionStateMachine companionStateMachine) : base(companionStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime("KnockedDown", CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalizedvalue = GetNormalizedTime(stateMachine.Animator, "KnockedDown");

            if (normalizedvalue >= 1)
            {
                stateMachine.SwitchState(new CompanionIdleCombatState(stateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}