using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Etheral
{
    public abstract class AIBaseState : State
    {
        public float movementSpeed { get; protected set; }
        protected AIStateMachine stateMachine;
        public BaseAIStateOverrides overrides;
        protected PatrolController patrolController;
        

        protected bool hasBeenOnScreen;


        protected float checkNextActionInterval = 0.15f;
        protected float checkNextActionTimer = 0;

        protected static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
        protected static readonly int IdleState = Animator.StringToHash("Idle");
        protected static readonly int Impact = Animator.StringToHash("Impact");
        protected static readonly int StrafeState = Animator.StringToHash("StrafeState");


        public AIBaseState(AIStateMachine _stateMachine)
        {
            stateMachine = _stateMachine;
            patrolController = stateMachine.GetAIComponents().GetPatrolController();

            actionProcessor = new ActionProcessor();
            actionProcessor.ClearAll();
            animationHandler = _stateMachine.GetAIComponents().GetAnimationHandler();

            if (stateMachine.GetAIComponents().GetOverrideStateController().GetStateOverrides() != null)
                overrides = _stateMachine.GetAIComponents().GetOverrideStateController().GetStateOverrides();
        }

        protected ITargetable GetCurrentTarget() => stateMachine.GetCurrentTarget();
        
        protected Vector3 GetCurrentTargetPosition()
        {
            if (GetCurrentTarget() == null) return default;
            return GetCurrentTarget().Transform.position;
        }

        protected void Move(Vector3 desiredDirection, float deltaTime)
        {
            if (stateMachine.AITestingControl.blockMovement) return;

            // stateMachine.CharacterController.Move((motion + stateMachine.ForceReceiver.ForcesMovement) * deltaTime);

            stateMachine.GetCharComponents().GetCC()
                .Move((desiredDirection + stateMachine.ForceReceiver.ForcesMovement) * deltaTime);
        }

        //used for forces outside of gravity and movement
        protected void Move(float deltaTime)
        {
            if (stateMachine.GetAIComponents().GetCC().enabled)
                Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 destination, float _targetSpeed, float deltaTime)
        {
            if (stateMachine.GetAIComponents().GetNavMeshAgentController().GetIsOnNavMesh())
            {
                stateMachine.GetAIComponents().GetNavMeshAgentController().GetAgent().nextPosition =
                    stateMachine.transform.position;

                stateMachine.GetAIComponents().GetNavMeshAgentController().SetDestination(destination);

                float targetSpeed = _targetSpeed;
                movementSpeed = Mathf.Lerp(movementSpeed, targetSpeed,
                    stateMachine.AIAttributes.Acceleration * deltaTime);

                //.normalized will equal 1
                Move(stateMachine.GetAIComponents().navMeshAgentController.GetDesiredVelocityNormalized().normalized *
                     movementSpeed, deltaTime);
            }

            stateMachine.GetAIComponents().navMeshAgentController
                .SetVelocityToKeepInSyncWithCC(stateMachine.GetCharComponents().GetCC()
                    .velocity);
        }

        public void Decelerate(float deltaTime)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, 0, stateMachine.AIAttributes.Deceleration * deltaTime);
            Move(stateMachine.GetCharComponents().GetCC().velocity.normalized * movementSpeed,
                deltaTime);
        }

        public bool IsInChaseRangeTarget()
        {
            if (GetCurrentTarget() == null) return false;

            var targetDistanceSqr = (GetCurrentTargetPosition() - stateMachine.transform.position).sqrMagnitude;


            // if (targetDistanceSqr >= stateMachine.AIAttributes.DetectionRange * stateMachine.AIAttributes.DetectionRange) ;

            var detectionRange = stateMachine.GetAIComponents().GetStatHandler()
                .GetAIDetectionRange(stateMachine.AIAttributes);

            return targetDistanceSqr <= detectionRange * detectionRange;
        }
        
        protected float GetPlayerDistance()
        {
            return Vector3.Distance(stateMachine.transform.position, stateMachine.GetPlayer().transform.position);
        }

        protected float GetDistanceToAnything(Transform target) =>
            Vector3.Distance(stateMachine.transform.position, target.position);


        protected Vector3 GetDirectionToPlayer() =>
            stateMachine.GetPlayer().transform.position - stateMachine.transform.position;

        protected Vector3 GetDirectionAwayFromPlayer() =>
            stateMachine.transform.position - stateMachine.GetPlayer().transform.position;


        public bool IsInStrafeRange()
        {
            if (GetCurrentTarget() == null) return false;

            var targetDistanceSq =
                (GetCurrentTargetPosition() - stateMachine.transform.position).sqrMagnitude;
            return targetDistanceSq <=
                   stateMachine.AIAttributes.StrafeRange *
                   stateMachine.AIAttributes.StrafeRange;
        }

        public bool IsInMeleeRange()
        {
            if(GetCurrentTarget() == null) return false;
            if (!stateMachine.AIAttributes.isMelee) return false;

            var targetDistanceSqr =
                (GetCurrentTargetPosition() - stateMachine.transform.position).sqrMagnitude;

            return targetDistanceSqr <=
                   stateMachine.AIAttributes.MeleeAttackRange *
                   stateMachine.AIAttributes.MeleeAttackRange;
        }

        public bool IsInCustomRange(float range)
        {
            if(GetCurrentTarget() == null) return false;
            var targetDistanceSqr =
                (GetCurrentTargetPosition() - stateMachine.transform.position).sqrMagnitude;

            return targetDistanceSqr <= range * range;
        }

        // Checks if the enemy is a Ranged Unit and is in the Ranged Attack Range
        public bool IsInRangedRange()
        {
            if(GetCurrentTarget() == null) return false;

            //Commented out because it leads to issues for non-ranged AI who have a ranged attack
            // if (!stateMachine.AIAttributes.isRanged) return false;

            var targetDistanceSqr =
                (GetCurrentTargetPosition() - stateMachine.transform.position).sqrMagnitude;

            return targetDistanceSqr <=
                   stateMachine.AIAttributes.RangedAttackRange *
                   stateMachine.AIAttributes.RangedAttackRange;
        }

        public bool IsSpecialAttackReady(int specialAttackIndex)
        {
            return stateMachine.AIAttributes.SpecialAbility.Length > 0 &&
                   stateMachine.AIAttributes.SpecialAbility[specialAttackIndex].CheckIfReady() &&
                   IsInCustomRange(stateMachine.AIAttributes.SpecialAbility[specialAttackIndex].ActionRange);
        }

        //Checks if the enemy is capable of performing RangedMeleeAttack
        public bool IsDistanceMeleeAndIsReady()
        {
            return IsInRangedRange() && !IsAdjacentRange() && stateMachine.AIAttributes.HasMeleeRangedAttack &&
                   stateMachine.AIAttributes.MeleeRangedActions.Length > 0 &&
                   stateMachine.AIAttributes.MeleeRangedActions.All(action => action.CheckIfReady());
        }

        // Checks if the enemy is in the adjacent attack range
        public bool IsAdjacentRange()
        {
            if (GetCurrentTarget() == null) return false;

            var distance = Vector3.Distance(stateMachine.transform.position, GetCurrentTargetPosition());

            if (distance <= stateMachine.AIAttributes.AdjacentAttackRange)
                return true;

            return false;
        }


        //Check if the enemy has a token before performing an action, but should enable action if
        //TokenManager is not present
        public bool CheckPriorityAndTokenBeforeActions()
        {
            //if the enemy does not use a token, return true
            if (!stateMachine.UsesToken) return true;

            if (TokenManager.Instance != null)
            {
                //if the enemy already has a token, return true
                if (stateMachine.GetCurrentAttackToken() != null)
                    return true;

                return stateMachine.RequestToken();
            }

            return true;
        }

        protected bool IsOnScreen()
        {
            Vector3 screenPoint = stateMachine.GetAIComponents().GetCamera()
                .WorldToViewportPoint(stateMachine.transform.position);

            if (!hasBeenOnScreen)
                hasBeenOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                                  screenPoint.y < 1;

            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                   screenPoint.y < 1;
        }


        public bool CheckNavMeshSamplePosition(Vector3 position, float distance, out NavMeshHit hit)
        {
            return NavMesh.SamplePosition(position, out hit, distance, NavMesh.AllAreas);
        }


        #region Rotation Methods
        protected void RotateTowardsTargetSnap()
        {
            if (GetCurrentTarget() == null) return;
            if (stateMachine.AITestingControl.blockRotate) return;

            var lookPos = GetCurrentTargetPosition() - stateMachine.transform.position;
            lookPos.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        protected void RotateTowardsTargetSmooth(float rotationSpeed)
        {
            if (stateMachine.AITestingControl.blockRotate) return;
            if (stateMachine.GetLockedOnTarget() == null) return;


            var lookDirection = (GetCurrentTargetPosition() - stateMachine.transform.position).normalized;
            lookDirection.y = 0;
            var lookRotation = Quaternion.LookRotation(lookDirection);


            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                Time.deltaTime * rotationSpeed);
        }

        protected void RotateTowardsAnything(Transform target, float rotationSpeed)
        {
            if (stateMachine.AITestingControl.blockRotate) return;
            if (target == null) return;
            var lookDirection = (target.position - stateMachine.transform.position).normalized;
            lookDirection.y = 0;
            var lookRotation = Quaternion.LookRotation(lookDirection);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                Time.deltaTime * rotationSpeed);
        }

        protected void RotateTowardsAnything(Vector3 position, float rotationSpeed)
        {
            if (stateMachine.AITestingControl.blockRotate) return;

            var lookDirection = (position - stateMachine.transform.position).normalized;
            lookDirection.y = 0;
            var lookRotation = Quaternion.LookRotation(lookDirection);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                Time.deltaTime * rotationSpeed);
        }


        protected void RotateTowardsPlayer()
        {
            if (stateMachine.GetPlayer() == null) return;
            if (stateMachine.AITestingControl.blockRotate) return;

            var lookDirection = stateMachine.GetPlayer().transform.position - stateMachine.transform.position;
            lookDirection.y = 0f;

            var lookRotation = Quaternion.LookRotation(lookDirection);
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, lookRotation,
                Time.deltaTime * stateMachine.AIAttributes.RotateSpeed);

            // stateMachine.transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        #endregion
    }
}