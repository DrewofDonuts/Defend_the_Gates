namespace Etheral
{
    public class PlayerTowerMovementState : PlayerBaseState
    {
        public PlayerTowerMovementState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("FreeLookBlendTree");
            
            stateMachine.InputReader.OnTowerModeEvent += OnTowerModeDown;
        }

        void OnTowerModeDown()
        {
            //Disabling for Testing - 06/12/2025
            return;
            stateMachine.ChangePerspective(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            HandleAllLocomotionAndAnimation(deltaTime);
            
            
            if (movementSpeed <= 0)
            {
                stateMachine.SwitchState(new PlayerTowerIdleState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.OnTowerModeEvent -= OnTowerModeDown;

        }
    }
}