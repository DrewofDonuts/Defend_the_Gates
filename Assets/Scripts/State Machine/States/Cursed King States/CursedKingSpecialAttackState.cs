using UnityEngine;

namespace Etheral.Cursed_King
{
    public class CursedKingSpecialAttackState : EnemyBaseState
    {
        int attackIndex;
        bool hasPlayedEffect;

        public CursedKingSpecialAttackState(EnemyStateMachine _stateMachine, int attackIndex) : base(_stateMachine)
        {
            this.attackIndex = attackIndex;
        }

        public override void Enter()
        {
            characterAction = stateMachine.AIAttributes.SpecialAbility[attackIndex];

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            
            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);

            StartCooldown(characterAction);

            AttackEffects();
            PlayEmote(characterAction, AudioType.attackEmote);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(4f);

            float normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            actionProcessor.ApplyForceTimes(normalizedTime, IsAdjacentRange());

            if (normalizedTime >= characterAction.TimeBeforeEffect && !hasPlayedEffect)
            {
                EventBusPlayerController.FeedbackBasedOnDistanceFromPlayer(stateMachine.gameObject,
                    stateMachine.transform.position, characterAction.FeedbackType);
                hasPlayedEffect = true;
            }

            if (characterAction.EnableRightWeapon.Length > 0)
                actionProcessor.RightWeaponTimes(normalizedTime);

            if (characterAction.EnableLeftWeapon.Length > 0)
                actionProcessor.LeftWeaponTimes(normalizedTime);

            if (characterAction.HasSpell)
                actionProcessor.CastSpells(normalizedTime);


            if (characterAction.NextComboStateIndex > 0 && normalizedTime >= characterAction.ComboAttackTime)
            {
                enemyStateBlocks.SwitchToSpecialAttack(attackIndex + 1);
                return;
            }

            if (normalizedTime >= 1f)
            {
                enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit()
        {

            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}