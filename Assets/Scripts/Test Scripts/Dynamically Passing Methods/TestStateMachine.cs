using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class TestStateMachine : EnemyStateMachine
    {
        [field: SerializeReference] public EnemyCounterActionState TestActionState { get; private set; }

        EnemyStateMachine _enemyStateMachine;

        void Start()
        {
            SwitchState(TestActionState.EnterState(_enemyStateMachine));
        }

        protected override void HandleTakeHit(IDamage iDamage)
        {
        }

        protected override void HandleBlock(IDamage iDamage)
        {
        }
    }
}