using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Etheral.CharacterActions;
using Etheral.Combat;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/AI/AI Character", fileName = "New Enemy")]
    [InlineEditor]
    public class AIAttributes : CharacterAttributes
    {
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [field: FoldoutGroup("AI")]
        [Tooltip("Must be greater than Strafe range")]
        [field: BoxGroup("AI/AI Stats", true)]
        [field: SerializeField] public EnemyType EnemyType { get; private set; } = EnemyType.Melee;

        [field: BoxGroup("AI/AI Stats")]
        [field: ShowIf("EnemyType", EnemyType.Melee)]
        [field: SerializeField] public bool CanStrafe { get; private set; }

        [field: BoxGroup("AI/AI Stats")]
        [field: ShowIf("EnemyType", EnemyType.Melee)]
        [field: SerializeField] public bool CanBlock { get; private set; }
        [field: BoxGroup("AI/AI Stats")]
        [field: SerializeField]  public bool CanBeExecuted { get; set; } = true;

        [field: BoxGroup("AI/AI Stats")]
        [field: SerializeField] public bool CanCounterAction { get; private set; }

        [field: BoxGroup("AI/AI Stats")]
        [field: SerializeField] public bool HasMeleeRangedAttack { get; private set; }
        [field: BoxGroup("AI/AI Stats")]
        [field: SerializeField] public bool HasGroundAttack { get; private set; }
        [field: BoxGroup("AI/AI Stats")]
        [field: SerializeField] public bool HasRetreat { get; private set; }

        [field: BoxGroup("AI/AI Stats", true)]
        [field: SerializeField] public float DetectionRange { get; private set; } = 10f;

        // [field: ShowIfGroup("AI Stats/IsMelee")]
        [field: BoxGroup("AI/AI Stats")]
        [field: ShowIf("isMelee")]
        [field: Range(1, 10)]
        [field: SerializeField] public float MeleeAttackRange { get; private set; } = 2f;

        [field: BoxGroup("AI/AI Stats")]
        [field: Range(0, 20)]
        [field: SerializeField] public float RangedAttackRange { get; private set; }

        [field: BoxGroup("AI/AI Stats")]
        [field: Range(0, 4)]
        [field: SerializeField] public float AdjacentAttackRange { get; private set; } = 1f;

        [field: BoxGroup("AI/AI Stats")]
        [field: Range(1, 10)]
        [field: SerializeField] public float StrafeRange { get; private set; } = 3f;


        [field: BoxGroup("AI/AI Stats/Strafe")]
        [field: ShowIfGroup("AI/AI Stats/Strafe/CanStrafe")]
        [field: Range(0, 100)]
        [field: SerializeField] public float AttackRateFromStrafe { get; private set; }


        [field: BoxGroup("AI/AI Stats/Strafe")]
        [field: Range(1, 6)]
        [field: ShowIfGroup("AI/AI Stats/Strafe/CanStrafe")]
        [field: SerializeField] public float MaxStrafeTime { get; private set; } = 1f;
        
        


        #region Attacks
        [field: FoldoutGroup("Attack Types")]
        [field: ShowIf("isMelee")]
        [field: SerializeField] public CharacterAction[] BasicAttacks { get; private set; }

        [field: FoldoutGroup("Attack Types")]
        [field: ShowIf("isRanged")]
        [field: SerializeField] public List<CharacterActionObject> RangedAttackObjects { get; private set; } = new();
        // [field: ShowIf("isRanged")]
        // [field: SerializeField] public CharacterAction[] RangedAttacks { get; private set; }

        [field: FoldoutGroup("Attack Types")]
        [field: ShowIf("HasMeleeRangedAttack")]
        [field: SerializeField] public CharacterAction[] MeleeRangedActions { get; private set; }


        [field: FoldoutGroup("Special Attack Types")]
        [field: ShowIf("isMelee")]
        [field: SerializeField] public CharacterAction[] HeavyAttack { get; private set; }


        [field: FoldoutGroup("Special Attack Types")]
        [field: SerializeField] public CharacterAction[] SpecialAbility { get; private set; }

        [field: FoldoutGroup("Special Attack Types")]
        [field: SerializeField] public CharacterAction GroundAttack { get; private set; }
        
        
        #endregion


        #region Counter Actions
        [field: BoxGroup("Counter Actions")]
        [field: ShowIf("CanCounterAction")]
        [field: SerializeField] public CharacterAction CounterCharacterAction { get; private set; }
        [field: BoxGroup("Counter Actions")]
        [field: ShowIf("CanCounterAction")]
        [field: SerializeField] public AICounterAttackSelector CounterAttackSelector { get; private set; }
        [field: BoxGroup("Counter Actions")]
        [field: ShowIf("CanCounterAction")]
        [field: SerializeField] public int HitsBeforeCounterAction { get; private set; } = 3;
        #endregion

        // [field: BoxGroup("Counter Action States")]
        // [field: ShowIf("CanEnterDefensiveCounter")]
        // [field: SerializeField] public StateSelectorHelper CounterAttackSelector { get; private set; }


        // [field: SerializeReference] public EnemyCounterActionState EnemyBlockCounterActionState { get; private set; }

        public bool isMelee => EnemyType is EnemyType.Melee or EnemyType.RangedMelee;
        public bool isRanged => EnemyType is EnemyType.Ranged or EnemyType.RangedMelee;

#if UNITY_EDITOR

        [FoldoutGroup("Functions")]
        [Button("Set All Forces", ButtonSizes.Medium)]
        public void SetAllBasicAttackForces(float _force)
        {
            foreach (var basicAttack in BasicAttacks)
            {
                basicAttack.UpdateForces(_force);
            }
        }
        [FoldoutGroup("Functions")]
        [Button("Set Basic Attack Knockback", ButtonSizes.Medium)]
        public void UpdateBasicAttack(float knockbackForce)
        {
            for (int i = 0; i < BasicAttacks.Length; i++)
            {
                BasicAttacks[i].UpdateKnockbackForce(knockbackForce);
            }
        }
        [FoldoutGroup("Functions")]
        [Button("Set Basic Attack Damage", ButtonSizes.Medium)]
        public void UpdateBasicAttackDamage(float damage)
        {
            for (int i = 0; i < BasicAttacks.Length; i++)
            {
                BasicAttacks[i].UpdateDamage(damage);
            }
        }
#endif
    }
}