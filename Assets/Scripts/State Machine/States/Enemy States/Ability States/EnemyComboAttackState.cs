using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class EnemyComboAttackState : EnemyBaseState
    {
        bool isCounterAttack;
        EnemyStateBlocks enemyStateBlocks;

        public EnemyComboAttackState(EnemyStateMachine __stateMachine, bool isCounterAttack = false) : base(
            __stateMachine)
        {
            this.isCounterAttack = isCounterAttack;
        }

        public override void Enter()
        {
            Debug.LogError("Needs to be updated to use the new animation and action systems");

            if (isCounterAttack)
                characterAction = enemyStateMachine.AIAttributes.CounterCharacterAction;

            if (enemyStateBlocks == null)
                enemyStateBlocks = new EnemyStateBlocks(enemyStateMachine, this);

            // enemyStateBlocks = new EnemyStateBlocks(stateMachine);

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);

            if (isCounterAttack)
                enemyStateMachine.Health.SetSturdy(true);

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, characterAction.AnimationName);

            actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.RightWeaponTimes(normalizedTime);


            if (normalizedTime >= 1)
                enemyStateBlocks.CheckLocomotionStates();
        }

        public override void Exit()
        {
            if (isCounterAttack)
                enemyStateMachine.Health.SetSturdy(false);
        }
    }
}