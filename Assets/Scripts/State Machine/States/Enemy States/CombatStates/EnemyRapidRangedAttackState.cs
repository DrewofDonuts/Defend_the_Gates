using UnityEngine;

namespace Etheral
{
    public class EnemyRapidRangedAttackState : EnemyBaseState
    {
        bool hasPlayedEffects;
        public EnemyRapidRangedAttackState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.AIAttributes.RangedAttackObjects[0].CharacterAction;

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);

            enemyStateMachine.GetAIComponents().navMeshAgentController.SetIsStopped(true);
            animationHandler.CrossFadeInFixedTime(characterAction.AnimationName, 0.1f);
            PassRangedDamage();
            AttackEffects();

            Debug.Log("Rapid Ranged Attack State");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(30f);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);
            actionProcessor.FireProjectiles(normalizedTime, 0);

            if (normalizedTime >= 1f)
            {
                enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit()
        {
            stateMachine.GetAIComponents().navMeshAgentController.SetIsStopped(false);
            stateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }

        void PassRangedDamage()
        {
            if (enemyStateMachine.WeaponInventory.RangedEquippedWeapon == null) return;
            enemyStateMachine.WeaponHandler.RangedWeaponDamage.SetAttackStatDamage(characterAction.Damage,
                characterAction.KnockBackForce,
                characterAction.KnockDownForce);
        }
    }
}