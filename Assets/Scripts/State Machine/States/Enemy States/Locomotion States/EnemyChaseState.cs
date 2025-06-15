using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class EnemyChaseState : EnemyBaseState
    {
        protected const float ChaseDampTime = 0.1f;
        protected const float TimeBeforeStrafe = .1f;
        protected const float TimeBeforeAttack = 0.2f;
        protected WaitForSeconds waitBeforeGroundAttack = new(1f);


        protected float timer;

        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            // _enemyStateBlocks = new EnemyStateBlocks(base.stateMachine);
        }

        public override void Enter()
        {
            Debug.Log("Entering Chase State");
            stateMachine.OnChangeStateMethod(StateType.Chase);
            aiComponents.GetNavMeshAgentController().SetRotation(true);
            aiComponents.GetNavMeshAgentController().GetAgent().angularSpeed = enemyStateMachine.AIAttributes.RotateSpeed;


            animationHandler.CrossFadeInFixedTime(Locomotion);
            if (enemyStateMachine.AIAttributes.CanBlock && enemyStateMachine.Health.CurrentDefense > 0)
                enemyStateMachine.Health.SetBlocking(true);

            if (enemyStateMachine.stateIndicator != null)
                enemyStateMachine.stateIndicator.color = Color.blue;
            
            aiComponents.GetNavMeshAgentController().SetAgentSpeed(enemyStateMachine.AIAttributes.RunSpeed);
        }


        
        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            //Disabled for testing to have navmesh agent rotate 06/15/2025
            // RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);
            HandleSwitchingToStrafe(deltaTime);


            // if (aiComponents.GetNavMeshAgentController().GetAgent().isOnOffMeshLink)
            // {
            //     enemyStateMachine.SwitchState(new EnemyCrossLinkJumpState(enemyStateMachine));
            //     return;
            // }


            Move(GetCurrentTargetPosition(),
                enemyStateMachine.AIAttributes.RunSpeed, deltaTime);


            if (IsInMeleeRange() && !CheckPriorityAndTokenBeforeActions())
            {

                // Decelerate(deltaTime);
                if (!stateMachine.AIAttributes.CanStrafe && movementSpeed < 0.1f)
                {
                    Debug.Log("Switching to Base State from Chase");
                    enemyStateBlocks.SwitchToBaseState();
                    return;
                }

                if (stateMachine.AIAttributes.CanStrafe && movementSpeed < 0.1f)
                {
                    enemyStateBlocks.SwitchToStrafe();
                    return;
                }
            }

            if (IsInRangedRange() || IsInMeleeRange() || IsDistanceMeleeAndIsReady())
            {
                if (CheckPriorityAndTokenBeforeActions())
                {
                    timer += deltaTime;

                    // Decelerate(deltaTime);
                    if (timer > TimeBeforeAttack)
                    {
                        if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                            return;
                    }
                }
            }
            else
                timer = 0;


            animationHandler.SetFloatWithDampTime(ForwardSpeed, movementSpeed, ChaseDampTime, deltaTime);


            if (!IsInChaseRangeTarget())
            {
                enemyStateBlocks.SwitchToBaseState();
                return;
            }
        }

        protected void HandleSwitchingToStrafe(float deltaTime)
        {
            if (IsInStrafeRange() && enemyStateBlocks.CanStrafe())
                enemyStateBlocks.TryToStrafe(TimeBeforeStrafe, deltaTime);
            else if (!IsInStrafeRange() && enemyStateBlocks.CanStrafe())
                enemyStateBlocks.ResetTimer();
        }


        public override void Exit()
        {
            enemyStateMachine.StopAllCoroutines();
            aiComponents.GetNavMeshAgentController().SetRotation(false);
            
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            enemyStateMachine.GetAIComponents().navMeshAgentController.SetVelocityToKeepInSyncWithCC(
                Vector3.zero);

            enemyStateMachine.Health.SetBlocking(false);
        }
    }
}