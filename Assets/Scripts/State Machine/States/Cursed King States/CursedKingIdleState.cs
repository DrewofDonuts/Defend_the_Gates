using UnityEngine;

namespace Etheral.Cursed_King
{
    public class CursedKingIdleState : EnemyBaseState
    {
        CursedKingController cursedKingController;
        PhaseInfoCursedKing phaseInfoCursedKing;

        public CursedKingIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            cursedKingController = aiComponents.GetCustomController<CursedKingController>();
            phaseInfoCursedKing = cursedKingController.GetPhaseInfo();

            cursedKingController.StartSkeletonTimer();
            cursedKingController.StartWraithTimer();

            // if (phaseInfoCursedKing.phase is 1 or 2)
            //     enemyStateMachine.Health.SetSturdy(true);
            // else
            //     enemyStateMachine.Health.SetSturdy(false);
            
            enemyStateMachine.Health.SetSturdy(true);

            stateMachine.Health.OnTakeHit += OnTakeHit;

            var chanceToTaunt = Random.Range(0, 2);
            
            if (chanceToTaunt == 1 && IsOnScreen())
                PlayEmote(enemyStateMachine.CharacterAudio.AudioLibrary.TauntEmote, AudioType.taunt);
        }

        void OnTakeHit(IDamage obj)
        {
            if (phaseInfoCursedKing.phase is 1 or 2)
                enemyStateMachine.Health.SetSturdy(true);
            else if (enemyStateMachine.Health.CurrentDefense <= 0)
                enemyStateMachine.Health.SetSturdy(false);
        }


        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (phaseInfoCursedKing == null)
            {
                phaseInfoCursedKing = cursedKingController.GetPhaseInfo();
                return;
            }

            if (IsInChaseRangeTarget() && enemyStateMachine.GetHostile())
                RotateTowardsTargetSmooth(60f);

            if (IsInChaseRangeTarget())
            {
                Debug.Log($"Is special Attack ready: {IsSpecialAttackReady(0)}");
                Debug.Log($"Is in Ranged Range: {IsInRangedRange()}");

                if (IsInRangedRange() && IsSpecialAttackReady(0))
                {
                    enemyStateBlocks.SwitchToSpecialAttack(0);
                    return;
                }

                if (cursedKingController.wraithTimer >= phaseInfoCursedKing.maxCallWraithTime && IsInRangedRange() &&
                    IsOnScreen())
                {
                    enemyStateMachine.SwitchState(new CursedKingSummonWraithState(enemyStateMachine));
                    return;
                }

                if (cursedKingController.skeletonTimer >= phaseInfoCursedKing.maxRaiseSkeletonTime &&
                    IsInRangedRange() && IsOnScreen())
                {
                    enemyStateBlocks.SwitchToSpawnEnemyState(phaseInfoCursedKing.spawnNumber);
                    cursedKingController.ResetSkeletonTimer();
                    return;
                }

                if (IsInMeleeRange())
                {
                    enemyStateBlocks.CheckAttacksFromLocomotionState();
                }

                if (IsInChaseRangeTarget() && !IsInMeleeRange())
                {
                    enemyStateBlocks.SwitchToChase();
                }
            }
        }

        public override void Exit()
        {
        }
    }
}