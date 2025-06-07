using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable]
    public class AITestingControl
    {
        public bool displayStateIndicator;
        public bool idleAndImpactOnly;
        public bool blockSwitchState;
        public bool blockAttack;
        public bool blockCounterAttack;
        public bool blockChase;
        public bool blockRotate;
        public bool blockMovement;
        [field: SerializeField] public int strafeDirection { get; set; }

        EnemyStateMachine _stateMachine;
        bool[] testStates;


        public AITestingControl(EnemyStateMachine stateMachine)
        {
            testStates = new[]
            {
                idleAndImpactOnly, blockSwitchState, blockAttack, blockCounterAttack, blockChase, blockRotate,
                blockMovement
            };

            foreach (var _testState in testStates)
            {
                if (_testState)
                {
                    Debug.Log(_stateMachine.name + " is in test state: " + _testState);
                }
            }
        }

        // public void SetStateMachine(EnemyStateMachine stateMachine)
        // {
        //     _stateMachine = stateMachine;
        // }
    }
}