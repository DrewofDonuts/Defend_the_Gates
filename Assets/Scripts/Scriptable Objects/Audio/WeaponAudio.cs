using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [InlineEditor]
    [CreateAssetMenu(menuName = "Etheral/Audio/Weapon SFX")]
    public class WeaponAudio : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] Swooshes { get; private set; }
        [field: SerializeField] public AudioClip[] Blocked { get; private set; }
        [field: SerializeField] public AudioImpact AudioImpact { get; private set; }
    }
}