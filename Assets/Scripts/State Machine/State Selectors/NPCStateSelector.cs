using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class NPCStateSelector : AIStateSelector
    {
        [FormerlySerializedAs("npcStateMachine")] [SerializeField]
        NPCStateMachine stateMachine;

        public override void EnterDialogueStateWithAnimation(string animationName)
        {
        }

        public override void EnterIdleState()
        {
            stateMachine.SwitchState(new NPCIdleCombatState(stateMachine));
        }

        public override void EnterDeadState()
        {
        }

        public override void ToggleHostile(bool value)
        {
        }

        public override void EnterPatrolState()
        {
        }

        public override void EnterChaseState()
        {
        }

        public override void EnterFleeState()
        {
        }

        public override void EnterCombatState()
        {
        }

        public override void EnterFollowState()
        {
        }

        public override void EnterEventState(string animation)
        {
            stateMachine.SwitchState(new NPCEventState(stateMachine, animation));
        }
    }
}