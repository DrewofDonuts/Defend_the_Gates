using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [InlineEditor]
    [CreateAssetMenu(menuName = "Etheral/Audio/Object SFX")]
    public class ObjectAudio : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] HitAudio { get; private set; }
        [field: SerializeField] public AudioClip[] DamagedAudio { get; private set; }
        [field: SerializeField] public AudioClip[] DestroyedAudio { get; private set; }
        
    }
}
