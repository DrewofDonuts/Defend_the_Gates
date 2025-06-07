using System;
using System.Collections.Generic;
using Etheral.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public abstract class WeaponInventory : MonoBehaviour
    {
        // [field: SerializeField] public StateMachine _stateMachine { get; private set; 

        [field: Header("Weapon Holders/Hands")]
        [field: SerializeField] public Transform LeftHand { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }
        [field: SerializeField] public StateMachine StateMachine { get; private set; }

        public WeaponDamage LeftHandDamage { get; protected set; }
        public WeaponDamage RightHandDamage { get; protected set; }

        [field: SerializeField] public MeleeWeaponItem LeftWeaponItem { get; protected set; }
        [field: SerializeField] public MeleeWeaponItem RightWeaponItem { get; protected set; }
        [field: SerializeField] public RangedWeaponItem RangedWeaponItem { get; private set; }

        //Needed for shared components between Player and NPC
        public MeleeWeaponItem LeftEquippedWeapon { get; protected set; }
        public MeleeWeaponItem RightEquippedWeapon { get; protected set; }
        public RangedWeaponItem RangedEquippedWeapon { get; protected set; }

        GameObject _rangedWeaponModel;
        public bool IsRangeEquipped { get; private set; }


        [Header("TESTING PUBLIC FIELD")]
        public WeaponRigidBody RightWeaponRB;

        public WeaponRigidBody LeftWeaponRB;


        // [field: SerializeField] public RangedWeaponItem RangedEquippedWeapon { get; protected set; }

        public event Action<GameObject> EquippedRangedEvent;
        public event Action EquippedMeleeEvent;

        bool _isLoadingEquipment = true;

        public bool IsLoadingEquipment
        {
            get => _isLoadingEquipment;
            protected set { _isLoadingEquipment = value; }
        }

        public void EquipRangedWeapon()
        {
            if (RangedWeaponItem != null)
            {
                DestroyOldWeapon();
                RangedEquippedWeapon = RangedWeaponItem;
                if (RangedEquippedWeapon.IsLeft)
                {
                    LeftWeaponRB.ResetRB();
                    _rangedWeaponModel = Instantiate(RangedEquippedWeapon.WeaponPrefab, LeftHand);
                }
                else
                {
                    RightWeaponRB.ResetRB();
                    _rangedWeaponModel = Instantiate(RangedEquippedWeapon.WeaponPrefab, RightHand);
                }

                EquippedRangedEvent?.Invoke(_rangedWeaponModel);
                IsRangeEquipped = true;

                RightWeaponRB.enabled = true;
            }
        }

        public void EquipLeftAndRightMelee()
        {
            DestroyOldWeapon();
            if (LeftWeaponItem != null)
                EquipLeftMelee(LeftWeaponItem);

            if (RightWeaponItem != null)
                EquipRightMelee(RightWeaponItem);
        }

        [ContextMenu("Equip Right Melee")]
        public void EquipRightMelee(MeleeWeaponItem meleeWeaponItem)
        {
            RightEquippedWeapon = meleeWeaponItem;
            RightHandDamage = Instantiate(RightEquippedWeapon.WeaponDamage, RightHand);
            RightHandDamage.SetCharacterComponents(StateMachine, StateMachine.CharacterAudio);
            EquippedMeleeEvent?.Invoke();
            RightWeaponRB.SetupWeapon(RightHandDamage);
            IsRangeEquipped = false;
        }

        [ContextMenu("Equip Left Melee")]
        public void EquipLeftMelee(MeleeWeaponItem meleeWeaponItem)
        {
            LeftEquippedWeapon = meleeWeaponItem;
            LeftHandDamage = Instantiate(LeftEquippedWeapon.WeaponDamage, LeftHand);
            LeftHandDamage.SetCharacterComponents(StateMachine, StateMachine.CharacterAudio);
            EquippedMeleeEvent?.Invoke();
            LeftWeaponRB.SetupWeapon(LeftHandDamage);
        }

        protected void DestroyOldWeapon()
        {
            if (LeftEquippedWeapon != null)
            {
                LeftWeaponRB.ResetRB();
                Destroy(LeftHandDamage.gameObject);
                LeftEquippedWeapon = null;
            }

            if (RightEquippedWeapon != null)
            {
                RightWeaponRB.ResetRB();
                Destroy(RightHandDamage.gameObject);
                RightEquippedWeapon = null;
            }

            if (RangedEquippedWeapon != null)
            {
                Destroy(_rangedWeaponModel);
                RangedEquippedWeapon = null;
                IsRangeEquipped = false;
            }
        }


#if UNITY_EDITOR

        [Button("Load Components")]
        public void LoadComponents()
        {
            List<Hands> Hands = new List<Hands>(GetComponentsInChildren<Hands>());
            foreach (var hand in Hands)
            {
                if (hand._leftHand)
                    LeftHand = hand.transform;
                if (!hand._leftHand)
                    RightHand = hand.transform;
            }

            List<WeaponRigidBody> WeaponRigidBodies =
                new List<WeaponRigidBody>(GetComponentsInChildren<WeaponRigidBody>());
            foreach (var weaponRigidBody in WeaponRigidBodies)
            {
                if (weaponRigidBody.IsLeft)
                    LeftWeaponRB = weaponRigidBody;
                if (!weaponRigidBody.IsLeft)
                    RightWeaponRB = weaponRigidBody;
            }

            StateMachine = GetComponent<StateMachine>();
        }
#endif
    }
}


//     // _rightHandWeaponModel.transform.localPosition = Vector3.zero;
//     // _rightHandWeaponModel.transform.localRotation = Quaternion.identity;