using System;
using UnityEngine;

namespace Etheral
{
    public class PlayerGroundToLedgeState : PlayerBaseClimbingState
    {
        RaycastHit climbHit;
        ClimbPoint currentClimbPoint;
        ClimbData climbData;

        public PlayerGroundToLedgeState(PlayerStateMachine stateMachine, RaycastHit climbHit,
            ClimbPoint climbPoint = default) : base(stateMachine)
        {
            this.climbHit = climbHit;
            currentClimbPoint = climbPoint;
        }

        public override void Enter()
        {
            climbData =
                stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.GroundToLedge);

            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(climbHit.transform, climbData.matchBodyPart, climbData.limbOffset);

            stateMachine.GetCharComponents().GetCC().enabled = false;
            stateMachine.Animator.applyRootMotion = true;
            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);

        }


        public override void Tick(float deltaTime)
        {
            RotateTowardsLedge(climbHit.transform);
            PerformClimbingTargetMatching(climbData);


            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                stateMachine.SwitchState(new PlayerHangingState(stateMachine, currentClimbPoint));
                return;
            }
        }
        
        public override void Exit()
        {
        }
    }
}