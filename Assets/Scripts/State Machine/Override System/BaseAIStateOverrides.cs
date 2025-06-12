using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [InlineEditor]
    public abstract class BaseAIStateOverrides : ScriptableObject
    {
        [field: SerializeField] public bool IsGateAttacking { get;  set; } = false;
         
        [field: Header("Locomotion")]
        [field: SerializeField] public bool IsOverrideIdleState { get; private set; }
        [field: SerializeField] public bool OverrideRetreatState { get; private set; }
        [field: SerializeField] public bool IsOverrideStartingState { get; private set; }


        [field: Header("Combat")]
        [field: SerializeField] public bool OverrideAttackState { get; private set; }
        [field: SerializeField] public bool OverrideRangedState { get; private set; }
        [field: SerializeField] public bool OverrideRangedMeleeState { get; private set; }
        [field: SerializeField] public bool OverrideSpecialAttackState { get; private set; }
        [field: SerializeField] public bool OverrideCounterAttackState { get; private set; }
        
        public virtual void IdleOverrideState<T>(T stateMachine) where T : StateMachine { }
        public virtual void AttackOverrideState<T>(T stateMachine, int attackIndex = 0) where T : StateMachine { }

        public virtual void RangedOverrideState<T>(T stateMachine) where T : StateMachine { }
        public virtual void RangedMeleeOverrideState<T>(T stateMachine) where T : StateMachine { }
        public virtual void RetreatOverrideState<T>(T stateMachine) where T : StateMachine { }
        public virtual void StartingOverrideState<T>(T stateMachine) where T : StateMachine { }
        public virtual void SpecialAttackOverrideState<T>(T stateMachine, int attackIndex = 0) where T : StateMachine { }
        public virtual void CounterAttackOverrideState<T>(T stateMachine, int attackIndex = 0) where T : StateMachine { }
        
        
    }
}       