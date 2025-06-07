using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    //Need to change implementation to use NavMesh in determining the direction and landing position
    //Add force is problematic as it may result in landing in unwanted positions
    public class EnemyJumpBackState : EnemyBaseState
    {
        // EnemyStateBlocks _enemyStateBlocks;
        // CharacterAction characterAction;
        bool alreadyAppliedForce;
        const float RETREAT_DISTANCE = 4f;
        bool finishedjumping;


        public EnemyJumpBackState(EnemyStateMachine stateMachine, bool isCounterAction) : base(stateMachine)
        {
            this.isCounterAction = isCounterAction;
        }

        public override void Enter()
        {
            characterAction = stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "JumpBack");


            // _enemyStateBlocks = new EnemyStateBlocks(stateMachine);
            alreadyAppliedForce = false;

            // enemyStateMachine.GetAIComponents().navMeshAgentController.DisableAgentUpdate();

            animationHandler.CrossFadeInFixedTime("DefenseCounter");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);

            float normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, characterAction.AnimationName);
            float flexNormalizedTime = GetNormalizedTime(enemyStateMachine.Animator, "Flex2");

            if (normalizedTime >= 1 && !finishedjumping)
            {
                animationHandler.CrossFadeInFixedTime("Flex2");
                finishedjumping = true;
            }

            if (flexNormalizedTime >= 1)
            {
                enemyStateBlocks.CheckLocomotionStates();
                return;
            }


            var directionFromPlayer = GetDirectionAwayFromPlayer();


            var retreatTarget = stateMachine.transform.position +
                                directionFromPlayer.normalized * RETREAT_DISTANCE;

            if (normalizedTime >= characterAction.TimesBeforeForce[0] &&
                normalizedTime < characterAction.TimesBeforeForce[1])
            {
                if (CheckNavMeshSamplePosition(retreatTarget, RETREAT_DISTANCE, out NavMeshHit hit))
                {
                    Move(hit.position, characterAction.Forces[0], deltaTime);
                }
            }


            // if (normalizedTime >= _characterAction.TimesBeforeForce[0])
            //     TryApplyForce(_characterAction.Force);
        }

        protected void TryApplyForce(float force)
        {
            if (!alreadyAppliedForce)
                enemyStateMachine.ForceReceiver.AddForce(enemyStateMachine.transform.forward * force);

            alreadyAppliedForce = true;
        }

        public override void Exit()
        {
            stateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}