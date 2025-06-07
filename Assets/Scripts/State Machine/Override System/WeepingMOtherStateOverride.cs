using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/Overrides/Weeping Mother State Override", fileName = "Weeping Mother Override")]
    public class WeepingMOtherStateOverride : BaseAIStateOverrides
    {
        public override void StartingOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new WeepingMotherStartingState(stateMachine as EnemyStateMachine));
        }
        
        public override void IdleOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new WeepingMotherIdleState(stateMachine as EnemyStateMachine));
        }
    }
}