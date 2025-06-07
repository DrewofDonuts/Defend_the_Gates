using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] Animator animator;


        public void CompensateForTimeScale() =>
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        public void ResetAnimatorSpeed() =>
            animator.updateMode = AnimatorUpdateMode.Normal;


        public void CrossFadeInFixedTime(string animationName, float crossfadeDuration = 0.2f, float speed = 1)
        {
            animator.speed = speed;
            animator.CrossFadeInFixedTime(animationName, crossfadeDuration);
        }

        public void CrossFadeInFixedTime(int animationHash, float crossfadeDuration = 0.2f, float speed = 1)
        {
            animator.speed = speed;
            animator.CrossFadeInFixedTime(animationHash, crossfadeDuration);
        }

        public void CrossFadeInFixedTime(CharacterAction characterAction, float speed = 1)
        {
            animator.speed = speed;
            animator.CrossFadeInFixedTime(characterAction.AnimationName, characterAction.TransitionDuration);
        }


        public void SetRootMotion(bool value) => animator.applyRootMotion = value;
        public bool GetRootMotion() => animator.applyRootMotion;
        public void SetBool(int nameHash, bool value) => animator.SetBool(nameHash, value);
        public void SetBool(string parameterName, bool value) => animator.SetBool(parameterName, value);

        public void SetFloatWithDampTime(int nameHash, float value, float dampTime = .1f, float deltaTime = 0) =>
            animator.SetFloat(nameHash, value, dampTime, deltaTime);

        public void SetFloat(int nameHash, float value) => animator.SetFloat(nameHash, value);
        public void SetFloat(string parameterName, float value) => animator.SetFloat(parameterName, value);

        public void SetAnimatorLayer(int index, float weight)
        {
            animator.SetLayerWeight(index, weight);
        }

        public float GetAnimationLayerWeight(int index)
        {
            return animator.GetLayerWeight(index);
        }


        public float GetNormalizedTime(string tagName, int index = 0)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(index);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(index);

            if (animator.IsInTransition(index) && nextInfo.IsTag(tagName))
            {
                return nextInfo.normalizedTime;
            }

            if (!animator.IsInTransition(index) && currentInfo.IsTag(tagName))
            {
                return currentInfo.normalizedTime;
            }

            return 0;
        }


        public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex = 0) =>
            animator.GetCurrentAnimatorStateInfo(layerIndex);

        public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex = 0) =>
            animator.GetNextAnimatorStateInfo(layerIndex);

        public bool GetIfCurrentOrNextState(string animationName, int layerIndex = 0)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);
            var nextState = animator.GetNextAnimatorStateInfo(layerIndex);

            return currentState.IsName(animationName) || nextState.IsName(animationName);
        }

        public float GetFloat(int value) => animator.GetFloat(value);


        public void SetEnabled(bool value)
        {
            if (animator != null)
                animator.enabled = value;
        }
        
        
        // void OnAnimatorMove()
        // {
        //     transform.position += animator.deltaPosition;
        //     transform.rotation *= animator.deltaRotation;
        // }
    }
}