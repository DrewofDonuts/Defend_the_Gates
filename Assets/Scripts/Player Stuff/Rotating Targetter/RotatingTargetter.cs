using UnityEngine;


namespace Etheral
{
    public class RotatingTargetter : MonoBehaviour
    {
        public InputObject inputObject;
        Vector2 movementValue;
        public float speed = 1f;


        void Start()
        {
            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            transform.rotation = transform.parent.rotation;
        }

        void Update()
        {
            var rotDirection = CalculateMovement();

            RotatePlayer(rotDirection);
        }

        void RotatePlayer(Vector3 movement)
        {
            if (movement.magnitude < .05f) return;

            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }


        protected Vector3 CalculateMovement()
        {
            movementValue = inputObject.MovementValue;

            if (Camera.main != null)
            {
                var forward = Camera.main.transform.forward;
                var right = Camera.main.transform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();

                return forward * movementValue.y +
                       right * movementValue.x;
            }

            return Vector3.zero;
        }
    }
}