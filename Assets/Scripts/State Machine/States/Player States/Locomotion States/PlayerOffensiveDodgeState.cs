using UnityEngine;

namespace Etheral
{
    public class PlayerOffensiveDodgeState : PlayerBaseState
    {
        float remainingDodgeTime;

        static readonly int ForwardDodge = Animator.StringToHash("DodgeForward");
        static readonly int RightDodge = Animator.StringToHash("DodgeRight");
        static readonly int LeftDodge = Animator.StringToHash("DodgeLeft");
        static readonly int DashBlendTree = Animator.StringToHash("DashBlendTree");

        public PlayerOffensiveDodgeState(PlayerStateMachine stateMachine) :
            base(stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.OffensiveDodge;
            stateMachine.Animator.CrossFadeInFixedTime(ForwardDodge, CrossFadeDuration);
            stateMachine.Health.SetIsInvulnerable(true);

            remainingDodgeTime = stateMachine.PlayerCharacterAttributes.DodgeDuration;
            StartCooldown();

            EventBusPlayerStatesToDeprecate.PlayerSwitchedState(this, StateType.Dodge);

            RotateTowardsTargetSnap();
            
            stateMachine.SetCharacterControllerCollisionLayer(true);

        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();
            movement += stateMachine.transform.forward *
                        stateMachine.PlayerCharacterAttributes.OffensiveDodge.Forces[0];


            
            Move(movement, deltaTime);

            remainingDodgeTime -= deltaTime;

            if (remainingDodgeTime <= 0f)
            {
                ReturnToLocomotion(true, 20);
            }
        }


        public override void Exit()
        {
            stateMachine.SetCharacterControllerCollisionLayer(false);
            stateMachine.Health.SetIsInvulnerable(false);
        }
    }
}