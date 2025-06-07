using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class PlayerLedgeJumpingState : PlayerBaseClimbingState
    {
        Neighbor neighbor;
        ClimbPoint neighborsClimbPoint;
        ClimbData climbData;
        Vector3 handOffset;
        bool playAudio;

        public PlayerLedgeJumpingState(PlayerStateMachine stateMachine, Neighbor neighbor) : base(stateMachine)
        {
            this.neighbor = neighbor;
            neighborsClimbPoint = neighbor.climbPoint;
        }

        public override void Enter()
        {
            // Debug.Log("Ledge Jumping");
            //

            var randomNumber = Random.Range(0, 3);
            if (randomNumber == 1)
                stateMachine.StartCoroutine(StartAudio());

            if (neighbor.climbDirection == ClimbDirection.Up)
            {
                climbData = stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.Up);

                // handOffset = new Vector3(0.25f, 0.08f, 0.15f);
                // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("HopUp",
                //     neighborsClimbPoint.transform.position,
                //     AvatarTarget.RightHand, handOffset, 0.35f, 0.65f);
            }
            else if (neighbor.climbDirection == ClimbDirection.Down)
            {
                climbData = stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.Down);

                // handOffset = new Vector3(0.25f, 0.1f, 0.13f);
                // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("HopDown",
                //     neighborsClimbPoint.transform.position,
                //     AvatarTarget.RightHand, handOffset, 0.31f, 0.65f);
            }
            else if (neighbor.climbDirection == ClimbDirection.Right)
            {
                climbData = stateMachine.PlayerComponents.GetClimbController()
                    .GetClimbingAction(ClimbActionType.Right);

                // handOffset = new Vector3(0.20f, 0.08f, 0.15f);
                // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("HopRight",
                //     neighborsClimbPoint.transform.position,
                //     AvatarTarget.RightHand, handOffset, 0.20f, 0.50f);
            }
            else if (neighbor.climbDirection == ClimbDirection.Left)
            {
                climbData = stateMachine.PlayerComponents.GetClimbController().GetClimbingAction(ClimbActionType.Left);

                // handOffset = new Vector3(0.20f, 0.08f, 0.15f);
                // climbData = stateMachine.PlayerComponentHandler.GetClimbController().BindClimbingData("HopLeft",
                //     neighborsClimbPoint.transform.position,
                //     AvatarTarget.RightHand, handOffset, 0.20f, 0.50f);
            }

            climbData.limbPlacementAfterOffset =
                CalculateLimbOffset(neighborsClimbPoint.transform, climbData.matchBodyPart, handOffset);

            stateMachine.Animator.CrossFadeInFixedTime(climbData.animName, 0.2f);
        }


        IEnumerator StartAudio()
        {
            yield return new WaitForSeconds(0.2f);
            stateMachine.CharacterAudio.PlayRandomOneShot(stateMachine.CharacterAudio.EmoteSource,
                stateMachine.CharacterAudio.AudioLibrary.EffortEmote, AudioType.none);
        }

        //TODO - FULLY IMPLEMENT THIS FUNCTION
        void PlayAudioWhenHandReachesLedge()
        {
            if (Vector3.Distance(stateMachine.WeaponInventory.RightHand.transform.position,
                    neighborsClimbPoint.transform.position) < 0.2f && !playAudio)
            {
                stateMachine.CharacterAudio.PlayRandomOneShot(stateMachine.CharacterAudio.LocomotionSource,
                    stateMachine.CharacterAudio.AudioLibrary.CarpetSurfaceWalking, AudioType.none);
                playAudio = true;
            }
        }

        public override void Tick(float deltaTime)
        {
            PerformClimbingTargetMatching(climbData);

            var normalizedTime = GetNormalizedTime(stateMachine.Animator, climbData.animName);


            if (normalizedTime >= climbData.matchStartTime)
                RotateTowardsLedgeWhenMatchtime();

            if (normalizedTime >= 1f && !stateMachine.Animator.isMatchingTarget)
            {
                stateMachine.SwitchState(new PlayerHangingState(stateMachine, neighborsClimbPoint));
                return;
            }
        }


        void RotateTowardsLedgeWhenMatchtime()
        {
            if (neighbor.isDifferentPlane)
            {
                // Rotate towards the ledge when normalized time reaches matchTimeRotation
                stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation,
                    Quaternion.LookRotation(-neighborsClimbPoint.transform.forward),
                    stateMachine.PlayerCharacterAttributes.RotateSpeed * Time.deltaTime);
            }
        }

        public override void Exit()
        {
        }
    }
}