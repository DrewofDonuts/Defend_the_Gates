using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    [Serializable]
    public class AICounterAttackSelector
    {
        [FormerlySerializedAs("Action")]
        [field: Tooltip("ComboAttack, LeapBack, ShieldBash, Whirlwind")]
        [SerializeField] public string action;

        [ReadOnly]
        [SerializeField] List<string> actionNames = new List<string>()
            { "ComboAttack", "LeapBack", "ShieldBash", "Whirlwind" };

        // ReSharper disable Unity.PerformanceAnalysis
        public void CounterStateSelector(EnemyStateMachine stateMachine)
        {
            if (action == "Whirlwind")
                stateMachine.SwitchState(new EnemyWhirlwindState(stateMachine, true));
            if (action == "LeapBack")
                stateMachine.SwitchState(new EnemyJumpBackState(stateMachine, true));
            if (action == "ShieldBash")
                stateMachine.SwitchState(new EnemyShieldBashState(stateMachine, true));
            if (action == "ComboAttack")
                stateMachine.SwitchState(new EnemyComboAttackState(stateMachine, true));
        }
    }
}