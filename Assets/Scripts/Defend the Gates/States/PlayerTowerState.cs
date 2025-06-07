namespace Etheral
{
    public class PlayerTowerState : PlayerBaseActionState
    {
        public PlayerTowerState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("FreeLookBlendTree");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            HandleAllLocomotionAndAnimation(deltaTime);
        }

        public override void Exit() { }
    }
}