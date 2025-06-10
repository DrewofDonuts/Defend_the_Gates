using System;
using UnityEngine;

namespace Etheral
{
    public class EventBusPlayerController
    {
        public static event EventHandler<CharacterStateEventArgsSUNSET> OnCharacterStateChange;

        public static PlayerStateMachine PlayerStateMachine { get; set; }

        public static event Action<bool> OnWalkEvent;

        // public static event Action OnDialogueEvent;
        public static event Action<PlayerControlTypes, float> OnPlayerStateChangeEvent;
        public static event Action OnTargetCameraEvent;
        public static event Action OnFarCameraEvent;
        public static event Action OnNearCameraEvent;
        public static event Action<Transform, float, float> OnAddTarget;
        public static event Action<Transform> OnRemoveTarget;
        public static event Action<Vector3, FeedbackType> OnFeedbackDistance;
        public static event Action<Vector3, DamageData> OnFeedbackDistanceDamageData;
        public static event Action<FeedbackType> OnFeedback;

        public static event Action<IDamage> OnInjurePlayerEvent;
        public static event Action<bool> OnGroundAttackingEvent;
        public static event Action<int> OnGetXP;

        public static void AddTarget(object sender, Transform target, float weight, float radius)
        {
            OnAddTarget?.Invoke(target, weight, radius);
        }

        public static void RemoveTarget(object sender, Transform target)
        {
            OnRemoveTarget?.Invoke(target);
        }

        public static void IsGroundAttacking(object sender, bool isGroundAttacking)
        {
            OnGroundAttackingEvent?.Invoke(isGroundAttacking);
        }

        public static void InjurePlayer(object sender, IDamage damage)
        {
            OnInjurePlayerEvent?.Invoke(damage);
        }

        public static void ChangePlayerState(object sender, PlayerControlTypes stateType, float angle = 0)
        {
            OnPlayerStateChangeEvent?.Invoke(stateType, angle);
        }

        public static void FarCamera(object sender)
        {
            OnFarCameraEvent?.Invoke();
        }

        public static void NearCamera(object sender)
        {
            OnNearCameraEvent?.Invoke();
        }

        public static void TargetCamera(object sender)
        {
            OnTargetCameraEvent?.Invoke();
        }

        public static void Walk(object sender, bool isWalking)
        {
            OnWalkEvent?.Invoke(isWalking);
        }

        // public static void Dialogue(object sender)
        // {
        //     OnDialogueEvent?.Invoke();
        // }

        public static void FeedbackBasedOnIDAndDistance(object feedbackName, Vector3 position,
            DamageData damageData)
        {
            OnFeedbackDistanceDamageData?.Invoke(position, damageData);
        }

        public static void FeedbackBasedOnDistanceFromPlayer(object feedbackName, Vector3 position,
            FeedbackType feedbackType)
        {
            OnFeedbackDistance?.Invoke(position, feedbackType);
        }

        public static void FeedbackIgnoringDistanceFromPlayer(object feedbackName, FeedbackType feedbackType)
        {
            OnFeedback?.Invoke(feedbackType);
        }


        //TODO: Figure out what to do with the StateChange methods below. This is redundant and maybe unnecessary
        public static void PublishCharacterStateChange(CharacterKey characterKey, StateType stateType)
        {
            OnCharacterStateChange?.Invoke(null, new CharacterStateEventArgsSUNSET(characterKey, stateType));
        }

        public static void SubscribeToCharacterStateChange(EventHandler<CharacterStateEventArgsSUNSET> handler)
        {
            OnCharacterStateChange += handler;
        }

        public static void UnsubscribeFromCharacterStateChange(EventHandler<CharacterStateEventArgsSUNSET> handler)
        {
            OnCharacterStateChange -= handler;
        }

        public static void PlayerGetsXP(object sender, int xp)
        {
            Debug.Log($" Sender {sender} giving. \n Player gets XP: " + xp);
            OnGetXP?.Invoke(xp);
        }
    }
}