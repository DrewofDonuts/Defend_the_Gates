using UnityEngine.AI;

namespace Etheral
{
    public class EnemyRetreatState : EnemyBaseState
    {
        float retreatTime = 1.5f;
        const float RETREAT_DISTANCE = 2f;

        float retreatTimer = 0f;


        public EnemyRetreatState(EnemyStateMachine _stateMachine) : base(_stateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(StrafeState);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            animationHandler.SetFloatWithDampTime(ForwardSpeed, -5f, .1f, deltaTime);


            retreatTimer += deltaTime;
            if (retreatTimer >= retreatTime)
            {
                enemyStateBlocks.SwitchToBaseState();
                return;
            }


            var directionFromPlayer = GetDirectionAwayFromPlayer();

            var retreatTarget = stateMachine.transform.position +
                                directionFromPlayer.normalized * RETREAT_DISTANCE;

            NavMeshHit hit;

            RotateTowardsTargetSmooth(120f);
            stateMachine.GetAIComponents().GetNavMeshAgentController().SetRotation(false);

            if (NavMesh.SamplePosition(retreatTarget, out hit, RETREAT_DISTANCE, NavMesh.AllAreas))
            {
                Move(hit.position, stateMachine.AIAttributes.WalkSpeed, deltaTime);
            }
        }

        public override void Exit() { }
    }
}