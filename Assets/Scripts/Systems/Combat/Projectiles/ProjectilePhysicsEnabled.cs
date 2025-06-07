using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

//Character holds identical 

namespace Etheral
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectilePhysicsEnabled : Projectile
    {
        [SerializeField] Rigidbody rb;
        
       
        void OnEnable()
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.linearVelocity = Vector3.zero;
            rb.rotation = Quaternion.identity;
            rb.isKinematic = false;
        }

        void FixedUpdate()
        {
            if (!isCollided)
            {
                Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            else
            {
                // Rigidbody.rotation = Quaternion.Euler( CollisionAngle, 0, 0);
                // _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.identity);
            }

            Debug.Log($"RB Rotation: {rb.rotation}");
        }
        
        protected override void HandleHittingObjects(Collider other)
        {
            base.HandleHittingObjects(other);
            rb.isKinematic = true;
        }

        public void SetProjectileDamage(float damage, float knockBack, float knockDown, Transform attacker = default)
        {
            DamageData.damage = damage;
            DamageData.knockBackForce = knockBack;
            DamageData.knockDownForce = knockDown;
        }


       [ContextMenu("Load Components")]
        public void LoadComponents()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}