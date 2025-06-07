using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/Overrides/Splitter State Overrides",
        fileName = "Splitter State Overrides")]
    public class SplitterOverrideState : BaseAIStateOverrides
    {
        public override void RangedOverrideState<T>(T stateMachine)
        {
            Debug.Log("Ranged Override State");
            stateMachine.SwitchState(new EnemyRapidRangedAttackState(stateMachine as EnemyStateMachine));
        }
    }
}