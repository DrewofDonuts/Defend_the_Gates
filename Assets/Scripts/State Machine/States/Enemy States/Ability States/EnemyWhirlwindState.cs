using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class EnemyWhirlwindState : EnemyBaseState
    {
        // EnemyStateBlocks _enemyStateBlocks;
        CharacterAction _characterAction;
        
        public EnemyWhirlwindState(EnemyStateMachine stateMachine, bool isCounterAction) : base(stateMachine)
        {
            this.isCounterAction = isCounterAction;
        }

        public override void Enter()
        {
            if (isCounterAction)
                _characterAction = enemyStateMachine.AIAttributes.CounterCharacterAction;

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            RotateTowardsTargetSnap();

            enemyStateMachine.Health.SetSturdy(true);
            enemyStateMachine.SetActiveAbility(true);

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, _characterAction);
            animationHandler.CrossFadeInFixedTime(_characterAction.AnimationName,
                _characterAction.TransitionDuration);

            AttackEffects();
            PlayEmote(enemyStateMachine.CharacterAudio.AudioLibrary.AttackEmote, AudioType.attackEmote);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, _characterAction.AnimationName);

            if (normalizedTime >= 1)
            {
                RotateTowardsTargetSmooth(deltaTime);
                enemyStateBlocks.CheckLocomotionStates();
            }
            
            // actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.RightWeaponTimes(normalizedTime);



            if (normalizedTime >= _characterAction.TimesBeforeForce[0] && normalizedTime <=
                _characterAction.DisableRightWeapon[0])
                Move(GetCurrentTargetPosition(), _characterAction.Forces[0], deltaTime);

        }
        
        public override void Exit()
        {
            enemyStateMachine.Health.SetSturdy(false);
            enemyStateMachine.SetActiveAbility(false);
        }
        
    }
}