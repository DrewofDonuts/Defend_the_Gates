using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class ControlCameraFollow : MonoBehaviour
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public Transform Target { get; private set; }
        [field: SerializeField] public float rotationSpeed { get; private set; } = 20f;


        public float returnToOriginalTime = 2.0f; // Time in seconds to return to the original rotation.
        float timeSinceLastInput = 0f;

        Quaternion originalRotation;
        Vector2 rotateInput;

        float rotateAmount;

        void Start()
        {
            originalRotation = transform.rotation;
        }

        void Update()
        {
            if (InputReader.RotateValue.magnitude > .05f)
            {
                RotateCamera();
                timeSinceLastInput = 0f;
            }

            else
            {
                timeSinceLastInput += Time.deltaTime;

                if (timeSinceLastInput > returnToOriginalTime)
                    ReturnToOriginalRotation();
            }

            RotateTowardsMovementDirection();
        }

        void RotateTowardsMovementDirection()
        {
            Vector3 movement = InputReader.MovementValue;
            if (movement.y > .05f || movement.y < -.05f)
                transform.rotation = transform.parent.rotation;
        }


        void RotateCamera()
        {
            if (CombatManager.Instance.IsPlayerAttacking || CombatManager.Instance.IsPlayerBlocking)
                return;

            rotateInput = InputReader.RotateValue;

            if (rotateInput.magnitude < .05f) return;
            float rotateAmount = rotateInput.x * rotationSpeed * Time.deltaTime;

            transform.RotateAround(Target.position, Vector3.up, rotateAmount);
        }


        private void ReturnToOriginalRotation()
        {
            // Lerp between the current rotation and the original rotation
            float t = timeSinceLastInput / returnToOriginalTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, t);
        }
    }
}