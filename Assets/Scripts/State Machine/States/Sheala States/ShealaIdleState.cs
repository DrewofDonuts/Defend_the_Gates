using UnityEngine;

namespace Etheral
{
    public class ShealaIdleState : EnemyBaseState
    {
        protected ShealaController shealaController;


        public ShealaIdleState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            shealaController = stateMachine.GetAIComponents().GetCustomController<ShealaController>();

            animationHandler.CrossFadeInFixedTime(IdleState);
            shealaController.UpdateTimersByCurrentPhase();
            shealaController.StartProjectileTimer();
            shealaController.StartHumanBombTimer();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(120f);

            if (IsInRangedRange())
            {
                if (shealaController.projectileTimer >= shealaController.currentTimeBeforeProjectile && GetPlayerDistance() <= 10f)
                {
                    enemyStateBlocks.SwitchToSpecialAttack(0);
                    return;
                }

                if (shealaController.humanBombTimer >= shealaController.currentTImeBeforeHumanBomb && GetPlayerDistance() <= 10f)
                {
                    enemyStateBlocks.SwitchToSpecialAttack(1);
                    return;
                }
            }
        }

        public override void Exit() { }

        public int HandlePhase()
        {
            return default;
        }
    }
}