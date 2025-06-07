using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/Overrides/Slasher Override", fileName = "Slasher Override")]
    public class SlasherAIStateOverride : BaseAIStateOverrides
    {
        public List<AIStates> slasherStates;
        public bool specialIdle;
        public string specialIdleAnimation;

        public override void AttackOverrideState<T>(T stateMachine, int attackIndex)
        {
            stateMachine.SwitchState(new SlasherAttackingState(stateMachine as EnemyStateMachine, attackIndex));
        }

        public override void StartingOverrideState<T>(T stateMachine)
        {
            
            if (slasherStates.Contains(AIStates.absoluteChase))
                stateMachine.SwitchState(new EnemyAbsoluteChaseState(stateMachine as EnemyStateMachine));
            
            if (slasherStates.Contains(AIStates.timeline))
                stateMachine.SwitchState(new EnemyTimelineState(stateMachine as EnemyStateMachine));
        }

        public override void IdleOverrideState<T>(T stateMachine)
        {
            if (specialIdle)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine as EnemyStateMachine, specialIdleAnimation));
            }
            else
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine as EnemyStateMachine));
            }
        }
    }

    public enum AIStates
    {
        climbing = 0,
        idle = 1,
        absoluteChase = 2,
        timeline = 3,
    }
}