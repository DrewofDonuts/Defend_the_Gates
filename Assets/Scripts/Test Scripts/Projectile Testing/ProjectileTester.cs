using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class ProjectileTester : MonoBehaviour
    {
        [FormerlySerializedAs("projectile")] public ProjectilePhysicsEnabled _projectilePhysicsEnabled;
        [FormerlySerializedAs("nonRBProjectile")] public Projectile _projectile;
        public Transform lookAtObject;

        public float force;

        void Update()
        {
            transform.LookAt(lookAtObject);
        }

        [Button("Instantiate NonRBProjectile")]
        public void InstantiateNonRBProjectile()
        {
            var projectile = Instantiate(_projectile, transform.position, transform.rotation);
        }


        [Button("Instantiate Projectile")]
        public void InstantiateProjectile()
        {
            // LoadRangedWeapon();


            var projectile = Instantiate(this._projectilePhysicsEnabled,
                transform.position,
                Quaternion.identity);

            SetProjectileForces(projectile);
        }


        void SetProjectileForces(ProjectilePhysicsEnabled projectilePhysicsEnabled)
        {
            projectilePhysicsEnabled.transform.rotation = transform.rotation;
            projectilePhysicsEnabled.SetProjectileDamage(1, 0, 0);
            var projectileRb = projectilePhysicsEnabled.GetComponent<Rigidbody>();
            var forceToAdd = transform.forward * force;
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }
    }
}