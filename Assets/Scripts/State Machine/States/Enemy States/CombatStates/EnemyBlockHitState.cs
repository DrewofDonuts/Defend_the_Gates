using UnityEngine;

namespace Etheral
{
    public class EnemyBlockHitState : EnemyBaseState
    {
        readonly int BlockImpact = Animator.StringToHash("BlockImpact");

        public EnemyBlockHitState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
            // _enemyStateBlocks = new EnemyStateBlocks(stateMachine);
            enemyStateMachine.Health.SetBlocking(true);

            enemyStateMachine.Animator.Play("BlockImpact");

            enemyStateMachine.CharacterAudio.PlayRandomOneShot(enemyStateMachine.CharacterAudio.BlockSource,
                enemyStateMachine.CharacterAudio.AudioLibrary.BlockImpact, AudioType.block);
            
            
            Debug.Log($"Is Enemy State Blocks Nulls? {enemyStateBlocks == null}");
            Debug.Log($"Is AI Testing Control Nulls? {enemyStateMachine.AITestingControl == null}");

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, "BlockImpact");

            if (normalizedTime >= 1f)
            {
                if (enemyStateMachine.AIAttributes.CanCounterAction && !enemyStateMachine.AITestingControl.blockCounterAttack)
                {
                    if (enemyStateMachine.CheckIfCounterActionIsReady())
                    {
                        enemyStateMachine.AIAttributes.CounterAttackSelector.CounterStateSelector(enemyStateMachine);
                    }
                    else
                    {
                        enemyStateBlocks.CheckLocomotionStates();
                    }
                }
                else
                    enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit()
        {
            enemyStateMachine.Health.SetBlocking(false);
        }
    }
}