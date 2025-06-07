using UnityEngine;

namespace Etheral
{
    public class PlayerDeadState : PlayerBaseState
    {
        static readonly int Dead = Animator.StringToHash("Dead");

        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(Dead, CrossFadeDuration);
            // stateMachine.GetCharComponents().GetCC().enabled = false;
            stateMachine.GetCharComponents().GetCC().detectCollisions = false;
            stateMachine.WeaponHandler.DisableAllMeleeWeapons();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}