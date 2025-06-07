using UnityEngine;

namespace Etheral.Combat
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] CharacterController _characterController;

        Collider[] allColliders;
        Rigidbody[] allRigidBodies;

        void Start()
        {
            allColliders = GetComponentsInChildren<Collider>(true);
            allRigidBodies = GetComponentsInChildren<Rigidbody>(true);
        
            ToggleRagdoll(false);
        }

        public void ToggleRagdoll(bool isRagdoll)
        {
            foreach (var collider in allColliders)
            {
                if (collider.gameObject.CompareTag("Ragdoll"))
                {
                    collider.enabled = isRagdoll;
                }
            }

            foreach (var rigidBody in allRigidBodies)
            {
                if (rigidBody.gameObject.CompareTag("Ragdoll"))
                {
                    rigidBody.isKinematic = false;
                    rigidBody.useGravity = true;
                }
            }

            //turns their animation and colliders off when in ragdoll
            _characterController.enabled = !isRagdoll;
            _animator.enabled = !isRagdoll;
        }
    
        public void LoadComponents()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }
    
    }
}
