using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    [Obsolete("No longer requires Weeping mother to move to a  dead body")]
    public class WeepingMotherMoveToDeadBodyState : EnemyBaseState
    {
        SpawnTrigger targetSpawnTrigger;
        EnemyControlledSpawner enemyControllerEnemySpawner;
        float modifier = 0f;
        float timer;

        public WeepingMotherMoveToDeadBodyState(EnemyStateMachine _stateMachine, SpawnTrigger spawnTrigger,
            EnemyControlledSpawner enemyControllerEnemySpawner) : base(
            _stateMachine)
        {
            targetSpawnTrigger = spawnTrigger;
            this.enemyControllerEnemySpawner = enemyControllerEnemySpawner;
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Locomotion, .4f);
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            enemyStateMachine.StartCoroutine(AlternateSpeed());
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            timer += deltaTime;

            // RotateTowardsTargetSmooth(120f);

            if (timer > .2f)
                RotateTowardsAnything(targetSpawnTrigger.transform, 60);

            Move(targetSpawnTrigger.transform.position, enemyStateMachine.AIAttributes.WalkSpeed + modifier, deltaTime);


            if (GetDistanceToAnything(targetSpawnTrigger.transform) < 1f)
            {
                Debug.Log("Should be at dead body");
                stateMachine.SwitchState(new WeepingMotherScreamState(enemyStateMachine));
                return;
            }
        }

        IEnumerator AlternateSpeed()
        {
            {
                modifier = modifier > 0 ? 0 : .50f;
                yield return new WaitForSeconds(.19f);
                modifier = modifier == 0 ? .50f : 0;
                yield return new WaitForSeconds(.25f);
            }
        }

        public override void Exit() { }
    }
}