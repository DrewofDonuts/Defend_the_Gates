using UnityEngine;

namespace Etheral
{
    public class NpcDownedState : NPCBaseState
    {
        bool startedLoop;
        
        readonly int KnockDown = Animator.StringToHash("KnockDown");

        public NpcDownedState(NPCStateMachine npcStateMachine) : base(npcStateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(KnockDown);
            stateMachine.Health.SetSturdy(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            
        }

        public override void Exit()
        {
            stateMachine.Health.SetSturdy(false);
        }
    }
}