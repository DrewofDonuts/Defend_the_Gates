using UnityEngine;

namespace Etheral
{
    public class SlasherAttackingState : EnemyAttackingState
    {
        public int randomAttacksBeforeRetreat;

        public SlasherAttackingState(EnemyStateMachine _stateMachine, int _attackIndex) : base(_stateMachine)
        {
            attackIndex = _attackIndex;
        }

        public override void Enter()
        {
            base.Enter();
            randomAttacksBeforeRetreat = Random.Range(2, stateMachine.attacksBeforeRetreat + 1);
            if (stateMachine.GetCurrentAttackCount() > randomAttacksBeforeRetreat)
            {
                if (IsInMeleeRange())
                {
                    stateMachine.ResetAttackCount();
                    enemyStateBlocks.SwitchToJumpBack(false);
                }
                else
                {
                    stateMachine.ResetAttackCount();
                    enemyStateBlocks.CheckLocomotionStates();
                }
            }
        }


        public override void Exit()
        {
            base.Exit();

            if (stateMachine.GetCurrentAttackCount() > stateMachine.attacksBeforeRetreat)
                stateMachine.ResetAttackCount();
        }
    }
}