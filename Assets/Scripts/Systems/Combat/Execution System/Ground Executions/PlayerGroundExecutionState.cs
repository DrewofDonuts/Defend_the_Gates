using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class PlayerGroundExecutionState : PlayerBaseState
    {
        GroundExecutionPointDetector groundExecutionPointDetector;
        HeadExecutionPoint headExecutionPoint;
        CharacterAction _characterAction;

        bool alreadyTriggeredExecution;

        public PlayerGroundExecutionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _characterAction = stateMachine.PlayerCharacterAttributes.GroundExecution;
            groundExecutionPointDetector = stateMachine.PlayerComponents.GroundExecutionPointDetector;
            headExecutionPoint = groundExecutionPointDetector.CurrentHeadExecutionPoint;

            stateMachine.Animator.CrossFadeInFixedTime("GroundExecution", 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            RotateTowardsExecutionPoint(stateMachine.PlayerComponents.GroundExecutionPointDetector.CurrentHeadExecutionPoint.transform.position);

            float normalizedValue = GetNormalizedTime(stateMachine.Animator, _characterAction.AnimationName);

            if (normalizedValue >= _characterAction.TimeBeforeEffect && !alreadyTriggeredExecution)
            {
                stateMachine.TriggerFeedbackFromAnimation(3);
                headExecutionPoint.BlowUpAndDie();
                alreadyTriggeredExecution = true;
            }

            if (normalizedValue >= 1)
                ReturnToLocomotion();
            


            //rotate towards the Execution Point
            //Perform the execution
        }

        public override void Exit()
        {
        }
    }
}