using System.Collections;
using System.Collections.Generic;
using Etheral.Combat;
using UnityEngine;


namespace Etheral
{
    public class WeaponItem : Item
    {
        [field: Tooltip("IsRanged and IsLeft are used for instantiation")]
        [field: Header("Equipment Information")]
        [field: SerializeField] public bool IsRanged { get; private set; }
        [field: SerializeField] public bool IsLeft { get; private set; }
        [field: SerializeField] public GameObject WeaponPrefab { get; private set; }
        [field: SerializeField] public TypeOfWeapon TypeOfWeapon { get; private set; }

        void Awake()
        {
            itemType = ItemType.Weapon;
        }
    }
}