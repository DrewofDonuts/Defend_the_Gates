using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/Overrides/Unbound Guard Override", fileName = "Unbound Guard Override")]
    public class UnboundGuardAIStateOverrides : BaseAIStateOverrides
    {
        public override void IdleOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new UnboundGuardIdleState(stateMachine as EnemyStateMachine));
        }
    }
}