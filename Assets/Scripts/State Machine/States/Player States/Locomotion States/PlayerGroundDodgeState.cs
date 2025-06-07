using UnityEngine;

namespace Etheral
{
    public class PlayerGroundDodgeState : PlayerBaseState
    {
        float remainingDodgeTime;
        Vector3 direction;

        static readonly int ForwardDodge = Animator.StringToHash("GroundDodge");

        public PlayerGroundDodgeState(PlayerStateMachine stateMachine) :
            base(stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.GroundDodge;
            stateMachine.Animator.CrossFadeInFixedTime(ForwardDodge, CrossFadeDuration);
            stateMachine.Health.SetBlocking(true);

            remainingDodgeTime = stateMachine.PlayerCharacterAttributes.DodgeDuration;
            StartCooldown();

            EventBusPlayerStatesToDeprecate.PlayerSwitchedState(this, StateType.Dodge);

            RotateTowardsTargetSnap();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            var normalizedTime = animationHandler.GetNormalizedTime("GroundDodge");
            Vector3 movement = new Vector3();
            movement += stateMachine.transform.forward *
                        stateMachine.PlayerCharacterAttributes.GroundDodge.Forces[0];


            if (normalizedTime >= characterAction.TimesBeforeForce[0])
            {
                ReturnToLocomotion();
                return;
            }

            // if (normalizedTime < characterAction.TimesBeforeForce[0])
            Move(movement, deltaTime);

            remainingDodgeTime -= deltaTime;

            // if (remainingDodgeTime <= 0f)
            // {
            //     ReturnToLocomotion();
            // }
        }


        public override void Exit()
        {
            stateMachine.Health.SetBlocking(false);
        }
    }
}