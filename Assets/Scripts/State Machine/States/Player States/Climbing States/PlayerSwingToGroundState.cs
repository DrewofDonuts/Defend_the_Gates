using UnityEngine;

namespace Etheral
{
    public class PlayerSwingToGroundState : PlayerBaseClimbingState
    {
        Neighbor neighbor;
        ClimbPoint targetClimbPoint;
        ClimbPoint currentPoint;
        ClimbData climbData;
        Vector3 offset = new Vector3(0, .6f, 0);
        bool hasLanded;

        public PlayerSwingToGroundState(PlayerStateMachine stateMachine, ClimbPoint receivedClimbPoint) : base(
            stateMachine)
        {
            currentPoint = receivedClimbPoint;

            //Get the neighbor of the current point
            neighbor = currentPoint.GetLeapOrSwingConnection();
            targetClimbPoint = neighbor.climbPoint;
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.ToggleGravity(false);

            climbData =
                stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.SwingToGround);

            // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("SwingToGround",
            //     targetClimbPoint.transform.position,
            //     AvatarTarget.LeftFoot, offset, .30f, .54f);

            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(targetClimbPoint.transform, climbData.matchBodyPart, climbData.limbOffset);

            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);

            // var normalizedTimeLanding = GetNormalizedTime(stateMachine.Animator, "IdleFreeLook");

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                ReturnToLocomotion();
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.ForceReceiver.ToggleGravity(true);
          stateMachine.GetCharComponents().GetCC().enabled = true;
            stateMachine.Animator.applyRootMotion = false;
        }
    }
}