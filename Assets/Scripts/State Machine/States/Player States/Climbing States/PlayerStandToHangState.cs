using UnityEngine;

namespace Etheral
{
    public class PlayerStandToHangState : PlayerBaseClimbingState
    {
        const float startNormalizedTime = .40f;
        const float targetNormalizedTime = .60f;
        const AvatarTarget avatarTarget = AvatarTarget.RightHand;

        // Vector3 handOffset = new(0, 0.2f, 0);
        Vector3 handOffset = new(0, 0, 0);
        ClimbPoint climbPoint;
        ClimbData climbData;
        RaycastHit climbHit;

        public PlayerStandToHangState(PlayerStateMachine stateMachine, RaycastHit climbHit, ClimbPoint climbPoint) :
            base(stateMachine)
        {
            this.climbHit = climbHit;
            this.climbPoint = climbPoint;

            Debug.Log($"ClimbPoint is {climbPoint.transform.name}");
            
        }

        public override void Enter()
        {
            
            climbData = stateMachine.PlayerComponents.GetClimbController().BindClimbingData("DropToHang",
                climbPoint.transform.position,
                avatarTarget, handOffset, startNormalizedTime, targetNormalizedTime);

            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(climbPoint.transform, climbData.matchBodyPart, climbData.limbOffset);

            // stateMachine.ForceReceiver.ToggleGravity(false);
          stateMachine.GetCharComponents().GetCC().enabled = false;
            stateMachine.Animator.applyRootMotion = true;
            stateMachine.Animator.CrossFadeInFixedTime("DropToHang", 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            RotateTowardsLedge(climbPoint.transform);
            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "DropToHang");

            if (normalizedTime >= 1f)
            {
                stateMachine.SwitchState(new PlayerHangingState(stateMachine, climbPoint));
            }
        }

        public override void Exit()
        {
        }
    }
}