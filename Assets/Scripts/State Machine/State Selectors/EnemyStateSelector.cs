using UnityEngine;

namespace Etheral
{
    public class EnemyStateSelector : AIStateSelector
    {
        [SerializeField] EnemyStateMachine stateMachine;

        public override void ToggleHostile(bool value)
        {
        }

        public override void EnterPatrolState()
        {
        }

        public override void EnterChaseState() => stateMachine.SwitchState(new EnemyChaseState(stateMachine));
        

        public override void EnterFleeState()
        {
        }

        public override void EnterCombatState()
        {
        }
        public override void EnterFollowState()
        {
        }
        
        public override void EnterDialogueStateWithAnimation(string animationName)
        {
        }

        public override void EnterIdleState()
        {
        }

        public override void EnterDeadState()
        {
        }
    }
}