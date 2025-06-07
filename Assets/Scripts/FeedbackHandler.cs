using System;
using MoreMountains.Feedbacks;
using UnityEngine;


namespace Etheral
{
    public class FeedbackHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float distanceToPlayFeedback = 3f;
        [SerializeField] int id;

        [Header("Haptic Feedback References")]
        [SerializeField] MMF_Player lightFeedback;
        [SerializeField] MMF_Player mediumFeedback;
        [SerializeField] MMF_Player heavyFeedback;
        [SerializeField] MMF_Player constantRumble;

        [Header("Camera Shake References")]
        [SerializeField] MMF_Player lightCameraShake;
        [SerializeField] MMF_Player mediumCameraShake;
        [SerializeField] MMF_Player heavyCameraShake;
        


        void Start()
        {
            lightFeedback.Initialization();
            mediumFeedback.Initialization();
            heavyFeedback.Initialization();
            lightCameraShake.Initialization();
            mediumCameraShake.Initialization();
            heavyCameraShake.Initialization();

            if (constantRumble != null)
                constantRumble.Initialization();

            EventBusPlayerController.OnFeedbackDistance += PlayFeedbackDistanceBasedOnDistanceFromPlayer;
            EventBusPlayerController.OnFeedbackDistanceDamageData += ShouldPlayFeedbackBasedOnID;
            EventBusPlayerController.OnFeedback += HandleHapticInput;
            EventBusPlayerController.OnFeedback += HandleCameraShake;
        }

        void ShouldPlayFeedbackBasedOnID(Vector3 arg1, DamageData arg2)
        {
            if(arg2.AttackerID != id) return;
            
            PlayFeedbackDistanceBasedOnDistanceFromPlayer(arg1, arg2.FeedbackType);
        }

        void OnDisable()
        {
            EventBusPlayerController.OnFeedbackDistance -= PlayFeedbackDistanceBasedOnDistanceFromPlayer;
            EventBusPlayerController.OnFeedbackDistanceDamageData -= ShouldPlayFeedbackBasedOnID;
            EventBusPlayerController.OnFeedback -= HandleHapticInput;
            EventBusPlayerController.OnFeedback -= HandleCameraShake;
        }

        void PlayFeedbackDistanceBasedOnDistanceFromPlayer(Vector3 otherPosition, FeedbackType feedbackType)
        {
            if (!enabled) return;
            var distance = Vector3.Distance(transform.position, otherPosition);

            if (distance < distanceToPlayFeedback && feedbackType is FeedbackType.Heavy)
            {
                HandleHapticInput(FeedbackType.Heavy);
                HandleCameraShake(FeedbackType.Heavy);
            }
            else if (distance < distanceToPlayFeedback && feedbackType is FeedbackType.Medium)
            {
                HandleHapticInput(FeedbackType.Medium);
                HandleCameraShake(FeedbackType.Medium);
            }
            else if (distance < distanceToPlayFeedback && feedbackType is FeedbackType.Light)
            {
                HandleHapticInput(FeedbackType.Light);
                HandleCameraShake(FeedbackType.Light);
            }
            else if (distance > distanceToPlayFeedback && feedbackType is FeedbackType.Heavy)
            {
                HandleHapticInput(FeedbackType.Medium);
                HandleCameraShake(FeedbackType.Medium);
            }
            else if (distance > distanceToPlayFeedback && feedbackType is FeedbackType.Medium)
            {
                HandleHapticInput(FeedbackType.Light);
                HandleCameraShake(FeedbackType.Light);
            }
        }

        public void HandleHapticInput(FeedbackType feedbackType)
        {
            if (feedbackType is FeedbackType.Light)
                lightFeedback.PlayFeedbacks();
            else if (feedbackType is FeedbackType.Medium)
                mediumFeedback.PlayFeedbacks();
            else if (feedbackType is FeedbackType.Heavy)
                heavyFeedback.PlayFeedbacks();
        }

        public void HandleCameraShake(FeedbackType feedbackType)
        {
            if (feedbackType is FeedbackType.Light)
                lightCameraShake.PlayFeedbacks();
            else if (feedbackType is FeedbackType.Medium)
                mediumCameraShake.PlayFeedbacks();
            else if (feedbackType is FeedbackType.Heavy)
                heavyCameraShake.PlayFeedbacks();
        }


        public void PlayConstantFeedback(bool isRumbling)
        {
            if (isRumbling)
            {
                constantRumble.PlayFeedbacks();
            }
            else
                constantRumble.StopFeedbacks();
        }

        //Used for executions and future timeline events
        public void AnimationEventAnimationFeedback(int attackIntensity)
        {
            if (attackIntensity is 1)
                lightFeedback.PlayFeedbacks();
            else if (attackIntensity is 2)
                mediumFeedback.PlayFeedbacks();
            else if (attackIntensity is 3)
                heavyFeedback.PlayFeedbacks();
        }
    }
}