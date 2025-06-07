using UnityEngine;

namespace Etheral
{
    public class EnemyShieldBreakState : EnemyBaseState
    {
        readonly int ShieldBreak = Animator.StringToHash("ShieldBreak");
        EnemyStateBlocks _enemyStateBlocks;


        public EnemyShieldBreakState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            // _enemyStateBlocks = new EnemyStateBlocks(base.stateMachine);
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime(ShieldBreak, CrossFadeDuration);
            enemyStateMachine.Health.SetIsInvulnerable(true);

            enemyStateMachine.CharacterAudio.PlayRandomOneShot(enemyStateMachine.CharacterAudio.BlockSource,
                enemyStateMachine.CharacterAudio.AudioLibrary.BreakShield, AudioType.block);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, "ShieldBreak");

            if (normalizedTime >= 1f)
            {
                _enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit()
        {
            enemyStateMachine.Health.SetIsInvulnerable(false);
        }
    }
}