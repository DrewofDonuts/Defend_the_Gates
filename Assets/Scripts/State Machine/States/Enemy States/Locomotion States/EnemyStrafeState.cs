using System.Collections;
using UnityEngine;


namespace Etheral
{
    public class EnemyStrafeState : EnemyBaseState
    {
        float targetSpeed;

        // Transform transform;
        EnemyStrafeProcessor enemyStrafeProcessor;
        const float CrossFadeDuration = 0.3f;

        float strafeDistance;
        public float randomWaitStrafeTime;

        public float checkToAttackTime { get; } = 2f;

        #region Timers and Randomly Generated
        public int directionSelector { get; private set; }
        public int nextDirection { get; private set; }
        public int leftOrRight { get; private set; }

        public float maxStrafeTime;
        float chaseTimer;
        float _timeBeforeChase = .5f;
        static readonly int RightSpeed = Animator.StringToHash("RightSpeed");
        static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion


        public EnemyStrafeState(EnemyStateMachine stateMachine, int directionToMove = 2) : base(stateMachine)
        {
            directionSelector = directionToMove;
        }

        public override void Enter()
        {
            targetSpeed = enemyStateMachine.AIAttributes.StrafeSpeed;
            enemyStateBlocks = new EnemyStateBlocks(enemyStateMachine, this);
            enemyStrafeProcessor = new EnemyStrafeProcessor(enemyStateMachine, this);

            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();

            strafeDistance = enemyStateMachine.AIAttributes.MeleeAttackRange + 1f;
            maxStrafeTime = enemyStateMachine.AIAttributes.MaxStrafeTime;

            if (enemyStateMachine.AIAttributes.CanBlock)
                enemyStateMachine.Health.SetBlocking(true);

            randomWaitStrafeTime = enemyStrafeProcessor.SetRandomStrafeTime();

            if (enemyStateMachine.stateIndicator != null && enemyStateMachine.AITestingControl.displayStateIndicator)
                enemyStateMachine.stateIndicator.color = Color.yellow;


            animationHandler.CrossFadeInFixedTime("StrafeState", CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(enemyStateMachine.AIAttributes.RotateSpeed);
            enemyStrafeProcessor.Tick(deltaTime);

            StrafingLogic(deltaTime);
            OtherStateChecks(deltaTime);
        }


        public void SetDirection()
        {
            nextDirection = enemyStrafeProcessor.GetRandomDirection(1, 10);
            if (nextDirection != directionSelector)
            {
                enemyStateMachine.StartCoroutine(ChangeDirectionAfterDeceleration());
            }

            //to keep the odds of strafing left or right equal, 1 is left and 2 is right
            leftOrRight = enemyStrafeProcessor.GetRandomDirection(1, 3);
        }

        IEnumerator ChangeDirectionAfterDeceleration()
        {
            Decelerate(Time.deltaTime);
            yield return new WaitUntil(() => movementSpeed > 0.3f);

            directionSelector = nextDirection;
        }


        public void SwitchToAttack()

        {
            // _enemyStateBlocks.SwitchToMeleeAttack();
            enemyStateBlocks.CheckAttacksFromLocomotionState();
        }


        public void OtherStateChecks(float deltaTime)
        {
            //Idle
            if (!IsInStrafeRange())
            {
                enemyStateBlocks.SwitchToBaseState();
                return;
            }
        }

        void StrafingLogic(float deltaTime)
        {
            // && !IsAdjacentRange()

            if (IsInMeleeRange())
            {
                if (directionSelector >= 1 && directionSelector <= 8)
                    LeftRightStrafeMovement(deltaTime);
                else if (directionSelector is 9 or 10)
                    IdleStance(deltaTime);
            }

            //IF NOT ADJACENT
            if (IsInStrafeRange() && !IsInMeleeRange())
            {
                if (directionSelector >= 1 && directionSelector <= 2)
                    LeftRightStrafeMovement(deltaTime);
                else if (directionSelector >= 3)
                    ForwardMovement(deltaTime);
            }
        }

        #region Strafe Movement
        void LeftRightStrafeMovement(float deltaTime)
        {
            var leftOrRightDirection = leftOrRight == 1 ? 1 : -1;


            // Transform _target;
            Vector3 strafeDirection = Quaternion.Euler(0, 90, 0) *
                                      (GetCurrentTargetPosition() - enemyStateMachine.transform.position);
            
            var destination =  strafeDirection.normalized * (strafeDistance * leftOrRightDirection);

            if (enemyStateMachine.GetAIComponents().navMeshAgentController.GetIsOnNavMesh())
                enemyStateMachine.GetAIComponents().navMeshAgentController.SetDestination(destination);

            if (!enemyStateMachine.GetAIComponents().navMeshAgentController.GetIsOnNavMesh())
                Debug.Log("Not on NavMesh");
            
            Move(destination, targetSpeed, deltaTime);


            UpdateLeftOrRightAnimator(leftOrRightDirection, deltaTime);
        }

        void ForwardMovement(float deltaTime)
        {
            var destination = GetCurrentTargetPosition();
            enemyStateMachine.GetAIComponents().navMeshAgentController.SetDestination(destination);

            // Move(stateMachine.ComponentHandler.navMeshAgentController.GetDesiredVelocity().normalized *
            //      stateMachine.CharacterAttributes.StrafeSpeed, deltaTime);

            Move(destination, targetSpeed, deltaTime);

            UpdateForwardAnimator(1, deltaTime);
        }

        public void IdleStance(float deltaTime)
        {
            var destination = Vector3.zero;
            enemyStateMachine.GetAIComponents().navMeshAgentController.SetDestination(destination);

            Move(((AIStateMachine)enemyStateMachine).GetAIComponents().navMeshAgentController
                 .GetDesiredVelocityNormalized().normalized *
                 0, deltaTime);

            UpdateIdleAnimator(deltaTime);
        }

        void UpdateIdleAnimator(float deltaTime)
        {
            enemyStateMachine.Animator.SetFloat(RightSpeed, 0, .1f, deltaTime);
            enemyStateMachine.Animator.SetFloat(ForwardSpeed, 0, .1f, deltaTime);
        }

        void UpdateLeftOrRightAnimator(int right, float deltaTime)
        {
            enemyStateMachine.Animator.SetFloat(RightSpeed, right * movementSpeed, .1f, deltaTime);
            enemyStateMachine.Animator.SetFloat(ForwardSpeed, 0, .1f, deltaTime);
        }

        void UpdateForwardAnimator(int forward, float deltaTime)
        {
            enemyStateMachine.Animator.SetFloat(RightSpeed, 0, .1f, deltaTime);
            enemyStateMachine.Animator.SetFloat(ForwardSpeed, movementSpeed, .1f, deltaTime);
        }

        public override void Exit()
        {
            enemyStateMachine.StopCoroutine(ChangeDirectionAfterDeceleration());
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
            enemyStateMachine.Health.SetBlocking(false);

            // _strafeStateHelper.StopAllRoutines();
        }
        #endregion StrafeMovement
    }
}