
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Etheral/Items/Projectile")]
    public class ProjectileItem : Item
    {
        [field: SerializeField] public Projectile Projectile { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public bool IsPhysicsEnabled { get; private set; }

        void Awake()
        {
            itemType = ItemType.Weapon;
        }
    }
}