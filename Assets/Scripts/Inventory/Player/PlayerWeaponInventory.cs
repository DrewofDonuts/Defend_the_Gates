using Sirenix.OdinInspector;
using UnityEngine;

//Can be used as a reference to know which weapon is in hand. Go into THAT attackstate
//SwordShield attack state. 2H Hammer Attack state. 

namespace Etheral
{
    public class PlayerWeaponInventory : WeaponInventory
    {
        [field: SerializeField] public PlayerStateMachine _stateMachine { get; private set; }

        [field: Header("Carried Weapons")]

        // [field: SerializeField] public MeleeWeaponItem RightOneHandWeapon { get; private set; }
        [field: SerializeField] public MeleeWeaponItem RightTwoHandWeapon { get; private set; }

        // [field: SerializeField] public MeleeWeaponItem LeftWeapon { get; private set; }
        [field: SerializeField] public MeleeWeaponItem Unarmed { get; private set; }

        // [field: SerializeField] public RangedWeaponItem RangedWeaponItem { get; private set; }

        protected void Start()
        {
            EquipSwordShieldEvent();

            // if (RightTwoHandWeapon != null)
            //     _stateMachine.InputReader.EquipHammerEvent += EquipTwoHandWeapon;
            //
            // if (RightOneHandWeapon != null && LeftWeapon != null)
            // {
            //     _stateMachine.InputReader.EquipSwordShieldEvent += EquipSwordShieldEvent;
            // }
        }

        //equipping methods moved from WeaponInventory to PlayerWeaponInventory
        protected void EquipSwordShieldEvent()
        {
            if (RightWeaponItem == null || LeftWeaponItem == null) return;
            DestroyOldWeapon();
            
            EquipRightMelee(RightWeaponItem);
            EquipLeftMelee(LeftWeaponItem);

            // EquipWeapons();
        }

        protected void EquipTwoHandWeapon()
        {
            if (RightTwoHandWeapon == null) return;

            DestroyOldWeapon();

            EquipLeftMelee(Unarmed);
            EquipRightMelee(RightTwoHandWeapon);

            // LeftEquippedWeapon = Unarmed;
            // RightEquippedWeapon = Right_2H_Weapon;

            // EquipWeapons();
        }

#if UNITY_EDITOR
        [Button("Load Player Weapon Inventory Components")]
        public void LoadPlayerWeaponComponents()
        {
            base.LoadComponents();
            _stateMachine = GetComponent<PlayerStateMachine>();
        }
#endif
    }
}