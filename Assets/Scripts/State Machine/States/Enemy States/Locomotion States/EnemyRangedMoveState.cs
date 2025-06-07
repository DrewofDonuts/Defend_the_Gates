using UnityEngine;

namespace Etheral
{
    public class EnemyRangedMoveState : EnemyBaseState
    {
        Vector3 destination;
        float timer;
        float timeBeforeCheckingRanges = 1.5f;

        public EnemyRangedMoveState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            destination = aiComponents.GetNavMeshAgentController().GetRandomPointInFront(2f);
            animationHandler.CrossFadeInFixedTime(Locomotion);
            enemyStateMachine.ResetAttackCount();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsAnything(destination, 120f);

            timer += deltaTime;

            Move(destination, enemyStateMachine.AIAttributes.RunSpeed, deltaTime);

            if (Vector3.Distance(enemyStateMachine.transform.position, destination) <= .25f ||
                timer >= timeBeforeCheckingRanges)
            {
                enemyStateBlocks.CheckLocomotionStates();
                return;
            }
        }

        public override void Exit() { }
    }
}