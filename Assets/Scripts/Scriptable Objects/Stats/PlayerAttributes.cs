using System.Collections.Generic;
using Etheral.CharacterActions;
using Etheral.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/Player", fileName = "New Player")]
    [InlineEditor]
    public class PlayerAttributes : CharacterAttributes
    {
        [field: FoldoutGroup("Movement")]
        [field: Tooltip("Damp the player turn towards movement direction in Free look")]
        [field: SerializeField] public float RotationDamping { get; private set; } = 10f;

        [field: SerializeField] public CharacterAction[] Actions { get; private set; }

        [field: FoldoutGroup("Dodge and Roll", expanded: false)]
        [field: SerializeField] public float DodgeDuration { get; private set; } = .5f;
        [field: FoldoutGroup("Dodge and Roll", expanded: false)]
        [field: SerializeField] public float DashDuration { get; private set; } = .15f;

        // [field: FoldoutGroup("Actions",expanded: false)]
        // [field: SerializeField] public float DodgeSpeed { get; private set; } = 10f;
        // [field: FoldoutGroup("Dodge and Roll",expanded: false)]
        // [field: SerializeField] public float DashSpeed { get; private set; } = 12f;

        [field: FoldoutGroup("Dodge and Roll", expanded: false)]
        [field: SerializeField] public CharacterAction OffensiveDodge { get; private set; }
        [field: FoldoutGroup("Dodge and Roll", expanded: false)]
        [field: SerializeField] public CharacterAction DefensiveDodge { get; private set; }
        [field: FoldoutGroup("Dodge and Roll", expanded: false)]
        [field: SerializeField] public CharacterAction GroundDodge { get; private set; }

        [field: FoldoutGroup("Basic Attacks", expanded: false)]
        [field: SerializeField] public CharacterAction[] Attacks { get; private set; }
        [field: FoldoutGroup("Basic Attacks", expanded: false)]
        [field: SerializeField] public CharacterAction[] RangedAttacks { get; private set; }
        [field: FoldoutGroup("Basic Attacks", expanded: false)]
        [field: SerializeField] public CharacterAction[] HammerAttacks { get; private set; }
        [field: FoldoutGroup("Basic Attacks", expanded: false)]
        [field: SerializeField] public CharacterAction[] SwordShieldExecutions { get; private set; }
        [field: FoldoutGroup("Basic Attacks", expanded: false)]
        [field: SerializeField] public CharacterAction[] CounterAttacks { get; private set; }


        [field: FoldoutGroup("Abilities", expanded: false)]
        [field: SerializeField] public List<CharacterActionObject> Abilities { get; private set; } = new();

        public CharacterAction GetAbility(string abilityName)
        {
            foreach (var ability in Abilities)
            {
                if (ability.CharacterAction.Name == abilityName)
                    return ability.CharacterAction;
            }

            Debug.LogWarning($"Ability {abilityName} not found in PlayerAttributes.");
            return null;
        }

        [field: FoldoutGroup("Abilities", expanded: false)]
        [field: FoldoutGroup("Abilities/Physical")]
        [field: SerializeField] public CharacterAction GroundExecution { get; private set; }
        [field: FoldoutGroup("Abilities/Physical")]
        [field: SerializeField] public CharacterAction SprintAttack { get; private set; }
        [field: FoldoutGroup("Abilities/Physical")]
        [field: SerializeField] public CharacterAction ShieldBash { get; private set; }

        [field: FoldoutGroup("Abilities/Physical")]
        [field: SerializeField] public CharacterAction HeightAttack { get; private set; }

        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction Leap { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction HolyCharge { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction BlessedGround { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction HolyShield { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction CleansingStrikes { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction Crusade { get; private set; }
        [field: FoldoutGroup("Abilities/Divine")]
        [field: SerializeField] public CharacterAction Purification { get; private set; }

#if UNITY_EDITOR

        [Button("Set All Forces", ButtonSizes.Medium)]
        public void SetAllBasicAttackForces(float _force)
        {
            foreach (var basicAttack in Attacks)
            {
                basicAttack.UpdateForces(_force);
            }
        }

        [Button("Set Basic Attack Knockback", ButtonSizes.Medium)]
        public void SetAllBasicAttackKnockBack(float _force)
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                Attacks[i].UpdateKnockbackForce(_force);
            }
        }

        [Button("Set Basic Attack Damage", ButtonSizes.Medium)]
        public void UpdateBasicAttackDamage(float damage)
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                Attacks[i].UpdateDamage(damage);
            }
        }

        [Button("Set Basic Attack Adjacent Range")]
        public void UpdateBasicAttackAdjacentRange(float range)
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                Attacks[i].UpdateAdjacentRange(range);
            }
        }

#endif
    }
}