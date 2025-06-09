namespace Etheral
{
    public class NPCEventState : NPCBaseState
    {
        string animation;

        public NPCEventState(CompanionStateMachine companionStateMachine, string animation) : base(companionStateMachine)
        {
            this.animation = animation;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(animation, CrossFadeDuration);
            stateMachine.GetAIComponents().GetDialogueSystemController().DisableSystemTrigger();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
        }

        public override void Exit()
        {
            stateMachine.GetAIComponents().GetDialogueSystemController().EnableSystemTrigger();
        }
    }
}