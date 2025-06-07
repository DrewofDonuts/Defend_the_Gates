using UnityEngine;

namespace Etheral
{
    public class EnemyKnockDownState : EnemyBaseState
    {
        float duration = 4f;
        bool isGetUp;

        readonly int KnockDown = Animator.StringToHash("KnockDown");
        readonly int GetUp = Animator.StringToHash("GetUp");

        public EnemyKnockDownState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(KnockDown, CrossFadeDuration);

            var healthPercentage =
                (enemyStateMachine.Health.CurrentHealth / enemyStateMachine.Health.CharacterAttributes.MaxHealth) *
                100f;


            //For demonstration - change from 45 to 100
            if (healthPercentage <= 100)
            {
                enemyStateMachine.GetAIComponents().GetHeadExecutionPoint().SetCanHit(true);

                // stateMachine.HighlightEffectController.SetHighlightEffect(
                //     stateMachine.HighlightEffectController.MultiuseHighlightEffect,
                //     stateMachine.CharacterAttributes.HighlightProfile.ExecutionPointProfile,
                //     stateMachine.ExecutionPoint.transform);
                //
                // stateMachine.HighlightEffectController.ToggleHighlightEffect(
                //     stateMachine.HighlightEffectController.MultiuseHighlightEffect,
                //     true);
            }
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            duration -= deltaTime;

            if (duration <= 0 && !isGetUp)
            {
                enemyStateMachine.Animator.CrossFadeInFixedTime(GetUp, CrossFadeDuration);

                // stateMachine.HighlightEffectController.ToggleHighlightEffect(stateMachine.HighlightEffectController.MultiuseHighlightEffect, false);

                enemyStateMachine.GetAIComponents().GetHeadExecutionPoint().SetCanHit(false);

                isGetUp = true;

                //
                // if (_stateMachine.ExecutionPoint.enabled)
                //     _stateMachine.ExecutionPoint.enabled = false;
            }

            if (GetNormalizedTime(enemyStateMachine.Animator, "GetUp") >= 1)
            {
                if (IsInMeleeRange() && enemyStateMachine.AIAttributes.CanStrafe)
                    enemyStateBlocks.SwitchToStrafe();
                else if (IsInMeleeRange())
                    enemyStateBlocks.SwitchToMeleeAttack(0);
                else if (IsInChaseRangeTarget())
                    enemyStateBlocks.SwitchToChase();
                else if (!IsInChaseRangeTarget())
                    enemyStateBlocks.SwitchToBaseState();

                // if (IsInMeleeRange())
                //     stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
                // else if (IsInChaseRange())
                //     stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                // else if (!IsInChaseRange())
                //     stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }

        public override void Exit()
        {
            // stateMachine.HighlightEffectController.ToggleHighlightEffect(
            // stateMachine.HighlightEffectController.MultiuseHighlightEffect, false);

            enemyStateMachine.GetAIComponents().GetHeadExecutionPoint().SetCanHit(false);
        }
    }
}