using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class Resolver : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip activatingClip;


        void Start()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (animator == null) return;
            AnimatorOverrideController overrideController =
                new AnimatorOverrideController(animator.runtimeAnimatorController);

            overrideController["Resolve"] = activatingClip;
            animator.runtimeAnimatorController = overrideController;
        }


        [Button("Resolve")]
        public void Resolve()
        {
            animator.Play("Resolve");
        }
    }
}