using UnityEngine;

namespace Etheral
{
    public class EnemyStartingClimbState : EnemyBaseState
    {
        Vector3 targetPosition;

        public EnemyStartingClimbState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            // aiComponents.GetNavMeshAgentController().DisableAgentComponent();

            stateMachine.GetCharComponents().GetCC().enabled = false;

            // animationHandler.SetRootMotion(true);
            animationHandler.CrossFadeInFixedTime("CrawlClimb", 0.1f);
            targetPosition = aiComponents.GetNavMeshAgentController().testDestination.position;
        }

        public override void Tick(float deltaTime)
        {
            // Move(deltaTime);
            var normalizedTime = animationHandler.GetNormalizedTime("CrawlClimb");

            aiComponents.GetNavMeshAgentController().SetDestination(targetPosition);

            if (normalizedTime >= 1)
            {
                enemyStateBlocks.CheckLocomotionStates();
                return;
            }
        }

        public override void Exit()
        {
            enemyStateMachine.GetCharComponents().GetCC().enabled = true;
            // aiComponents.GetNavMeshAgentController().EnableAgentUpdate();
            aiComponents.GetNavMeshAgentController().ResetNavAgent();
        }
    }
}