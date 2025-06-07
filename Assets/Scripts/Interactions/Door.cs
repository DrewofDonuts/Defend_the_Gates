using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class Door : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 90f; // Maximum rotation speed in degrees per second
        [SerializeField] float acceleration = 2f; // Acceleration factor
        [SerializeField] float deceleration = 2f; // Deceleration factor
        [SerializeField] float openAngle = 90f; // Target angle for the door to open

        float startingYRotation;
        bool isOpening;

        void Start()
        {
            startingYRotation = transform.localEulerAngles.y; // Store the initial Y rotation
        }


        [ContextMenu("Open Door")]
        public void OpenDoor()
        {
            if (!isOpening)
            {
                StopAllCoroutines(); // Ensure only one coroutine runs at a time
                StartCoroutine(RotateDoor(startingYRotation + openAngle));
            }
        }

        [ContextMenu("Close Door")]
        public void CloseDoor()
        {
            if (isOpening)
            {
                StopAllCoroutines(); // Ensure only one coroutine runs at a time
                StartCoroutine(RotateDoor(startingYRotation));
            }
        }

        IEnumerator RotateDoor(float targetAngle)
        {
            isOpening = !isOpening;

            float currentAngle = transform.localEulerAngles.y;
            float speed = 0f; // Initial speed is 0
            float totalRotation = Mathf.Abs(targetAngle - currentAngle);

            // Handle wrapping for angles
            if (targetAngle - currentAngle > 180f) targetAngle -= 360f;
            if (currentAngle - targetAngle > 180f) currentAngle -= 360f;

            while (Mathf.Abs(targetAngle - currentAngle) > 0.1f)
            {
                float remainingRotation = Mathf.Abs(targetAngle - currentAngle);

                // Accelerate or decelerate based on the remaining rotation
                if (remainingRotation > totalRotation * 0.5f)
                {
                    // Accelerate in the first half of the rotation
                    speed = Mathf.Min(speed + acceleration * Time.deltaTime, rotationSpeed);
                }
                else
                {
                    // Decelerate in the second half of the rotation
                    speed = Mathf.Max(speed - deceleration * Time.deltaTime, 10f); // Avoid speed going to 0
                }

                // Move towards the target angle
                currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, currentAngle,
                    transform.localEulerAngles.z);

                yield return null;
            }

            // Snap to the final rotation to prevent overshooting
            transform.localEulerAngles =
                new Vector3(transform.localEulerAngles.x, targetAngle, transform.localEulerAngles.z);
        }
    }
}