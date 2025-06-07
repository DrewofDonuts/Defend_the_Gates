using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [Obsolete]
    public class TestEnemyActionState : EnemyCounterActionState
    {
        public TestEnemyActionState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick(float deltaTime)
        {
            Debug.Log("THIS IS THE TESTING STATE");
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override State EnterState(EnemyStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}
