using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Sheala Override", menuName = "Etheral/Characters/AI/Overrides/Sheala Override")]
    public class ShealaStateOveride : BaseAIStateOverrides
    {
        public override void SpecialAttackOverrideState<T>(T stateMachine,
            int attackIndex = 0)
        {
            if (attackIndex == 0)
                stateMachine.SwitchState(new ShealaProjectileState(stateMachine as EnemyStateMachine));
            else if (attackIndex == 1)
                stateMachine.SwitchState(new ShealaHumanBombState(stateMachine as EnemyStateMachine));
        }

        public override void IdleOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new ShealaIdleState(stateMachine as EnemyStateMachine));
        }
    }
}