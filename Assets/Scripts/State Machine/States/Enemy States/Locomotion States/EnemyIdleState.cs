using System.Collections;
using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public class EnemyIdleState : EnemyBaseState
    {
        // static readonly int LocomotionState = Animator.StringToHash("Locomotion");
        // static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");

        WaitForSeconds waitBeforeGroundAttack = new(1f);
        protected string animationOverride;

        protected bool isGroundAttack;
        protected float timeBeforeGroundAttack = 1f;

        public EnemyIdleState(EnemyStateMachine stateMachine, string _animationOverride = "") : base(stateMachine)
        {
            animationOverride = _animationOverride;
        }

        public override void Enter()
        {
            // if (CheckForTokenBeforeAction())
            //     if (enemyStateBlocks.CheckAttacksFromLocomotionState())
            //         return;

            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();


            stateMachine.OnChangeStateMethod(StateType.Idle);

            if (!animationOverride.IsNullOrWhitespace())
            {
                animationHandler.CrossFadeInFixedTime(animationOverride);
            }
            else
                animationHandler.CrossFadeInFixedTime(IdleState);

            if (enemyStateMachine.stateIndicator != null && enemyStateMachine.AITestingControl.displayStateIndicator)
                enemyStateMachine.stateIndicator.color = Color.green;

            EventBusPlayerStatesToDeprecate.OnPlayerChangeState += OnPlayerChangeStateChanged;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);


            if (!IsInChaseRangeTarget())
                HandlePatrolLogic(deltaTime);

            if (IsInChaseRangeTarget() && enemyStateMachine.GetHostile())
                RotateTowardsTargetSmooth(60f);

            if (isGroundAttack)
                timeBeforeGroundAttack -= deltaTime;

            // if (timeBeforeGroundAttack <= 0)
            // {
            //     enemyStateBlocks.SwitchToGroundAttack();
            //     return;
            // }

            if (!enemyStateMachine.GetHostile()) return;

            CheckCombatWIthTimer(deltaTime);
        }


        public override void Exit()
        {
            stateMachine.OnChangeStateMethod(StateType.Default);

            checkNextActionTimer = 0;
            EventBusPlayerStatesToDeprecate.OnPlayerChangeState -= OnPlayerChangeStateChanged;

            // enemyStateMachine.StopCoroutine(WaitBeforeGroundAttack());
        }

        public void OnPlayerChangeStateChanged(StateType newstatetype)
        {
            // if (newstatetype == StateType.KnockedDown &&
            //     GetPlayerDistance() < enemyStateMachine.AIAttributes.RangedAttackRange &&
            //     enemyStateMachine.AIAttributes.HasGroundAttack)
            // {
            //     enemyStateMachine.StartCoroutine(WaitBeforeGroundAttack());
            // }
            // else if (newstatetype != StateType.KnockedDown)
            // {
            //     enemyStateMachine.StopCoroutine(WaitBeforeGroundAttack());
            // }

            // isGroundAttack = true;
        }

        IEnumerator WaitBeforeGroundAttack()
        {
            yield return waitBeforeGroundAttack;

            enemyStateBlocks.SwitchToGroundAttack();
        }
    }
}