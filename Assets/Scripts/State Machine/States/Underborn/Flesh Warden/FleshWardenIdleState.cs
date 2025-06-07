using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public class FleshWardenIdleState : EnemyIdleState
    {
        public FleshWardenIdleState(EnemyStateMachine stateMachine, string _animationOverride = "") : base(stateMachine,
            _animationOverride) { }
        
        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            timerBeforePatrol += deltaTime;

            HandlePatrolLogic(deltaTime);

            checkNextActionTimer += deltaTime;

            if (IsInChaseRangeTarget() && enemyStateMachine.GetHostile())
                RotateTowardsTargetSmooth(60f);


            if (!enemyStateMachine.GetHostile()) return;

            if (checkNextActionTimer > checkNextActionInterval)
            {
                if (IsInChaseRangeTarget() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                {

                    if (stateMachine.AIAttributes.SpecialAbility[0].CheckIfReady() && IsInRangedRange() )
                    {
                        stateMachine.SwitchState(new FleshWardenHookState(enemyStateMachine));
                        return;
                    }

                    if (CheckPriorityAndTokenBeforeActions())
                    {
                        if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                            return;

                        if (enemyStateBlocks.CanStrafe())
                        {
                            enemyStateBlocks.SwitchToStrafe();
                            return;
                        }

                        if (!IsInMeleeRange())
                            enemyStateBlocks.SwitchToChase();
                    }
                }
                else
                {
                    //do this if the enemy is not in chase range
                    enemyStateMachine.RemoveTokenAndQueue();
                }

                checkNextActionTimer = 0;
            }
        }


        public override void Exit()
        {
            checkNextActionTimer = 0;
            EventBusPlayerStatesToDeprecate.OnPlayerChangeState -= OnPlayerChangeStateChanged;
            enemyStateMachine.StopAllCoroutines();
        }
    }
}