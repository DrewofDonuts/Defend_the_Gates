using UnityEngine;

namespace Etheral
{
    public class AIWeaponInventory : WeaponInventory
    {
        
        public void Start()
        {
            IsLoadingEquipment = true;
            EquipWeaponsOnStart();
            IsLoadingEquipment = false;
        }

        public void EquipWeaponsOnStart()
        {
            if (RangedWeaponItem != null)
            {
                RangedEquippedWeapon = RangedWeaponItem;
                EquipRanged();
            }
            else
            {
                if (LeftWeaponItem != null)
                {
                    EquipLeftMelee(LeftWeaponItem);
                }

                if (RightWeaponItem != null)
                {
                    EquipRightMelee(RightWeaponItem);
                }
            }

            //TODO: For ranged characters that hold a melee/shield in left 
            //if(RangedWeapon.HoldsOffHandWeapon) LeftEquippedWeapon - leftWeaponItem;

            // EquipWeapons();
        }

        [ContextMenu("Equip Ranged")]
        public void EquipRanged()
        {
            DestroyOldWeapon();
            if (RangedWeaponItem == null) return;

            // DestroyOldWeapon();
            EquipRangedWeapon();
        }

        [ContextMenu("Equip Melee")]
        public void EquipMelee()
        {
            // if (LeftEquippedWeapon == null && RightEquippedWeapon == null) return;
            DestroyOldWeapon();
            EquipRightMelee(RightWeaponItem);
            EquipLeftMelee(LeftWeaponItem);
        }
    }
}