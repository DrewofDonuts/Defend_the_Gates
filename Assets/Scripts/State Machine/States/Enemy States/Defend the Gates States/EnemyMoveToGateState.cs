using UnityEngine;

namespace Etheral
{
    public class EnemyMoveToGateState : EnemyBaseState
    {
        Gate currentGate;
        Vector3 gatePosition;
        float gateCheckTimer;
        float gateCheckInterval = 1.5f; // Interval to check for the gate

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public EnemyMoveToGateState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Locomotion);

            aiComponents.GetNavMeshAgentController().SetRotation(true);
            GetGate();
            stateMachine.SetStateType(StateType.MoveToGate);

            if (enemyStateMachine.stateIndicator != null && enemyStateMachine.AITestingControl.displayStateIndicator)
                enemyStateMachine.stateIndicator.color = Color.magenta;
        }

        public override void Tick(float deltaTime)
        {
            gateCheckTimer += deltaTime;
            if (gateCheckTimer > gateCheckInterval)
            {
                if (Vector3.Distance(stateMachine.transform.position, gatePosition) < 2f)
                {
                    // Switch to the next state when close enough to the gate
                    stateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine));

                    // enemyStateBlocks.CheckAttacksFromLocomotionState();
                    return;
                }

                gateCheckTimer = 0f;
            }

            Move(deltaTime);

            // if (currentGate == null)
            //     GetGate();

            if (stateMachine.AITestingControl.blockMovement) return;


            if (Vector3.Distance(stateMachine.transform.position, gatePosition) <
                stateMachine.AIAttributes.MeleeAttackRange)
                RotateTowardsGateSmooth(30f);
            Move(gatePosition, stateMachine.AIAttributes.WalkSpeed, deltaTime);


            CheckCombatWIthTimer(deltaTime);
        }

        void GetGate()
        {
            currentGate = aiComponents.GetAIGateHandler().GetClosestGate();


            currentGate.OnGateDestroyed += HandleGateDestroyed;
            gatePosition = currentGate.transform.position;
        }

        void HandleGateDestroyed(Gate destroyedGate)

        {
            currentGate.OnGateDestroyed -= HandleGateDestroyed;
            GetGate();
        }


        void RotateTowardsGateSmooth(float rotationSpeed)
        {
            if (aiComponents.GetNavMeshAgentController().IsUpdateRotation())
                aiComponents.GetNavMeshAgentController().SetRotation(false);

            var targetPosition = gatePosition;
            var direction = (targetPosition - stateMachine.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                rotationSpeed * Time.deltaTime);
        }

        public override void Exit()
        {
            aiComponents.GetNavMeshAgentController().SetRotation(false);

            if (currentGate != null && !currentGate.IsDestroyed)
                currentGate.OnGateDestroyed -= HandleGateDestroyed;
        }
    }
}