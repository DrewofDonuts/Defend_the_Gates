using UnityEngine;

namespace Etheral
{
    public class SimplePlayerInjuredIdleState : SimplePlayerBaseState
    {
        public SimplePlayerInjuredIdleState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("IdleInjured", .2f);
            RegisterEvents();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

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
                Debug.Log("Should Switch to Injured Move");
                stateMachine.SwitchState(new SimplePlayerInjuredMoveState(stateMachine));
            }
        }
    }
}