using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Etheral
{
    public class Targetter : MonoBehaviour
    {
        [SerializeField] PlayerInputController playerInputController;
        [SerializeField] LayerMask groundMask;
        public float maxDistanceSpeed;
        public float maxDistanceFromPlayer;
        public float distanceAboveGround = 0.1f;
        public Transform Player;
        float _speed;
        public InputObject inputObject;

        Vector3 movement;
        bool canMove;


        void Start()
        {
            if (playerInputController == null)
                playerInputController =
                    EventBusPlayerController.PlayerStateMachine.PlayerComponents.GetPlayerInputController();
        }

        public void HandleSettings(Vector3 startPosition, float maxDistanceSpeed, float maxDistance, float speed)
        {
            transform.position = startPosition;
            this.maxDistanceSpeed = maxDistanceSpeed;
            maxDistanceFromPlayer = maxDistance;
            _speed = speed;
        }

        public void AdjustTargetterHeightToHitpoint(Vector3 hit)
        {
            //the location of the Targetter is set to the location of the hit point
            Vector3 newPosition = transform.position;
            newPosition.y = hit.y + distanceAboveGround;
            transform.position = newPosition;

            if (Mathf.Abs(hit.y - transform.position.y) > .3f)
            {
                newPosition.y = Mathf.MoveTowards(transform.position.y, hit.y, 2 * Time.deltaTime);
                transform.position = newPosition;
            }
        }

        public void SetCanMove(bool _canMove)
        {
            canMove = _canMove;
        }

        void Update()
        {
            if (canMove)
            {
                if (playerInputController.GetInputType() is InputType.gamePad)
                    movement = UpdateMovementWithGamepad();
                else if (playerInputController.GetInputType() is InputType.keyboard)
                    AimWithMouse();
            }

            // MoveTargetter(direction);
            // TargetterPosition = RaycastAgainstGround();
        }

        void LateUpdate()
        {
            if (playerInputController.GetInputType() is InputType.gamePad)
                MoveTargetter();
        }

        void MoveTargetter()
        {
            if (Player == null)
                Player = GameObject.FindWithTag("Player").transform;

            var speed = _speed;

            if (Time.timeScale < 1)
                speed /= Time.timeScale;

            Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
            Vector3 playerPosition = Player.transform.position;
            float distance = Vector3.Distance(newPosition, playerPosition);

            if (distance <= maxDistanceFromPlayer)
            {
                transform.position = newPosition;
            }

            if (distance >= maxDistanceFromPlayer)
            {
                maxDistanceSpeed /= Time.timeScale;

                float moveTowardsPlayerSpeed = Mathf.MoveTowards(0, maxDistanceSpeed, .75f);

                transform.position = Vector3.MoveTowards(transform.position, playerPosition,
                    moveTowardsPlayerSpeed * Time.deltaTime);
            }

            // if (distance >= 7)
            // {
            //     transform.position = Vector3.MoveTowards(transform.position, playerPosition, 2 * Time.deltaTime);
            // }
        }

        public Vector3 RaycastAgainstGround(out float distance)

        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                distance = hit.distance;
                return hit.point;
            }

            distance = 0;
            return default;
        }

        Vector3 UpdateMovementWithGamepad()
        {
            var forward = Camera.main.transform.forward;
            var right = Camera.main.transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            return forward * inputObject.MovementValue.y + right * inputObject.MovementValue.x;
        }

        void AimWithMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(inputObject.MousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                Vector3 targetPosition;
                var direction = (hitInfo.point - Player.position).normalized;
                var distance = Vector3.Distance(hitInfo.point, Player.position);
                
                if (distance <= maxDistanceFromPlayer)
                {
                    targetPosition = hitInfo.point;
                }
                else
                {
                    // Clamp to max distance while preserving direction
                    targetPosition = Player.position + direction * maxDistanceFromPlayer;
                }

                transform.position = new Vector3(targetPosition.x, transform.parent.position.y, targetPosition.z);
            }
        }
    }
}