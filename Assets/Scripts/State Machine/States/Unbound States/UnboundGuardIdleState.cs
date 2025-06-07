using UnityEngine;

namespace Etheral
{
    public class UnboundGuardIdleState : EnemyIdleState
    {
        int attackSelector;

        public UnboundGuardIdleState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entering Unbound Guard Idle State");

            attackSelector = Random.Range(0, 3);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
        }

        public override void Exit() { }
    }
}