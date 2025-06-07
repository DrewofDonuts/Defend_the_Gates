using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Etheral
{
    public class TurrentShooter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] TurretLockOnController turretLockOnController;
        [SerializeField] float timeBetweenShots = 0.5f;
        [Tooltip("A value of 1 means perfect aim, 0 means no aim accuracy at all.")]
        [SerializeField] float aimAccuracy = 1f;
        [MinValue(-.25f)]
        [SerializeField] float defaultAimOffset = -.25f;

        [Header("Attack Stats")]
        [SerializeField] float attackDamage;
        [SerializeField] float attackKnockback;
        [SerializeField] float attackKnockDown;
        
        
        [Header("References")]
        [SerializeField] RangedWeaponItem loadedWeapon;
        [SerializeField] bool isIgnoreAudioLimit;
        [SerializeField] Transform projectileSpawnPoint;



        float timer;
        bool hasTarget;

        void Start()
        {
            turretLockOnController.OnTargetChanged += HandleTargetChanged;
        }

        void OnDisable()
        {
            turretLockOnController.OnTargetChanged -= HandleTargetChanged;
        }

        void HandleTargetChanged(Transform _target)
        {
            hasTarget = _target != null;
        }


        void Update()
        {
            if (!hasTarget)
                return;

            timer += Time.deltaTime;

            if (timer >= timeBetweenShots)
            {
                timer = 0f;
                InstantiateProjectile(aimAccuracy);
            }
        }


        public void InstantiateProjectile(float aimAccuracyModifier)
        {
            // LoadRangedWeapon();

            var audioType = isIgnoreAudioLimit ? AudioType.none : AudioType.rangedWeapon;


            var projectile = Instantiate(loadedWeapon.ProjectileData.Projectile,
                transform.TransformPoint(loadedWeapon.InstantiateOffset),
                Quaternion.identity);

            SetProjectileStats(projectile, aimAccuracyModifier);
            SetProjectileForces(projectile);
        }

        void SetProjectileForces(Projectile projectile)
        {
            projectile.transform.rotation = transform.rotation;
            projectile.SetEulerAndCollisionAngles(loadedWeapon.EulerAngleVelocity, loadedWeapon.CollisionAngle);
            var projectileRb = projectile.GetComponent<Rigidbody>();
            var forceToAdd = transform.forward * loadedWeapon.ForwardSpeed +
                             transform.up * loadedWeapon.UpwardForce;
            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }

        void SetProjectileStats(Projectile projectile, float aimAccuracyModifier = 1)
        {
            // Clamp aimAccuracyModifier to ensure it's in the valid range [0, 1]
            aimAccuracyModifier = Mathf.Clamp01(aimAccuracyModifier);


            // Reduce the aimOffset by the percentage defined by aimAccuracyModifier
            var modifiedAimOffset = defaultAimOffset * (1 - aimAccuracyModifier);


            // Calculate the randomized offset within the adjusted range
            var offset = Random.Range(modifiedAimOffset * -1, modifiedAimOffset);


            // Apply stats to the projectile
            projectile.SetProjectileStats(attackDamage, attackKnockback, attackKnockDown,
                loadedWeapon.ProjectileData.LifeTime, loadedWeapon.ProjectileImpact, offset);

            // Assign the team to the projectile
            projectile.SetTeam(Affiliation.Fellowship);

            // Set projectile speed if physics is disabled
            if (!loadedWeapon.ProjectileData.IsPhysicsEnabled)
                projectile.SetProjectileSpeed(loadedWeapon.ForwardSpeed);
        }
    }
}