using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(AudioSource))]
    public class WillItem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] WillType willType;
        [SerializeField] float willAmount = 10f;
        [SerializeField] float forwardForce = 2f;
        [SerializeField] float upwardForce = 2f;
        [SerializeField] float minHeight = .25f;
        [SerializeField] float maxHeight = 1f;
        [SerializeField] LayerMask layerMask;

        [Header("Move Towards Player Settings")]
        [SerializeField] float maxSpeed = 5.0f; // Maximum speed of the object
        [SerializeField] float accelerationDuration = 2.0f; // Time to reach max speed (ease-in)
        [SerializeField] float decelerationRange = 3.0f; // Range to start decelerating (ease-out)

        float currentSpeed = 0f;

        [Header("References")]
        [SerializeField] Rigidbody rb;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip audioClip;
        [SerializeField] SphereCollider sphereCollider;
        [SerializeField] ParticleFader particleFader;

        WillController willController; // The target position for the object to reach

        bool isFloating;
        [ReadOnly]
        public bool isMoveTowardsPlayer;

        public float amplitude = 0.5f;
        public float floatSpeed = 1f;

        public float WillAmount => willAmount;
        public WillType WillType => willType;

        Vector3 startPosition;

        void Start()
        {
            sphereCollider.enabled = false;
        }


        public void SpawnAfterMoving(Vector3 position)
        {
            transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            transform.position = position;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.up * upwardForce + transform.forward * forwardForce, ForceMode.Impulse);
        }

        void Update()
        {
            RaycastCheck();

            if (!isFloating)
                startPosition = transform.position;

            if (isFloating)
                Float();

            if (isFloating && !sphereCollider.enabled)
                sphereCollider.enabled = true;

            if (isMoveTowardsPlayer)
            {
                MoveTowardsPlayer();
            }
        }

        void StopParticleFaderFromFading()
        {
            particleFader.StopFading();
        }

        void MoveTowardsPlayer()
        {
            float distanceToTarget = Vector3.Distance(transform.position, willController.transform.position);

            // Accelerate if within accelerationDuration time
            if (distanceToTarget > decelerationRange)
            {
                float t = Mathf.Min(Time.time / accelerationDuration, 1.0f); // Normalize time for ease-in
                currentSpeed = Mathf.Lerp(0, maxSpeed, EaseOutCurve(t)); // Ease-out curve
            }
            else
            {
                // Start decelerating when within the deceleration range
                float t = Mathf.Clamp01(distanceToTarget / decelerationRange); // Normalize distance for ease-out
                currentSpeed = Mathf.Lerp(0, maxSpeed, EaseInCurve(t)); // Ease-in curve
            }

            // Move the object
            Vector3 direction = (willController.transform.position - transform.position).normalized;
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);

            // Stop movement if very close to target
            if (distanceToTarget <= .15f)
            {
                transform.position = willController.transform.position;
                willController.ReceiveWill(this);
                enabled = false; // Disable this script when target is reached
            }
        }

        void RaycastCheck()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 5, layerMask))
            {
                if (hit.distance <= minHeight)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    isFloating = true;
                }
            }
        }

        void Float()
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WillController _willController))
            {
                if (WillType == WillType.Defense)
                {
                    isMoveTowardsPlayer = _willController.PlayerHealth.CurrentDefense <
                                          _willController.PlayerHealth.MaxDefense;
                }
                else if (WillType == WillType.HolyCharge)
                    isMoveTowardsPlayer = _willController.PlayerHealth.CurrentHolyCharge <
                                          _willController.PlayerHealth.MaxHolyCharge;
                else if (WillType == WillType.Heals)
                    isMoveTowardsPlayer = EventBusPlayerController.PlayerStateMachine.PlayerComponents
                        .GetHealController().healsRemaining < EventBusPlayerController.PlayerStateMachine
                        .PlayerComponents.GetHealController().MaxHeals;


                isFloating = false;
                willController = _willController;
            }
        }

        // Custom ease-out curve: slower start, faster acceleration
        float EaseOutCurve(float t)
        {
            return t * t; // Quadratic ease-out curve
        }

        // Custom ease-in curve: faster deceleration towards the end
        float EaseInCurve(float t)
        {
            return Mathf.Pow(t, 0.5f); // Square-root ease-in curve
        }
    }
}