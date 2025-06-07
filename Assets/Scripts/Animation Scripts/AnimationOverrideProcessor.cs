using UnityEngine;

namespace Etheral
{
    public class AnimationOverrideProcessor
    {

        public AnimatorOverrideController ReturnAnimatorOverrideController(Animator animator)
        {
            return new AnimatorOverrideController(animator.runtimeAnimatorController);
        }
        
        public AnimationClipOverrides ReturnAnimationClipOverrides(AnimatorOverrideController animatorOverrideController)
        {
            return new AnimationClipOverrides(animatorOverrideController.overridesCount);
        }
        
    }
}