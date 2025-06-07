using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class SimplePlayerInjuredMoveState : SimplePlayerBaseState
    {
        float slowSpeed = .50f;
        float maxSpeed = 1f;
        float lastFootstepTime;
        float lastFootDragTime;
        InjuredAudio injuredAudio;

        public SimplePlayerInjuredMoveState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            stateMachine.StartCoroutine(AlternateSpeed());

            animationHandler.CrossFadeInFixedTime("WalkInjuredSlow");
            injuredWalk = true;

            injuredAudio = stateMachine.GetComponent<InjuredAudio>();
            
            RegisterEvents();
            
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime("WalkInjuredSlow");

            HandleFootsteps(normalizedTime);

            HandleAllLocomotionAndAnimation(deltaTime);
            if (playerComponents.GetInput().MovementValue.magnitude <= 0.15f)
            {
                stateMachine.SwitchState(new SimplePlayerInjuredMoveState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            DeRegisterEvents();
            stateMachine.StopCoroutine(AlternateSpeed());
        }


        IEnumerator AlternateSpeed()
        {
            while (stateMachine.gameObject.activeSelf)
            {
                customSpeed = slowSpeed;
                yield return new WaitForSeconds(.5f);
                customSpeed = maxSpeed;
                yield return new WaitForSeconds(.5f);
            }
        }

        void HandleFootsteps(float normalizedTime)
        {
            // Define the points in the animation cycle where footstep sounds should be played

            normalizedTime %= 1f;

            float footstepTime = .9f;
            float footDragTime = .2f;

            if (normalizedTime >= footstepTime && lastFootstepTime < footstepTime)
            {
                PlayFootstepSound();
                lastFootstepTime = normalizedTime;
            }

            if (normalizedTime >= footDragTime && lastFootDragTime < footDragTime)
            {
                PlayFootDragSound();
                lastFootDragTime = normalizedTime;
            }


            // Reset lastFootstepTime and lastFootDragTime when the animation cycle completes
            if (normalizedTime < lastFootstepTime || normalizedTime < lastFootDragTime)
            {
                lastFootstepTime = 0f;
                lastFootDragTime = 0f;
            }
        }

        void PlayFootstepSound()
        {
            // Implement the logic to play the footstep sound
            playerComponents.GetCharacterAudio().PlayRandomOneShot(
                playerComponents.GetCharacterAudio().LocomotionSource,
                injuredAudio.footSteps, AudioType.none);
        }

        void PlayFootDragSound()
        {
            // Implement the logic to play the foot drag sound
            playerComponents.GetCharacterAudio().PlayOneShot(playerComponents.GetCharacterAudio().LocomotionSource,
                injuredAudio.footDrag, AudioType.none, .85f, 1.15f, .90f, 1f);
        }
    }
}