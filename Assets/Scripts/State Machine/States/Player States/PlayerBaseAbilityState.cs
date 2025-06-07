using UnityEngine;

namespace Etheral
{
    public abstract class PlayerBaseAbilityState : PlayerBaseState
    {
        protected Ability ability;
        protected bool alreadyAppliedPreForce;
        protected bool alreadyAppliedForce;
        protected float previousFrameTime;
        protected float forceTime;

        public PlayerBaseAbilityState(PlayerStateMachine stateMachine, Ability ability) : base(stateMachine)
        {
            this.ability = ability;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ability.Animation, CrossFadeDuration);

            forceTime = ability.ForceTime;
        }

        public override void Tick(float deltaTime)
        {
            RotateTowardsTarget();

            Move(deltaTime);
            forceTime -= Mathf.Max(deltaTime, 0);

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, ability.Animation);

            if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {
                AbilitySecondaryForce(ability.PreMovementForce);

                if (forceTime <= 0)
                {
                    stateMachine.Health.SetSturdy(true);
                    AbilityForce(ability.MovementForce);
                }
            }
            else
            {
                stateMachine.Health.SetSturdy(false);
                ReturnToLocomotion();
            }
        }
        
        


        protected void AbilitySecondaryForce(float force)
        {
            if (alreadyAppliedPreForce) return;
            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
            alreadyAppliedPreForce = true;
        }

        protected void AbilityForce(float force)
        {
            if (alreadyAppliedForce) return;
            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
            alreadyAppliedForce = true;
        }

        public override void Exit()
        {

        }


        //Obsolete - for 3rd person only
        // protected void RotateCameraOnEnter()
        // {
        //     _stateMachine.FreeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
        //     _stateMachine.FreeLookCamera.m_XAxis.m_MaxSpeed = 0;
        // }

        // protected void ReturnCameraSettingsToFreeLook()
        // {
        //     _stateMachine.FreeLookCamera.m_XAxis.m_MaxSpeed = 200f;
        //     _stateMachine.FreeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
        // }
    }
}