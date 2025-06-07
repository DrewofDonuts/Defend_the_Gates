using Etheral.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

//Used to enable disable weapon colliders (located on the player) through Animation Events

namespace Etheral
{
    public abstract class WeaponHandler : MonoBehaviour
    {
        [field: Header("Components")]
        [SerializeField] StateMachine _stateMachine;

        [field: SerializeField] public WeaponInventory WeaponInventory { get; private set; }
        [field: SerializeField] public RangedWeaponDamage RangedWeaponDamage { get; private set; }

        // [field: SerializeField] public RangedWeaponDamage LeftRangedWeaponDamage { get; private set; }
        [Header("Shown for testing purposes")]
        
        [ReadOnly]
        public WeaponDamage _currentRightHandDamage;
        [ReadOnly]
        public WeaponDamage _currentLeftHandDamage;
        [ReadOnly]
        public WeaponRigidBody leftRBWeapon;
        [ReadOnly]
        public WeaponRigidBody rightRBWeaponr;


        public virtual void LoadCurrentWeaponDamage()
        {
            if (WeaponInventory.RightEquippedWeapon != null)
            {
                _currentRightHandDamage = WeaponInventory.RightHandDamage;
            }

            if (WeaponInventory.LeftEquippedWeapon != null)
            {
                _currentLeftHandDamage = WeaponInventory.LeftHandDamage;
            }
        }

        public void SetWeaponGO(bool isRight, bool isActive)
        {
            if (isRight)
                _currentRightHandDamage.gameObject.SetActive(isActive);
            else
                _currentLeftHandDamage.gameObject.SetActive(isActive);
        }

        public void DisableAllMeleeWeapons()
        {
            if (_currentLeftHandDamage != null)
            {
                DisableLeftWeapon();
            }

            if (_currentRightHandDamage != null)
            {
                DisableRightWeapon();
            }
        }

        #region Weapon Control
        public void EnableDualWeapons()
        {
            EnableRightWeapon();
            EnableLeftWeapon();
        }

        public void DisableDualWeapons()
        {
            DisableRightWeapon();
            DisableLeftWeapon();
        }
        
        public void EnableRightWeapon()
        {
            // _currentRightHandDamage.enabled = true;
            _currentRightHandDamage.EnableWeaponDamage();
            rightRBWeaponr.ToggleCurrentCollider(true);
        }

        public void EnableLeftWeapon()
        {
            // _currentLeftHandDamage.enabled = true;
            _currentLeftHandDamage.EnableWeaponDamage();
            leftRBWeapon.ToggleCurrentCollider(true);
        }

        public void DisableRightWeapon()
        {
            // _currentRightHandDamage.enabled = false;
            _currentRightHandDamage.DisableWeaponDamage();
            rightRBWeaponr.ToggleCurrentCollider(false);
        }


        public void DisableLeftWeapon()
        {
            // _currentLeftHandDamage.enabled = false;
            _currentLeftHandDamage.DisableWeaponDamage();
            leftRBWeapon.ToggleCurrentCollider(false);
        }

        public void ReleaseProjectile(float aimAccuracyModifier)
        {
            RangedWeaponDamage.InstantiateProjectile(1);
        }

        //
        // public void ReloadProjectile()
        // {
        //     RangedWeaponDamage.ReloadProjectile();
        // }
        #endregion

        [ContextMenu("Disable/Enable WeaponDamage ")]
        public void ToggleWeaponDamage()
        {
            if (_currentRightHandDamage.enabled)
                _currentLeftHandDamage.enabled = false;
            else
                _currentRightHandDamage.enabled = true;
        }

#if UNITY_EDITOR
        [Button("Load Components")]
        public void LoadComponents()
        {
            WeaponInventory = GetComponentInParent<WeaponInventory>();
            _stateMachine = GetComponentInParent<StateMachine>();

            RangedWeaponDamage = GetComponentInChildren<RangedWeaponDamage>();

            if (RangedWeaponDamage != null)
            {
                RangedWeaponDamage.HookComponents(WeaponInventory, _stateMachine.GetCharComponents().GetCC(),
                    _stateMachine.CharacterAudio);
            }

            WeaponRigidBody[] weapons = GetComponentsInChildren<WeaponRigidBody>();

            foreach (var weapon in weapons)
            {
                if (weapon.IsLeft)
                    leftRBWeapon = weapon;
                else
                    rightRBWeaponr = weapon;
            }
        }
#endif
    }
}