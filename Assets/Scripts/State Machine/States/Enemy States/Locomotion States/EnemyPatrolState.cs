using UnityEngine;

namespace Etheral
{
    public class EnemyPatrolState : EnemyBaseState
    {
        protected const float ChaseDampTime = 0.1f;
        protected const float TimeBeforeStrafe = .1f;
        protected const float TimeBeforeAttack = 0.2f;
        protected float timer;
        bool isIdle;
        float movementModifier = 1f;
        bool isPatrolOverride;

        public EnemyPatrolState(EnemyStateMachine _stateMachine, bool _setPatrolOverride = false) :
            base(_stateMachine)
        {
            isPatrolOverride = _setPatrolOverride;
        }

        public override void Enter()
        {
            if (CheckPriorityAndTokenBeforeActions())
                if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                    return;


            if (enemyStateMachine.stateIndicator != null)
                enemyStateMachine.stateIndicator.color = Color.magenta;

            patrolController.StartPatrolling(isPatrolOverride);

            stateMachine.OnChangeStateMethod(StateType.Patrol);

            animationHandler.CrossFadeInFixedTime(Locomotion);
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }


        protected void HandleIdleAndMoving(float deltaTime, string idle = "Idle",
            string move = "Locomotion")
        {
            isIdle = stateMachine.GetAIComponents().GetNavMeshAgentController().GetDesiredVelocityNormalized().magnitude <
                     0.1f;

            if (isIdle && !animationHandler.GetIfCurrentOrNextState(idle, 0))
            {
                movementModifier = 0;
                animationHandler.CrossFadeInFixedTime(idle);
            }

            if (!isIdle && !animationHandler.GetIfCurrentOrNextState(move, 0))
            {
                movementModifier = 1;
                animationHandler.CrossFadeInFixedTime(move);
            }
        }

        public override void Tick(float deltaTime)
        {
            
            
            
            Move(deltaTime);

            HandleIdleAndMoving(deltaTime);
            animationHandler.SetFloatWithDampTime(ForwardSpeed, movementSpeed * movementModifier, .2f, deltaTime);


            Move(patrolController.GetTargetPoint(), enemyStateMachine.AIAttributes.WalkSpeed, deltaTime);
            RotateTowardsAnything(patrolController.GetTargetPoint(), enemyStateMachine.AIAttributes.RotateSpeed);


            if (IsInChaseRangeTarget())
            {
                enemyStateBlocks.SwitchToBaseState();
                return;
            }

            // if (IsInRangedRange() || IsInMeleeRange() || IsDistanceMeleeAndIsReady())
            // {
            //     if (CheckForTokenBeforeAction())
            //     {
            //         timer += deltaTime;
            //         Decelerate(deltaTime);
            //         if (timer > TimeBeforeAttack)
            //         {
            //             if (enemyStateBlocks.CheckAttacksFromLocomotionState())
            //                 return;
            //         }
            //     }
            // }
            // else
            //     timer = 0;


            // animationHandler.SetFloat(ForwardSpeed, movementSpeed, ChaseDampTime, deltaTime);
        }

        public override void Exit()
        {
            patrolController.StopPatrolling();
        }
    }
}