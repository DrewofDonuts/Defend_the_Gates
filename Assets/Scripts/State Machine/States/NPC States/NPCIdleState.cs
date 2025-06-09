using UnityEngine;

namespace Etheral
{
    public class NPCIdleState : NPCBaseState
    {
        public NPCIdleState(CompanionStateMachine companionStateMachine) : base(companionStateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("Idle");
            stateMachine.OnChangeStateMethod(StateType.NPC);
            Debug.Log("Switching to idle state");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}