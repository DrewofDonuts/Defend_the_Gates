using UnityEngine;

namespace Etheral
{
    public class SimplePlayerIdleState : SimplePlayerBaseState
    {
        public SimplePlayerIdleState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Idle, .2f);
            RegisterEvents();
        }


        public override void Tick(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
            
            if (GetInputType() is InputType.gamePad && GetIsMouseAiming())
                CheckAndSetMouseAiming(false);

            SwitchToPlayerOffensiveStateIfMoving();
        }


        public override void Exit()
        {
            DeRegisterEvents();
        }

        void SwitchToPlayerOffensiveStateIfMoving()
        {
            if (playerComponents.GetInput().MovementValue.magnitude > 0.1f)
            {
                stateMachine.SwitchState(new SimplePlayerMovementState(stateMachine));
            }
        }
    }
}