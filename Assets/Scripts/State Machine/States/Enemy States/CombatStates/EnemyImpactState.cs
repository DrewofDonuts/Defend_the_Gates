using UnityEngine;

namespace Etheral
{
    public class EnemyImpactState : EnemyBaseState
    {
        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Impact);
            RotateTowardsTargetSnap();

            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();
        }

        public override void Tick(float deltaTime)
        {
            //Move is called to make sure gravity is active
            Move(deltaTime);

            float normalizedvalue = GetNormalizedTime(enemyStateMachine.Animator, "Impact");

            if (normalizedvalue >= 1)
            {
                HandleStateFromImpact();
            }
        }

        void HandleStateFromImpact()
        {
            // stateMachine.CharacterAttributes.CounterAttackSelector.CounterStateSelector(stateMachine);

            if (CheckPriorityAndTokenBeforeActions(true))
            {
                if (CheckIfCanCounterAction())
                {
                    enemyStateMachine.AIAttributes.CounterAttackSelector.CounterStateSelector(enemyStateMachine);
                    return;
                }


                if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                    return;
            }


            enemyStateBlocks.CheckLocomotionFromImpactState();
        }

        bool CheckIfCanCounterAction()
        {
            return enemyStateMachine.AIAttributes.CanCounterAction && enemyStateMachine.CheckIfCounterActionIsReady() &&
                   !enemyStateMachine.AITestingControl.blockCounterAttack &&
                   !enemyStateMachine.AITestingControl.idleAndImpactOnly &&
                   !enemyStateMachine.AITestingControl.blockAttack;
        }

        public override void Exit() { }
    }
}