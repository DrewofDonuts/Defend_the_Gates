using System;
using UnityEngine;

namespace Etheral
{
    [Obsolete("Use CharacterAction instead")]
    public class Ability : ScriptableObject
    {
        [field: Header("Display")]
        [field: SerializeField] public string Name { get; private set; } = "New Ability";

        [field: Header("Ability Settings")]
        [field: SerializeField] public string Animation { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        [field: Header("Damage Stats")]
        [field: SerializeField] public int DirectDamage { get; private set; }
        [field: SerializeField] public int AreaDamage { get; private set; }
        [field: SerializeField] public int DotDamage { get; private set; }
        [field: SerializeField] public float DotDamageLength { get; private set; }

        [field: Header("Attack Forces")]
        [field: SerializeField] public float KnockBackForce { get; private set; }
        [field: SerializeField] public float KnockDownForce { get; private set; }

        [field: Header("Movement Forces")]
        [field: SerializeField] public float PreMovementForce { get; private set; }
        [field: SerializeField] public float PreForceTime { get; private set; }
        [field: SerializeField] public float MovementForce { get; private set; }
        [field: Tooltip("Time before Movement Force is applied")]
        [field: SerializeField] public float ForceTime { get; private set; }

        [field: Header("Timers")]
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float ChargeUpTime { get; private set; }
        [field: SerializeField] public float ActiveTime { get; private set; }
        [field: SerializeField] public float EndTime { get; private set; }

        [field: Header("Collision")]
        [field: SerializeField] public LayerMask AffectedLayer { get; private set; }
        [field: SerializeField] public string[] TagsToCheck { get; private set; }

        [field: Header("Sound")]
        [field: SerializeField] public AudioClip InitialSound { get; private set; }
        [field: SerializeField] public AudioClip ActiveSounds { get; private set; }
        [field: SerializeField] public AudioClip FinishSound { get; private set; }
    }
}