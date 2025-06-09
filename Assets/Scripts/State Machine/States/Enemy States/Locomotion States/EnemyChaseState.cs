using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class EnemyChaseState : EnemyBaseLocomotionState
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
            stateMachine.OnChangeStateMethod(StateType.Chase);

            animationHandler.CrossFadeInFixedTime(Locomotion);
            if (enemyStateMachine.AIAttributes.CanBlock && enemyStateMachine.Health.CurrentDefense > 0)
                enemyStateMachine.Health.SetBlocking(true);

            if (enemyStateMachine.stateIndicator != null)
                enemyStateMachine.stateIndicator.color = Color.blue;

            
            if (enemyStateBlocks == null)
                enemyStateBlocks = new EnemyStateBlocks(enemyStateMachine, this);

            EventBusPlayerStatesToDeprecate.OnPlayerChangeState += OnPlayerChangeStateChanged;

            aiComponents.GetNavMeshAgentController().SetAgentSpeed(enemyStateMachine.AIAttributes.RunSpeed);
        }

        public void OnPlayerChangeStateChanged(StateType newstatetype)
        {
            // if (!enemyStateMachine.GetHostile()) return;
            // if (newstatetype == StateType.KnockedDown &&
            //     GetPlayerDistance() < stateMachine.AIAttributes.RangedAttackRange &&
            //     stateMachine.AIAttributes.HasGroundAttack)
            // {
            //     enemyStateMachine.StartCoroutine(WaitBeforeGroundAttack());
            // }
            // else if (newstatetype != StateType.KnockedDown)
            // {
            //     enemyStateMachine.StopCoroutine(WaitBeforeGroundAttack());
            // }
        }

        //Disabling Ground Attacks
        IEnumerator WaitBeforeGroundAttack()
        {
            yield return waitBeforeGroundAttack;
            enemyStateMachine.SwitchState(new EnemyGroundAttackState(enemyStateMachine));
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);
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
                Debug.Log("If in Melee Range and no token");

                // Decelerate(deltaTime);
                if (!stateMachine.AIAttributes.CanStrafe && movementSpeed < 0.1f)
                {
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
            EventBusPlayerStatesToDeprecate.OnPlayerChangeState -= OnPlayerChangeStateChanged;

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            enemyStateMachine.GetAIComponents().navMeshAgentController.SetVelocityToKeepInSyncWithCC(
                Vector3.zero);

            enemyStateMachine.Health.SetBlocking(false);
        }
    }
}