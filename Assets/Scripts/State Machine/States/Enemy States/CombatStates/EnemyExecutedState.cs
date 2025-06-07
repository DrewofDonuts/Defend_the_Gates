using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class EnemyExecutedState : EnemyBaseState
    {
        int index;
        float timeToWaitbeforeDisappearing = 4f;
        bool isBloodPool;
        bool triggeredExecution;

        public EnemyExecutedState(EnemyStateMachine stateMachine, int index) : base(stateMachine)
        {
            this.index = index;
        }

        public override void Enter()
        {
            MoveEnemyToExecutionPoint();
            stateMachine.GetCharComponents().GetCC().enabled = false;
            enemyStateMachine.GetAIComponents().navMeshAgentController.DisableAgentUpdate();


            animationHandler.CompensateForTimeScale();
            animationHandler.SetRootMotion(true);
            enemyStateMachine.Animator.CrossFadeInFixedTime("Executed" + index, CrossFadeDuration);
            enemyStateMachine.Health.SetIsDead();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, "Executed" + index);

            if (normalizedTime >= 1)
            {
                timeToWaitbeforeDisappearing -= deltaTime;

                if (timeToWaitbeforeDisappearing <= 0)
                    EnemyDisappear(deltaTime);

                if (!triggeredExecution)
                {
                    enemyStateMachine.Health.SetExecution();
                    triggeredExecution = true;
                }


                GameObject.Destroy(enemyStateMachine.gameObject, 5f);
            }
        }

        void MoveEnemyToExecutionPoint()
        {
            var player = enemyStateMachine.GetPlayer();
            var transform = enemyStateMachine.transform;

            transform.position = player.PlayerComponents.ExecutionPoint.position;
            transform.rotation = player.PlayerComponents.ExecutionPoint.rotation;
        }


        public override void Exit()
        {
            animationHandler.SetRootMotion(false);
        }
    }
}