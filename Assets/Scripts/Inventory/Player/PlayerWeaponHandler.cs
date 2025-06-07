using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class PlayerWeaponHandler : WeaponHandler
    {
        [field: SerializeField] public InputReader _inputReader { get; private set; }
        [SerializeField] PlayerVFXController playerVFXController;

        void Start()
        {
            // RightOneHandLogic.SetActive(false);
            // RightTwoHandLogic.SetActive(false);
            // LeftShieldLogic.SetActive(false);
            LoadCurrentWeaponDamage();


            _inputReader.WeaponSwitchEvent += LoadCurrentWeaponDamage;
            WeaponInventory.EquippedMeleeEvent += LoadCurrentWeaponDamage;
        }

        public override void LoadCurrentWeaponDamage()
        {
            if (WeaponInventory.RightEquippedWeapon != null)
            {
                _currentRightHandDamage = WeaponInventory.RightHandDamage;
                
                //where we will control future weapon VFX
            }

            if (WeaponInventory.LeftEquippedWeapon != null)
            {
                _currentLeftHandDamage = WeaponInventory.LeftHandDamage;
                
                //where we will control future weapon VFX
            }
        }

        //START HERE ADD VFX
        public void SetRightWeaponVFX(bool isActive)
        {
            playerVFXController.SetRightVFX(isActive);
        }

        public void SetLeftWeaponVFX(bool isActive)
        {
            playerVFXController.SetLeftVFX(isActive);
        }


#if UNITY_EDITOR

        [Button("Load Load Components")]
        public void LoadInputReader()
        {
            LoadComponents();
            Debug.Log("Get Component in Player Script");
            _inputReader = GetComponentInChildren<InputReader>();
        }

        [Button("Enable Right Weapon VFX")]
        public void LoadRightWeaponVFX()
        {
            playerVFXController.SetRightVFX(true);
        }

        [Button("Enable Right Weapon VFX")]
        public void LoadLeftWeaponVFX()
        {
            playerVFXController.SetLeftVFX(true);
        }

#endif
    }
}