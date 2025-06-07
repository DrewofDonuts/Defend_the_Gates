using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class CharacterCollision : MonoBehaviour
    {
        [SerializeField] Collider characterCollider;
        [SerializeField] LayerMask layersToIgnore;
        [SerializeField] LayerMask alwaysIgnore;
        [SerializeField] CharacterController characterController;
        
        public LayerMask LayersToIgnore => layersToIgnore;

        void Start()
        {
            characterCollider.excludeLayers = alwaysIgnore;
            EventBusEnemyController.OnIgnorePlayerCollision += IgnorePlayerCollision;
        }

        void OnDisable() => EventBusEnemyController.OnIgnorePlayerCollision -= IgnorePlayerCollision;


        void OnCollisionEnter(Collision other)
        {
            Debug.Log($"Collision with: {other.gameObject.name}");

            if (other.gameObject.TryGetComponent(out CharacterController otherCC))
            {
                Debug.Log("Enemy Collision");

                // Get the movement direction
                Vector3 moveDirection = characterController.velocity.normalized;

                // Calculate the dot product between movement direction and collision normal
                float dotProduct = Vector3.Dot(moveDirection, other.contacts[0].normal);

                if (dotProduct < 0)
                {
                    // Stop movement in the collision direction
                    characterController.Move(Vector3.zero);
                }
            }
        }


        void IgnorePlayerCollision(bool obj, Affiliation affiliation)
        {
            if (affiliation == Affiliation) return;

            if (obj)
            {
                IgnoreCollision();
            }
            else
            {
                ConsiderCollision();
            }
        }

        void ConsiderCollision()
        {
            characterCollider.excludeLayers = alwaysIgnore;
        }

        void IgnoreCollision() => characterCollider.excludeLayers = layersToIgnore;


        // public void DisableCollision() => sphereCollider.enabled = false;
        // public void EnableCollision() => sphereCollider.enabled = true;
        public Affiliation Affiliation { get; set; }
    }
}