using UnityEngine;

namespace Etheral.Combat
{
    public abstract class WeaponHolder : MonoBehaviour
    {
        public MeleeWeaponItem _equippedWeapon;
    
        int _addedDamage;

        public void SetDamage(int damage)
        {
            _addedDamage = damage;
        }

        void Update()
        {
        
        }
    }
}