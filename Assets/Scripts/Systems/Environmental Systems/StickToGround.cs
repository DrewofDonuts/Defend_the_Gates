namespace Etheral.Environmental_Systems
{
    using UnityEngine;

    public class StickToGround : MonoBehaviour
    {
        [Tooltip("How far below the object to start checking for the ground.")]
        [SerializeField] public float raycastDistance = 2f;

        [Tooltip("Optional height offset above the ground.")]
        [SerializeField] public float groundOffset = 0.05f;

        [Tooltip("Layer used for the ground.")]
        [SerializeField] LayerMask groundLayerMask;

        void Start()
        {
            Stick();
        }

        void Stick()
        {
            // Cast ray downward from above the object
            Vector3 rayOrigin = transform.position + Vector3.up * raycastDistance * 0.5f;
            float totalDistance = raycastDistance;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, totalDistance, groundLayerMask))
            {
                // Move to the hit point with an optional offset
                Vector3 newPosition = hit.point + Vector3.up * groundOffset;
                transform.position = newPosition;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: No ground found within range.");
            }
        }

        // Optional: Automatically re-stick if scene reloads or position resets
        void OnEnable()
        {
            Stick();
        }
    }
}