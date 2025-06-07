using Etheral.Cursed_King;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Cursed King Override",
        menuName = "Etheral/Characters/AI/Overrides/Cursed King Override")]
    public class CursedKingStateOverride : BaseAIStateOverrides
    {
        public override void IdleOverrideState<T>(T stateMachine)
        {
            stateMachine.SwitchState(new CursedKingIdleState(stateMachine as EnemyStateMachine));
        }

        public override void SpecialAttackOverrideState<T>(T stateMachine, int attackIndex = 0)
        {
            stateMachine.SwitchState(new CursedKingSpecialAttackState(stateMachine as EnemyStateMachine, attackIndex));
        }

        public override void CounterAttackOverrideState<T>(T stateMachine, int attackIndex = 0)
        {
            var random = Random.Range(0, 2);

            if (random == 0)
                stateMachine.SwitchState(new CursedKingSpecialAttackState(stateMachine as EnemyStateMachine,
                    attackIndex));
            else
                stateMachine.SwitchState(new CursedKingSummonWraithState(stateMachine as EnemyStateMachine));
        }
    }
}