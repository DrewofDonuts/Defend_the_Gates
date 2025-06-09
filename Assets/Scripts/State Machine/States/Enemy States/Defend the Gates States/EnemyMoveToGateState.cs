using UnityEngine;

namespace Etheral
{
    public class EnemyMoveToGateState : EnemyBaseState
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public EnemyMoveToGateState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Locomotion);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            
            if(stateMachine.AITestingControl.blockMovement) return;

            var gatePosition = aiComponents.GetAIGateHandler().GetClosestGate().position;

            // RotateTowardsTargetSmooth(30f);
            RotateTowardsGateSmooth(30f);
            Move(gatePosition, stateMachine.AIAttributes.WalkSpeed, deltaTime);

            if (Vector3.Distance(stateMachine.transform.position, gatePosition) < 2f)
            {
                // Switch to the next state when close enough to the gate
                stateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine));
                // enemyStateBlocks.CheckAttacksFromLocomotionState();
                return;
            }

            CheckHowAIShouldRespondToCombatRootMethod(deltaTime);
        }


        void RotateTowardsGateSmooth(float rotationSpeed)
        {
            var targetPosition = aiComponents.GetAIGateHandler().GetClosestGate().position;
            var direction = (targetPosition - stateMachine.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                rotationSpeed * Time.deltaTime);
        }

        public override void Exit() { }
    }
}