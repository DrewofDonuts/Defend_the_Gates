using UnityEngine;

namespace Etheral
{
    public class PlayerImpactState : PlayerBaseState
    {
        float _duration = .25f;
        float timeBeforeCanDodge = 0.20f;
        readonly int Impact = Animator.StringToHash("Impact");

        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(Impact, CrossFadeDuration);

            stateMachine.WeaponHandler.DisableAllMeleeWeapons();
            
            stateMachine.InputReader.SouthButtonEvent += OnSouthButtonDown;
        }

        void OnSouthButtonDown()
        {
            if (timeBeforeCanDodge > 0) return;

            // playerBlocks.EnterDefensiveDodgeState(stateMachine.InputReader.MovementValue);
            playerBlocks.EnterOffensiveDodgeState();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            _duration -= deltaTime;
            timeBeforeCanDodge -= deltaTime;

            if (_duration <= 0f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.SouthButtonEvent -= OnSouthButtonDown;
        }
    }
}