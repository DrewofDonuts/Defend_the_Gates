using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    [Obsolete("This class is obsolete, use HolyShieldTargetter instead.")]
    public class HolyShieldTargetter : MonoBehaviour, LeapInputs.ITargeterActions
    {
        [field: SerializeField] public Transform CenterObject { get; private set; }


        LeapInputs _leapInputs;
        Vector2 _movementValue;
        [field: SerializeField] public Quaternion InitialRotation { get; private set; }

        public float _speed = 1f;
        public float radius = 5f;
        public float currentAngle = 0f;

        //
        void Start()
        {
            _leapInputs = new LeapInputs();
            _leapInputs.Targeter.SetCallbacks(this);
            _leapInputs.Targeter.Enable();
            // gameObject.SetActive(false);
        }

        void OnEnable()
        {
            _leapInputs = new LeapInputs();
            _leapInputs.Targeter.SetCallbacks(this);
            _leapInputs.Targeter.Enable();
        }

        void OnDisable()
        {
            _leapInputs.Disable();
            _leapInputs.Targeter.Disable();
        }

        void Update()
        {
            MoveShield();
            RotateTowardsCenter();
        }

        void RotateTowardsCenter()
        {
            Vector3 targetPostition =
                new Vector3(CenterObject.position.x, transform.position.y, CenterObject.position.z);
            transform.LookAt(targetPostition);
        }

        void MoveShield()
        {
            var speed = _speed;

            if (Time.timeScale < 1)
                speed /= Time.timeScale;

            currentAngle += speed * -_movementValue.x * Time.deltaTime;

            float x = CenterObject.position.x + radius * Mathf.Cos(currentAngle);
            float z = CenterObject.position.z + radius * Mathf.Sin(currentAngle);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);

            transform.position = newPosition;
        }

        public void HandleSettings(Vector3 startPosition, float speed)
        {
            transform.position = startPosition;
            transform.Rotate(startPosition);

            _speed = speed;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            _movementValue = context.ReadValue<Vector2>();
        }
    }
}