using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class EnemyAttackingState : EnemyBaseState
    {
        Image _attackIndicatorState;

        bool canRotate = true;
        bool playedAudio;
        protected int attackIndex;


        public EnemyAttackingState(EnemyStateMachine stateMachine, int _attackIndex = 0) : base(stateMachine)
        {
            attackIndex = _attackIndex;
        }

        public override void Enter()
        {
            characterAction = stateMachine.AIAttributes.BasicAttacks[attackIndex];
            stateMachine.StateType = StateType.Attack;

            if (characterAction == null)
            {
                characterAction = enemyStateMachine.AIAttributes.BasicAttacks[0];
                Debug.LogError("No character action found, defaulting to first attack");
            }

            if (enemyStateMachine.stateIndicator != null && enemyStateMachine.AITestingControl.displayStateIndicator)
                enemyStateMachine.stateIndicator.color = Color.red;
            

            stateMachine.TrackAttacksForToken();
            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            animationHandler.CrossFadeInFixedTime(characterAction);

            
            PlayEmote(enemyStateMachine.CharacterAudio.AudioLibrary?.AttackEmote, AudioType.attackEmote);

            AttackEffects();
        }

        public override void Tick(float deltaTime)
        {
            if (canRotate)
                RotateTowardsTargetSmooth(4);

            Move(deltaTime);

            float normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, characterAction.AnimationName);


            if (characterAction.TimesBeforeForce.Length == 1 && normalizedTime >= characterAction.TimesBeforeForce[0])
            {
                canRotate = false;
            }

            actionProcessor.ApplyForceTimes(normalizedTime, IsAdjacentRange());

            if (characterAction.EnableRightWeapon.Length > 0)
                actionProcessor.RightWeaponTimes(normalizedTime);

            if (characterAction.EnableLeftWeapon.Length > 0)
                actionProcessor.LeftWeaponTimes(normalizedTime);


            if (normalizedTime >= 1)
            {
                if (enemyStateMachine.CheckIfShouldRetreat())
                {
                    Debug.Log($"Token queue count: {TokenManager.Instance.tokenQueue.Count}");
                    enemyStateBlocks.SwitchToRetreat();

                    // enemyStateMachine.SwitchState(new EnemyRetreatState(enemyStateMachine));
                    return;
                }

                enemyStateBlocks.CheckLocomotionStates();
            }

            TryComboAttack(normalizedTime);
        }

        protected void TryComboAttack(float normalizedTime)
        {
            if (characterAction.NextComboStateIndex == -1) return;

            //if -1, then there is no combo

            if (normalizedTime < characterAction.ComboAttackTime) return;
            enemyStateBlocks.SwitchToMeleeAttack(characterAction.NextComboStateIndex);

            // if (aiStateOverrides.OverrideAttackState)
            //     aiStateOverrides.AttackOverrideState(enemyStateMachine, characterAction.NextComboStateIndex);
            // else
            //     enemyStateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine,
            //         characterAction.NextComboStateIndex));
        }

        public override void Exit()
        {
            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}

//     if (stateMachine.CharacterAttributes.BasicAttacks[attack].MaxCooldown > 0)
//         CooldownManager.instance.StartCooldown(stateMachine.CharacterAttributes.BasicAttacks[attack]);