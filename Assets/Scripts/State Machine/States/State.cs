using System;
using UnityEngine;

namespace Etheral
{
    public abstract class State : IState
    {
        protected AnimationHandler animationHandler;
        protected ActionProcessor actionProcessor;

        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();

        protected const float CrossFadeDuration = 0.1f;
        protected const float AnimatorDampTime = 0.1f;

        protected CharacterAction characterAction;
        protected bool alreadyAppliedForce;
        
        StateMachine stateMachine;
        public StateType StateType { get; set; }


        protected static readonly int Locomotion = Animator.StringToHash("Locomotion");
        protected static readonly int Focus = Animator.StringToHash("Focusing");
        protected static readonly int FocusEnergyParameter = Animator.StringToHash("FocusEnergy");
        protected static readonly int Idle = Animator.StringToHash("Idle");

        
        // protected ITargetable currentTarget;
        
                
        // public void SetCurrentTarget(ITargetable target) =>
        //     currentTarget = target;
        


        protected float GetNormalizedTime(Animator animator, string tag)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

            //if we are transitioning to an attack, then we want to get data for the next state
            if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }

            //if we aren't in the middle of a transition, send the current state
            if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }

            return 0;
        }

    }
}