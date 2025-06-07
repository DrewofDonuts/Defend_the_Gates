using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class WeepingMotherIdleState : EnemyBaseState
    {
        EnemyControlledSpawner enemyControllerEnemySpawner;
        EnemyHealController enemyHealController;
        SpawnTrigger currentSpawnTrigger;
        
        bool hasStoodUp;

        float screamTimer;

        public WeepingMotherIdleState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            if (CheckPriorityAndTokenBeforeActions())
                if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                    return;

            enemyControllerEnemySpawner = enemyStateMachine.GetComponentInChildren<EnemyControlledSpawner>();
            enemyHealController = enemyStateMachine.GetComponentInChildren<EnemyHealController>();
            animationHandler.CrossFadeInFixedTime(IdleState);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (!enemyStateMachine.GetHostile()) return;
            
            checkNextActionTimer += deltaTime;

            if (checkNextActionTimer > checkNextActionInterval)
            {
                if (IsInChaseRangeTarget() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                {

                    if (CanHealEnemies())
                    {
                        enemyStateBlocks.SwitchToHeal();
                        return;
                    }

                    if (CanTriggerSpawn() && IsInRangedRange())
                    {
                        TriggerSpawn();
                        return;
                    }
                    
                    if (IsInMeleeRange() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                    {
                        enemyStateBlocks.CheckAttacksFromLocomotionState();
                        return;
                    }
                }

                checkNextActionTimer = 0;
            }
        }

        bool CanHealEnemies()
        {
            return enemyHealController.CanHealEnemies() &&
                   enemyStateMachine.AIAttributes.SpecialAbility.Any(x => x.Name == "Heal" && x.CheckIfReady());
        }


        bool CanTriggerSpawn()
        {
            return enemyStateMachine.AIAttributes.SpecialAbility.Any(x => x.Name == "Scream" && x.CheckIfReady()) &&
                   !enemyStateMachine.AITestingControl.idleAndImpactOnly && IsOnScreen();
        }

        void TriggerSpawn()
        {
            // enemyStateMachine.GetTarget(closestSpawnTrigger.transform);

            enemyStateMachine.SwitchState(new WeepingMotherScreamState(enemyStateMachine));

            // enemyStateMachine.SwitchState(
            //     new WeepingMotherMoveToDeadBodyState(enemyStateMachine, currentSpawnTrigger, enemySpawner));
        }

        public override void Exit() { }
    }
}