using UnityEngine;

namespace Etheral
{
    public class PlayerJumpToLedgeFromGroundState : PlayerBaseClimbingState
    {
        Neighbor neighbor;
        ClimbPoint targetClimbPoint;
        ClimbPoint currentPoint;
        ClimbData climbData;

        Vector3 footOffset = new Vector3(0, .2f, 0);
        bool hasLanded;


        public PlayerJumpToLedgeFromGroundState(PlayerStateMachine stateMachine, ClimbPoint receivedClimbPoint) : base(
            stateMachine)
        {
            currentPoint = receivedClimbPoint;
            neighbor = currentPoint.GetLeapOrSwingConnection();
            targetClimbPoint = neighbor.climbPoint;
        }

        public override void Enter()
        {
            //First, just get a leap to occur
            //Then change the leap animation depending on the distance to the next ledge
            //We can determine distance by distance from currentPoint to neighbor.climbPoint

            PlayEffortAudio();

            climbData = stateMachine.PlayerComponents.GetClimbController().BindClimbingData("LedgeLeap",
                targetClimbPoint.transform.position,
                AvatarTarget.RightFoot, footOffset, .17f, 1f);
            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(targetClimbPoint.transform, climbData.matchBodyPart, climbData.limbOffset);

            stateMachine.Animator.applyRootMotion = true;
            stateMachine.GetCharComponents().GetCC().enabled = false;

            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            RotateTowardsGroundClimbPoint(currentPoint.transform);

            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);

            var jumpLandingNormalizedTime = GetNormalizedTime(stateMachine.Animator, "JumpLanding");

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                if (!hasLanded)
                {
                    stateMachine.Animator.CrossFadeInFixedTime("JumpLanding", 0.2f);
                    hasLanded = true;
                }
            }

            if (!(jumpLandingNormalizedTime >= 1f)) return;
            ReturnToLocomotion();
            return;
        }


        public override void Exit()
        {
            stateMachine.Animator.applyRootMotion = false;
            stateMachine.GetCharComponents().GetCC().enabled = true;
        }
    }
}