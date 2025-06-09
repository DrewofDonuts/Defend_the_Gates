using UnityEngine;

namespace Etheral
{
    public class PlayerTowerIdleState : PlayerBaseState
    {
        public PlayerTowerIdleState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            stateMachine.OnChangeStateMethod(StateType.Idle);
            animationHandler.CrossFadeInFixedTime(Idle, .2f);
            
            stateMachine.InputReader.OnTowerModeEvent += OnTowerModeDown;
        }

        void OnTowerModeDown()
        {
            stateMachine.ChangePerspective(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            HandleAllLocomotionAndAnimation(deltaTime);

            if (GetInputType() is InputType.gamePad && GetIsMouseAiming())
                RotateByMouseTopDown(false);


            Move(Vector3.zero, deltaTime);
            SwitchToPlayerOffensiveStateIfMoving();
        }

        void SwitchToPlayerOffensiveStateIfMoving()
        {
            if (stateMachine.InputReader.MovementValue.magnitude > 0.1f)
            {
                stateMachine.SwitchState(new PlayerTowerMovementState(stateMachine));
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.OnTowerModeEvent -= OnTowerModeDown;
        }
    }
}