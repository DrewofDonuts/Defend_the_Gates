using UnityEngine;

namespace Etheral
{
    public abstract class EnemyBaseLocomotionState : EnemyBaseState
    {
        protected EnemyBaseLocomotionState(EnemyStateMachine _stateMachine) : base(_stateMachine)
        {
        }
        
    }
}