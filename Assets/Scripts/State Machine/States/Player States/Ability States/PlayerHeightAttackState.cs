using UnityEngine;

namespace Etheral
{
    class PlayerHeightAttackState : PlayerBaseState
    {
        bool hasLanded;

        public PlayerHeightAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            momentum = stateMachine.GetCharComponents().GetCC().velocity;
            momentum.y = 0f;
            characterAction = stateMachine.PlayerCharacterAttributes.HeightAttack;
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            actionProcessor.PassRightBaseDamage(0);

            stateMachine.CharacterAudio.PlayRandomEmote(stateMachine.CharacterAudio.AudioLibrary.AttackEmote);
            stateMachine.Health.SetSturdy(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum, deltaTime);
            Debug.Log("HEIGHT ATTACK STATE");
            if (stateMachine.ForceReceiver.IsGrounded())
            {
                momentum = Vector3.zero;
                var normalizedTime = GetNormalizedTime(stateMachine.Animator, "AirAttackEnd");
                if (!hasLanded)
                    stateMachine.Animator.CrossFadeInFixedTime("AirAttackEnd", 0.1f);
                hasLanded = true;
                
                if (normalizedTime >= 1 && hasLanded)
                {
                    ReturnToLocomotion();
                }
            }
        }

        public override void Exit()
        {
        }
    }
}