using TeleportFX;
using UnityEngine;

namespace Etheral
{
    public class EnemySpawnedState : EnemyBaseState
    {
        float timeBeforeSwitchingToIdle = 1.5f;

        public EnemySpawnedState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(IdleState);
            // aiComponents.GetNavMeshAgentController().enabled = false;
            // aiComponents.GetNavMeshAgentController().enabled = true;
            // aiComponents.GetNavMeshAgentController().ResetNavAgent();
            // aiComponents.GetTeleportationFX().TeleportationState =
            //     KriptoFX_Teleportation.TeleportationStateEnum.Appear;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(120);

            timeBeforeSwitchingToIdle -= deltaTime;

            if (timeBeforeSwitchingToIdle <= 0)
            {
                enemyStateBlocks.SwitchToStartingState(aiComponents);
            }
                
                
                // enemyStateBlocks.CheckLocomotionStates();
        }

        public override void Exit() { }
    }
}