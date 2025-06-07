using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/Overrides/Flesh Warden State Overrides", fileName = "Flesh Warden State Overrides")]
    public class FleshWardenStateOverrides : BaseAIStateOverrides
    {
        public override void IdleOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new FleshWardenIdleState(stateMachine as EnemyStateMachine));
        }
    }
}