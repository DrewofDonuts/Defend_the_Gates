using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    public class EnemyCrossLinkJumpState : EnemyBaseState
    {
        bool shouldMove;
        OffMeshLinkData offMeshLinkData;
        public EnemyCrossLinkJumpState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Entering Cross Link Jump State");

            // animationHandler.CrossFadeInFixedTime("Jump");
            stateMachine.StateType = StateType.JumpOffMeshLink;
            offMeshLinkData =
                aiComponents.GetNavMeshAgentController().GetAgent().currentOffMeshLinkData;


            // aiComponents.GetNavMeshAgentController().SetDestination(offMeshLinkData.endPos);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            var normalizedValue = animationHandler.GetNormalizedTime("Jump");

            // if (normalizedValue >= .15f)
            //     shouldMove = true;
            //
            //
            // if (shouldMove && normalizedValue > .80f)
            //     shouldMove = false;

            aiComponents.GetNavMeshAgentController().SetDestination(GetCurrentTargetPosition());


            // if (shouldMove)
            Move(aiComponents.GetNavMeshAgentController().GetDesiredVelocityNormalized(),
                enemyStateMachine.AIAttributes.RunSpeed, deltaTime);


            if (normalizedValue >= 1f)
            {
                enemyStateBlocks.CheckLocomotionStates();
                return;
            }
        }

        public override void Exit()
        {
            aiComponents.GetCC().enabled = true;
            stateMachine.ForceReceiver.SetGravity(true);
            aiComponents.GetNavMeshAgentController().ResetNavAgent();
        }
    }
}