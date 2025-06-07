using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class WeepingMotherStartingState : EnemyBaseState
    {
        EnemyHealController enemyHealController;
        SpawnTrigger currentSpawnTrigger;
        bool hasStoodUp;

        bool isIdleLoop;
        bool isBeginExit;
        bool canSwitchState;

        protected static readonly int EnterIdle = Animator.StringToHash("Flex1");
        protected static readonly int ExitIdle = Animator.StringToHash("Flex2");


        public WeepingMotherStartingState(EnemyStateMachine _stateMachine, string _animationOverride = "") : base(
            _stateMachine)
        {
        }

        public override void Enter()
        {
            if (CheckPriorityAndTokenBeforeActions())
                if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                    return;

            enemyHealController = enemyStateMachine.GetComponentInChildren<EnemyHealController>();
            animationHandler.CrossFadeInFixedTime(EnterIdle);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (!enemyStateMachine.GetHostile()) return;


            var normalizedEnterTime = animationHandler.GetNormalizedTime("Flex1");
            var normalizedExitTime = animationHandler.GetNormalizedTime("Flex2");

            checkNextActionTimer += deltaTime;

            if (checkNextActionTimer > checkNextActionInterval)
            {
                if (IsInMeleeRange() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                {
                    isBeginExit = true;

                    if (canSwitchState)
                    {
                        Debug.Log("Can Heal Enemies: " + CanHealEnemies());

                        if (CanHealEnemies())
                        {
                            Debug.Log("Healing Enemies");
                            enemyStateBlocks.SwitchToHeal();
                            return;
                        }

                        if (CanTriggerSpawn() && IsInRangedRange())
                        {
                            Debug.Log("Should Trigger Spawn");
                            TriggerSpawn();
                            return;
                        }


                        // if (IsInRangedRange() && stateMachine.AIAttributes.SpecialAbility[0].CheckIfReady() &&
                        //     !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                        // {
                        //     enemyStateMachine.SwitchState(new WeepingMotherShriekState(enemyStateMachine));
                        //     return;
                        // }
                        //
                        if (IsInMeleeRange() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                        {
                            enemyStateBlocks.CheckAttacksFromLocomotionState();
                            return;
                        }

                        // if (!IsInMeleeRange() || !IsInRangedRange())
                        // {
                        //     enemyStateBlocks.SwitchToChase();
                        //     return;
                        // }
                    }
                }

                checkNextActionTimer = 0;
            }

            if (normalizedEnterTime >= 1 && !isIdleLoop)
            {
                animationHandler.CrossFadeInFixedTime(IdleState);
                isIdleLoop = true;
            }


            if (isBeginExit && normalizedExitTime == 0 && isIdleLoop)
            {
                animationHandler.CrossFadeInFixedTime(ExitIdle);
                isBeginExit = false;
            }

            if (normalizedExitTime >= 1 && !canSwitchState)
                canSwitchState = true;
        }

        bool CanHealEnemies()
        {
            return enemyHealController.CanHealEnemies() &&
                   enemyStateMachine.AIAttributes.SpecialAbility.Any(x => x.Name == "Heal" && x.CheckIfReady());
        }


        bool CanTriggerSpawn()
        {
            return enemyStateMachine.AIAttributes.SpecialAbility.Any(x => x.Name == "Scream" && x.CheckIfReady()) &&
                   !enemyStateMachine.AITestingControl.idleAndImpactOnly;
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