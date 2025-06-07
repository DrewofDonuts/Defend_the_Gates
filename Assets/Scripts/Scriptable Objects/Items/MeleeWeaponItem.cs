using Etheral.Combat;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Etheral
{
    [CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Etheral/Items/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        [field: Tooltip("Type of Weapon is to determine what animation to use in Attacking states")]
        [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

        [field: Header("Melee Audio")]
        [field: SerializeField] public WeaponAudio WeaponAudio { get; private set; }
        [field: SerializeField] public GameObject TrailRenderer { get; private set; }
        
        [Button(ButtonSizes.Large), GUIColor(.25f, .50f, 0)]
        public void LeadWeaponDataIntoWeaponDamage()
        {
            WeaponDamage = WeaponPrefab.GetComponentInChildren<WeaponDamage>();
            WeaponDamage.SetWeaponItem(this);
        }
    }
}