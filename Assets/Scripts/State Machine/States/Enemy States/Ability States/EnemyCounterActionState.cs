using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [Obsolete]
    public class EnemyCounterActionState : EnemyBaseState
    {
        static readonly int Indicator = Animator.StringToHash("Indicator");
        
        public EnemyCounterActionState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(Indicator, CrossFadeDuration);
            enemyStateMachine.Health.SetSturdy(true);
        }

        //This state is required when calling an any state from a SerializedReference
        public virtual State EnterState(EnemyStateMachine stateMachine)
        {
            return this;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, "Indicator");

            if (normalizedTime >= 1)
            {
                enemyStateMachine.AIAttributes.CounterAttackSelector.CounterStateSelector(enemyStateMachine);
                
                // _stateMachine.SwitchState(
                //     _stateMachine.CharacterAttributes.EnemyCounterActionState.EnterState(_stateMachine));
            }
        }

        // protected void TryApplyForce(float force)
        // {
        //     // if (alreadyAppliedForce) return;
        //
        //     _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * force);
        //     alreadyAppliedForce = true;
        // }

        public override void Exit()
        {
            // stateMachine.SetCanCounter();
            enemyStateMachine.Health.SetSturdy(false);
        }
    }
}