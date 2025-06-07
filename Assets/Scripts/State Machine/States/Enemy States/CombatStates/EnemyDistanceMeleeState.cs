using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public class EnemyDistanceMeleeState : EnemyBaseState
    {
        bool canRotate = true;
        bool hasAttacked;

        public EnemyDistanceMeleeState(EnemyStateMachine _stateMachine, int index = 0) : base(_stateMachine)
        {
            characterAction = enemyStateMachine.AIAttributes.MeleeRangedActions[index];
        }

        public override void Enter()
        {
            characterAction = enemyStateMachine.AIAttributes.MeleeRangedActions[0];

            if (enemyStateMachine.stateIndicator != null)
                enemyStateMachine.stateIndicator.color = Color.red;

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();

            if (characterAction != null)
            {

                var animationName = !characterAction.PreAnimation.IsNullOrWhitespace()
                    ? characterAction.PreAnimation
                    : characterAction.AnimationName;
                
                animationHandler.CrossFadeInFixedTime(animationName);
            }

            if (enemyStateMachine.UsesToken)
                stateMachine.TrackAttacksForToken();

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);

            StartCooldown(characterAction);
            
            AttackEffects();
            PlayEmote(enemyStateMachine.CharacterAudio.AudioLibrary.AttackEmote, AudioType.attackEmote);
        }

        public override void Tick(float deltaTime)
        {
            if (canRotate)
                RotateTowardsTargetSmooth(4);

            Move(deltaTime);

            if (!characterAction.PreAnimation.IsNullOrWhitespace())
            {
                float chargeNormalizedTime =
                    animationHandler.GetNormalizedTime(characterAction.PreAnimation);
            
                if (chargeNormalizedTime >= 1 && !hasAttacked)
                {
                    animationHandler.CrossFadeInFixedTime(characterAction);
                    hasAttacked = true;
                }
            
                if (characterAction.TimesBeforeForce.Length == 1 &&
                    chargeNormalizedTime >= characterAction.TimesBeforeForce[0])
                {
                    canRotate = false;
                }
            }

            float attackNormalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);


            actionProcessor.ApplyForceTimes(attackNormalizedTime);
            actionProcessor.RightWeaponTimes(attackNormalizedTime);

            //START HERE - NEED LOGIC WHEN ATTACKS ARE FINISHED TO MOVE TO NEXT STATE


            if (attackNormalizedTime >= 1)
            {
                enemyStateMachine.CheckIfShouldReturnToken();
                enemyStateBlocks.CheckLocomotionStates();

                // stateMachine.SwitchState(new EnemyIdleState(stateMachine, characterAction.MaxCooldown));
            }
        }

        public override void Exit() { }
    }
}