using UnityEngine;

namespace Etheral
{
    public class PlayerJumpToSwingState : PlayerBaseClimbingState
    {
        Neighbor neighbor;
        ClimbPoint targetClimbPoint;
        ClimbPoint currentPoint;
        ClimbData climbData;
        Vector3 offset = new Vector3(0, 0, 0);

        public PlayerJumpToSwingState(PlayerStateMachine stateMachine, ClimbPoint receivedClimbPoint) : base(
            stateMachine)
        {
            currentPoint = receivedClimbPoint;

            //Get the neighbor of the current point
            neighbor = currentPoint.GetLeapOrSwingConnection();
            targetClimbPoint = neighbor.climbPoint;
        }

        public override void Enter()
        {
            climbData =
                stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.SwingFromGronud);

            // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("JumpToSwing",
            //     targetClimbPoint.transform.position,
            //     AvatarTarget.RightHand, offset, .69f, 1f);
            // climbData.limbPlacementAfterOffset =
            //     CalculateLimbOffset(targetClimbPoint.transform, climbData.matchBodyPart, climbData.limbOffset);

            stateMachine.Animator.applyRootMotion = true;
            stateMachine.GetCharComponents().GetCC().enabled = false;

            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(targetClimbPoint.transform, climbData.matchBodyPart, climbData.limbOffset);
            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            RotateTowardsGroundClimbPoint(currentPoint.transform);
            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                stateMachine.SwitchState(new PlayerSwingToGroundState(stateMachine, targetClimbPoint));
            }
        }

        public override void Exit()
        {
        }
    }
}