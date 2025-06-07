using System.Collections;
using UnityEngine;

namespace Etheral
{
    public abstract class PlayerBaseClimbingState : PlayerBaseState
    {
        public PlayerBaseClimbingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            stateMachine.StateType = StateType.Climbing;
        }

        protected Vector3 CalculateLimbOffset(Transform climbPointData, AvatarTarget hand, Vector3 offset = default)

        {
            return stateMachine.PlayerComponents.GetClimbController().GetHandPosition(climbPointData, hand, offset);
        }

        protected void PerformClimbingTargetMatching(ClimbData climbData)
        {
            if (stateMachine.Animator.isMatchingTarget)
                return;

            //MatchPosition will use an offset to match the hand to the ledge. Using ledge.forward
            //will only work for ledges that are facing the player

            stateMachine.Animator.MatchTarget(climbData.limbPlacementAfterOffset,
                stateMachine.transform.rotation, climbData.matchBodyPart,
                new MatchTargetWeightMask(Vector3.one, 0), climbData.matchStartTime, climbData.matchTargetTime, false);
        }

        public void RotateTowardsLedge(Transform climbHitTransform)
        {
            stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation,
                Quaternion.LookRotation(-climbHitTransform.forward),
                stateMachine.PlayerCharacterAttributes.RotateSpeed * Time.deltaTime);
        }

        protected void RotateTowardsGroundClimbPoint(Transform climbPointTransform)

        {
            stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation,
                climbPointTransform.rotation,
                stateMachine.PlayerCharacterAttributes.RotateSpeed * Time.deltaTime);
        }

        protected void PlayEffortAudio()
        {
            stateMachine.StartCoroutine(StartAudio());
        }

        IEnumerator StartAudio()
        {
            yield return new WaitForSeconds(0.2f);
            stateMachine.CharacterAudio.PlayRandomOneShot(stateMachine.CharacterAudio.EmoteSource,
                stateMachine.CharacterAudio.AudioLibrary.EffortEmote, AudioType.none);
        }
    }
}