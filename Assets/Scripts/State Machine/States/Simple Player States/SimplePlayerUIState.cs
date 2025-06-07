using UnityEngine;

namespace Etheral
{
    public class SimplePlayerUIState : SimplePlayerBaseState
    {
        public SimplePlayerUIState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Idle, .2f);
        }

        public override void Tick(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        public override void Exit() { }
    }
}