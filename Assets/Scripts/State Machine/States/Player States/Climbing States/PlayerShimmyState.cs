using UnityEngine;

namespace Etheral
{
    public class PlayerShimmyState : PlayerBaseClimbingState
    {
        Neighbor neighbor;
        ClimbPoint pointToMoveTo;
        ClimbData climbData;

        public PlayerShimmyState(PlayerStateMachine stateMachine, Neighbor neighbor) : base(stateMachine)
        {
            this.neighbor = neighbor;
            pointToMoveTo = this.neighbor.climbPoint;
        }

        public override void Enter()
        {
            if (neighbor.climbDirection == ClimbDirection.Right)
            {
                climbData =
                    stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.ShimmyRight);
        
            }
            else if (neighbor.climbDirection == ClimbDirection.Left)
            {
                climbData =
                stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.ShimmyLeft);
            }
            
            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(pointToMoveTo.transform, climbData.matchBodyPart, climbData.limbOffset);
            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                stateMachine.SwitchState(new PlayerHangingState(stateMachine, pointToMoveTo));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}