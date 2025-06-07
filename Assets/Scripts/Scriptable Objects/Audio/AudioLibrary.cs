using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Audio/Character SFX")]
    [InlineEditor]
    public class AudioLibrary : ScriptableObject
    {
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] CarpetSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] GrassSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] GravelSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] HardSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] LeavesSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] MetalSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] SandSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] SnowSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] WaterSurfaceWalking { get; private set; }
        [field: FoldoutGroup("WALKING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] WoodSurfaceWalking { get; private set; }

        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] CarpetSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] GrassSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] GravelSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] HardSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] LeavesSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] MetalSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] SandSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] SnowSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] WaterSurfaceRunning { get; private set; }
        [field: FoldoutGroup("RUNNING FOOTSTEPS")]
        [field: SerializeField] public AudioClip[] WoodSurfaceRunning { get; private set; }

        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] AttackEmote { get; private set; }
        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] HitEmote { get; private set; }
        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] DeathEmote { get; private set; }
        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] EffortEmote { get; private set; }
        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] TauntEmote { get; private set; }
        [field: FoldoutGroup("EMOTES")]
        [field: SerializeField] public AudioClip[] FallingDeath { get; private set; }

        [field: Header("TAKE HIT SOUNDS")]
        [field: SerializeField] public AudioClip[] ArrowDamage { get; private set; }
        [field: SerializeField] public AudioClip[] BladeDamage { get; private set; }
        [field: SerializeField] public AudioClip[] BluntDamage { get; private set; }
        [field: SerializeField] public AudioClip[] BlockImpact { get; private set; }
        [field: SerializeField] public AudioClip[] BreakShield { get; private set; }
        [field: SerializeField] public AudioClip[] DeathBlow { get; private set; }


        [field: Header("EQUIPMENT SOUND")]
        [field: SerializeField] public AudioClip[] DodgeEquipment { get; private set; }
        [field: SerializeField] public AudioClip[] MoveEquipment { get; private set; }
        
    }
}