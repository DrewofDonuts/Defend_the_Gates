using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;


namespace Etheral
{
    [CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Etheral/Items/Ranged Weapon")]
    public class RangedWeaponItem : WeaponItem
    {
        [field: Header("Ranged Weapon")]
        [field: Header("Projectile Instantiation")]
        [field: PreviouslySerializedAs("Projectile")]
        [field: SerializeField] public ProjectileItem ProjectileData { get; private set; }
        [field: SerializeField] public Vector3 InstantiateOffset { get; private set; }

        [field: Header("Projectile Forces")]
        [field: SerializeField] public float ForwardSpeed { get; private set; } = 50f;
        [field: SerializeField] public float UpwardForce { get; private set; } = 20f;
        [field: SerializeField] public Vector3 EulerAngleVelocity { get; private set; }
        [field: SerializeField] public float CollisionAngle { get; private set; }

        [field: Header("Ranged Audio")]
        [field: SerializeField] public AudioImpact ProjectileImpact { get; private set; }
        [field: SerializeField] public AudioClip DrawAudio { get; private set; }
        [field: SerializeField] public AudioClip[] ReleasedAudio { get; private set; }
    }
}