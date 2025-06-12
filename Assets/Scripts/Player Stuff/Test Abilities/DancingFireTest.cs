using UnityEngine;

namespace Etheral
{
    public class DancingFireTest : MonoBehaviour
    {
        [SerializeField] CharacterController characterController;
        [SerializeField] float movementSpeed = 6f;
        [SerializeField] InputObject inputObject;
        [SerializeField] LayerMask ignoreLayers;
        [SerializeField] float timeToLive = 10f;

        void Start()
        {
            characterController.excludeLayers = ignoreLayers;
        }


        void Update()
        {
            if (timeToLive <= 0)
            {
                Destroy(gameObject);
                return;
            }

            timeToLive -= Time.deltaTime;

            Vector3 movement;
            movement.x = inputObject.MovementValue.x;
            movement.y = 0f; // No vertical movement
            movement.z = inputObject.MovementValue.y;

            if (movement.magnitude > 0.1f)
            {
                characterController.Move(movement.normalized * movementSpeed * Time.deltaTime);
            }
        }
    }
}