using UnityEngine;

namespace Etheral
{
    public class PlayerParkourState : PlayerBaseState
    {
        // targetRotation;
        readonly ObstacleHitData hitData;
        static readonly int IsMirrorAction = Animator.StringToHash("isMirrorAction");

        public PlayerParkourState(PlayerStateMachine stateMachine, ClimbAndParkourData climbAndParkourData) : base(
            stateMachine)
        {
            climbAndParkour = climbAndParkourData;
        }

        public override void Enter()
        {
            //must disable character controller to allow for animation to move character
          stateMachine.GetCharComponents().GetCC().enabled = false;
            stateMachine.Animator.applyRootMotion = true;

            stateMachine.Animator.SetBool(IsMirrorAction, climbAndParkour.mirrorAnimation);
            stateMachine.Animator.CrossFadeInFixedTime(climbAndParkour.animName, 0.2f);


            matchBodyPart = climbAndParkour.mirrorAnimation
                ? climbAndParkour.overrideMatchBodyPart
                : climbAndParkour.matchBodyPart;
        }


        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, climbAndParkour.animName);
            PerformTargetMatchingIfEnabled();
            RotateTowardObstacleIfEnabled(deltaTime);


            if (normalizedTime >= 1f + climbAndParkour.PauseTime && !stateMachine.Animator.isMatchingTarget)
            {
                ReturnToLocomotion();
            }
            
        }

        void RotateTowardObstacleIfEnabled(float deltaTime)
        {
            if (climbAndParkour.rotateTowardsObstacle)
                stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation,
                    climbAndParkour.targetRotation,
                    stateMachine.PlayerCharacterAttributes.RotateSpeed * deltaTime);
        }

        void PerformTargetMatchingIfEnabled()
        {
            if (stateMachine.Animator.isMatchingTarget)
                return;

            if (!climbAndParkour.enableTargetMatching)
                return;

            //have to divide times by 100 to normalize them from percentage to a float value

            //only matching in the Y axis, so the character doesn't move in the X and Z axis
            stateMachine.Animator.MatchTarget(climbAndParkour.matchPosition, stateMachine.transform.rotation,
                matchBodyPart, new MatchTargetWeightMask(climbAndParkour.matchPosWeight, 0),
                climbAndParkour.matchStartTime / 100, climbAndParkour.matchTargetTime / 100);
        }

        public override void Exit()
        {
            stateMachine.Animator.applyRootMotion = false;
          stateMachine.GetCharComponents().GetCC().enabled = true;
        }
    }
}