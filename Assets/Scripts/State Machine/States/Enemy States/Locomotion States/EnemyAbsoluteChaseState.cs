using UnityEngine;

namespace Etheral
{
    public class EnemyAbsoluteChaseState : EnemyChaseState
    {
        public EnemyAbsoluteChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }


        public override void Tick(float deltaTime)
        {
            // Move(deltaTime);


            animationHandler.SetFloatWithDampTime(ForwardSpeed, movementSpeed, ChaseDampTime, deltaTime);

            Move(enemyStateMachine.GetPlayerPosition(), enemyStateMachine.AIAttributes.RunSpeed, deltaTime);


            HandleSwitchingToStrafe(deltaTime);


            if (IsInMeleeRange() && !CheckPriorityAndTokenBeforeActions())
            {
                Debug.Log("If in Melee Range and no token");

                // Decelerate(deltaTime);

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

            if (GetPlayerDistance() > 5f)
            {
                aiComponents.GetNavMeshAgentController().GetAgent().updateRotation = true;
            }
            else
            {
                aiComponents.GetNavMeshAgentController().GetAgent().updateRotation = false;
                RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);
            }

            // if (!IsInChaseRangeTarget())
            // {
            //     enemyStateBlocks.SwitchToIdle();
            //     return;
            // }
        }
    }
}