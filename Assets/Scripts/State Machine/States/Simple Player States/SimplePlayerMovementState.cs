using PixelCrushers.DialogueSystem;

namespace Etheral
{
    public class SimplePlayerMovementState : SimplePlayerBaseState
    {
        public SimplePlayerMovementState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("FreeLookBlendTree");
            
            RegisterEvents();
        }
        

        public override void Tick(float deltaTime)
        {
            if (DialogueManager.isConversationActive) return;

            Move(deltaTime);
            HandleAllLocomotionAndAnimation(deltaTime);

            if (movementSpeed <= 0)
            {
                stateMachine.SwitchState(new SimplePlayerIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            DeRegisterEvents();
        }
    }
}