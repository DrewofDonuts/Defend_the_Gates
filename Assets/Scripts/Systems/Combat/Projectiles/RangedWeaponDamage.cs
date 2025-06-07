using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

//passes stats to the projectile from Character and RangedWeaponItem
//Acts as instantiation position for projectile

namespace Etheral
{
    public class RangedWeaponDamage : MonoBehaviour
    {
        [Title("Load Components either through WeaponHandler or LoadComponents method in Inspector")]
        [SerializeField] WeaponInventory weaponInventory;
        [SerializeField] CharacterAudio characterAudio;
        [SerializeField] GameObject aimingArrow;
        [SerializeField] bool isIgnoreAudioLimit;

        [FormerlySerializedAs("minAimOffset")]
        [MinValue(-.25f)]
        [SerializeField] float aimOffset;

        [SerializeField] public Affiliation team;
        GameObject throwingWeaponPrefab;
        public RangedWeaponItem loadedWeapon;

        float attackDamage;
        float attackKnockback;
        float attackKnockDown;


        void Awake()
        {
            if (weaponInventory == null)
            {
                Debug.LogError("Weapon Inventory not assigned to RangedWeaponDamage");
                Debug.LogError("Will get Weapon Inventory from parent");
                weaponInventory = GetComponentInParent<WeaponInventory>();
            }

            weaponInventory.EquippedRangedEvent += LoadRangedWeapon;

            if (aimingArrow != null)
                aimingArrow.SetActive(false);

            if (loadedWeapon != null)
                ObjectPoolManager.Instance.GetPool(loadedWeapon.ProjectileData.Projectile, 30);
        }

        void OnDisable()
        {
            weaponInventory.EquippedRangedEvent -= LoadRangedWeapon;
        }

        public void LoadRangedWeapon(GameObject equippedRangedWeaponModel)
        {
            loadedWeapon = weaponInventory.RangedEquippedWeapon;
            throwingWeaponPrefab = equippedRangedWeaponModel;
        }


        public void SetAttackStatDamage(float damage, float knockBack, float knockDown)
        {
            attackDamage = damage;
            attackKnockback = knockBack;
            attackKnockDown = knockDown;
        }

        //Called by WeaponHandler Animation Event method

        [Button("Instantiate Projectile")]
        public void InstantiateProjectile(float aimAccuracyModifier)
        {
            // LoadRangedWeapon();

            var audioType = isIgnoreAudioLimit ? AudioType.none : AudioType.rangedWeapon;

            characterAudio.PlayRandomOneShot(characterAudio.RangedDamageSource, loadedWeapon.ReleasedAudio,
                audioType);

            var projectile = Instantiate(loadedWeapon.ProjectileData.Projectile,
                transform.TransformPoint(loadedWeapon.InstantiateOffset),
                Quaternion.identity);
            
            SetProjectileStats(projectile, aimAccuracyModifier);
            SetProjectileForces(projectile);

            // var objectPooledProjectile =
                // ObjectPoolManager.Instance.GetObject(loadedWeapon.ProjectileData.Projectile, 20);

            // objectPooledProjectile.transform.position = transform.TransformPoint(loadedWeapon.InstantiateOffset);
            // objectPooledProjectile.transform.rotation = Quaternion.identity;


            // SetProjectileStats(objectPooledProjectile, aimAccuracyModifier);
            // SetProjectileForces(objectPooledProjectile);

            if (loadedWeapon.TypeOfWeapon is TypeOfWeapon.ThrowingWeapon)
                throwingWeaponPrefab.SetActive(false);
        }

        //Called by WeaponHandler Animation Event method
        public void ReloadProjectile()
        {
            throwingWeaponPrefab.SetActive(true);
        }

        public void ToggleAimingArrow(bool isActive)
        {
            aimingArrow.SetActive(isActive);
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

        //1 == 100% accuracy, 0 == 0% accuracy
        void SetProjectileStats(Projectile projectile, float aimAccuracyModifier = 1)
        {
            // Clamp aimAccuracyModifier to ensure it's in the valid range [0, 1]
            aimAccuracyModifier = Mathf.Clamp01(aimAccuracyModifier);


            // Reduce the aimOffset by the percentage defined by aimAccuracyModifier
            var modifiedAimOffset = aimOffset * (1 - aimAccuracyModifier);


            // Calculate the randomized offset within the adjusted range
            var offset = Random.Range(modifiedAimOffset * -1, modifiedAimOffset);


            // Apply stats to the projectile
            projectile.SetProjectileStats(attackDamage, attackKnockback, attackKnockDown,
                loadedWeapon.ProjectileData.LifeTime, loadedWeapon.ProjectileImpact, offset);

            // Assign the team to the projectile
            projectile.SetTeam(team);

            // Set projectile speed if physics is disabled
            if (!loadedWeapon.ProjectileData.IsPhysicsEnabled)
                projectile.SetProjectileSpeed(loadedWeapon.ForwardSpeed);
        }

#if UNITY_EDITOR
        public void HookComponents(WeaponInventory weaponInventory, Collider charCollider,
            CharacterAudio characterAudio)
        {
            this.weaponInventory = weaponInventory;
            this.characterAudio = characterAudio;
        }

        [Button(ButtonSizes.Medium), GUIColor(.25f, .50f, 0f)]
        public void LoadComponents()
        {
            weaponInventory = GetComponentInParent<WeaponInventory>();
            characterAudio = GetComponentInParent<CharacterAudio>();
        }

#endif
    }
}