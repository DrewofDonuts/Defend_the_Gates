using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class CharacterEventMover : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] NavMeshAgentController navMeshController;
        [SerializeField] CharacterController characterController;
        [SerializeField] CharacterID characterID;

        // [SerializeField] StateSelector stateSelector;
        [SerializeField] List<WaypointEtheral> waypoints = new();
        [SerializeField] String animatorParameter;

        [SerializeField] float speed;


        float currentSpeed;


        public delegate void OnChange(StateType newStateType);
        public event OnChange OnStateChange;


        // Transform transform;
        public Transform nextWaypoint;

        public int currentWaypointIndex = 0;
        public bool isEventMovement;
        public bool isIdle;


        public void InitializeWaypointMovement(float speed, EventKey eventKey)
        {
            SetWaypoints(eventKey);
            TriggerWaypointMovement();
            SetSpeed(speed);
        }

        public void SetWaypoints(EventKey eventKey)
        {
            WaypointsHandler _waypointsHandler = EventManager.Instance.GetWaypointGroup(eventKey);

            waypoints = _waypointsHandler.GetWaypointsList();
        }

        public void TriggerWaypointMovement()
        {
            isEventMovement = true;
            OnChangeState(StateType.Move);
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        void Update()
        {
            if (isEventMovement)
                HandleMovementAndAnimation();
        }

        void HandleMovementAndAnimation()
        {
            if (IsLastWaypointReached())
            {
                HandleLastWaypoint();
            }
            else
            {
                MoveTowardsNextWaypoint();
                WaklingAnimation();
            }
        }

        bool IsLastWaypointReached()
        {
            return currentWaypointIndex >= waypoints.Count;
        }

        void HandleLastWaypoint()
        {
            // StopMovement();
            RotateAtLastWaypoint();
        }

        void MoveTowardsNextWaypoint()
        {
            SetNextWaypoint();
            MoveCharacter();
            RotateTowardsNextWaypoint();
            CheckIfNextWaypointReached();
        }

        void SetNextWaypoint()
        {
            if (currentWaypointIndex < waypoints.Count)
                nextWaypoint = waypoints[currentWaypointIndex].transform;
        }

        void MoveCharacter()
        {
            navMeshController.SetDestination(nextWaypoint.position);

            // if (!navMeshController.GetIsUpdatePosition())
            //     navMeshController.EnableAgentUpdate();

            float targetSpeed = speed;
            currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, Time.deltaTime * 10);


            characterController.Move(navMeshController.GetDesiredVelocityNormalized() *
                                     (currentSpeed * Time.deltaTime));

            //keep the agent in sync
            // navMeshController.SetVelocityToKeepInSyncWithGameObject(characterController.velocity);
        }

        void RotateTowardsNextWaypoint()
        {
            RotateTowardsTarget(nextWaypoint.position);
        }

        void CheckIfNextWaypointReached()
        {
            if (Vector3.Distance(transform.position, nextWaypoint.position) < .2f)
            {
                currentWaypointIndex++;
            }
        }

        void RotateAtLastWaypoint()
        {
            Quaternion targetRotation = nextWaypoint.rotation;
            characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation,
                targetRotation, 1.5f * Time.deltaTime);

            if (!isIdle)
            {
                animator.CrossFadeInFixedTime("Idle", 0.2f);
                isIdle = true;
            }

            if (Quaternion.Angle(characterController.transform.rotation, targetRotation) < .01f)
            {
                Debug.Log("Rotation Complete");
                OnChangeState(StateType.Idle);
                isEventMovement = false;
            }
        }

        void RotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation,
                targetRotation, 2f * Time.deltaTime);
        }

        void WaklingAnimation()
        {
            animator.SetFloat(animatorParameter, speed);
        }

        public void OnChangeState(StateType newStateType)
        {
            // OnStateChange?.Invoke(newStateType);
            EventBusPlayerController.PublishCharacterStateChange(characterID.CharacterKey, newStateType);
        }
    }
}