using UnityEngine;

namespace Etheral
{
    public class PlayerUIState : PlayerBaseState
    {
        public PlayerUIState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("Idle");
        }

        public override void Tick(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        public override void Exit() { }
    }
}