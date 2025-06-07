using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable]
    public class CharacterAction
    {
        [field: SerializeField] public string Name { get;  set; }

        // [field: FoldoutGroup("Action Configuration")]
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool Animation { get;  set; } = true;
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool IsAttack { get;  set; } = true;
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool IsAction { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool IsUseStamina { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool HasSpell { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool HasProjectile { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool HasEffect { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool Cooldown { get;  set; }
        [field: HorizontalGroup("Action Options")]
        [field: SerializeField] public bool HasAudio { get;  set; }

        // [field: HorizontalGroup("Action Configuration", MarginLeft = .1f, Title = "Action Configuration")]


        #region ANIMATION CONFIGURATION
        [field: FoldoutGroup("Animation")]
        [field: GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [field: ShowIf("Animation")]
        [field: SerializeField] public string PreAnimation { get;  set; }

        [field: FoldoutGroup("Animation")]
        [field: GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [field: ShowIf("Animation")]
        [field: SerializeField] public string AnimationName { get;  set; }

        [field: FoldoutGroup("Animation")]
        [field: GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        [field: ShowIf("Animation")]
        [Tooltip("Animation Dampening")]
        [field: Range(0.1f, .5f)]
        [field: SerializeField] public float TransitionDuration { get;  set; } = .1f;
        #endregion

        [field: FoldoutGroup("Stamina Configuration")]
        [field: ShowIf("IsUseStamina")]
        [field: SerializeField] public float StaminaCost { get;  set; }
        [field: FoldoutGroup("Stamina Configuration")]
        [field: ShowIf("IsUseStamina")]
        [field: SerializeField] public float TimeBeforeStamina { get;  set; }

        #region ACTION CONFIGURATION
        [field: FoldoutGroup("Action Configuration")]
        [field: ShowIf("Animation")]
        [Tooltip("How long before force will be applied")]
        [field: FoldoutGroup("Action Configuration")]
        [field: ShowIf("Animation")]
        [field: FoldoutGroup("Action Configuration")]
        [field: ShowIf("Animation")]
        [field: FoldoutGroup("Action Configuration")]
        [field: ShowIf("Cooldown")]
        [field: SerializeField] public float MaxCooldown { get;  set; }
        #endregion

        [field: FoldoutGroup("Action Configuration")]
        [field: Range(0, 1)]
        [field: SerializeField] public float[] TimesBeforeForce { get;  set; }
        [field: FoldoutGroup("Action Configuration")]
        [field: SerializeField] public float[] Forces { get;  set; }
        [field: FoldoutGroup("Action Configuration")]
        [field: ShowIf("IsAction")]
        [field: SerializeField] public ActionTimes[] Action { get;  set; }

        // [field: SerializeField] public float[] TimesBeforeActions { get;  set; }

        #region ARRAYED ACTION MELEE CONFIGURATION
        [field: FoldoutGroup("Action Configuration")]
        [field: Range(0, 1)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float[] EnableRightWeapon { get;  set; }
        [field: FoldoutGroup("Action Configuration")]
        [field: Range(0, 1)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float[] DisableRightWeapon { get;  set; }
        [field: FoldoutGroup("Action Configuration")]
        [field: Range(0, 1)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float[] EnableLeftWeapon { get;  set; }
        [field: FoldoutGroup("Action Configuration")]
        [field: Range(0, 1)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float[] DisableLeftWeapon { get;  set; }
        #endregion


        #region MELEE ATTACK STATS
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float Damage { get;  set; }
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float AdjacentDistance { get;  set; } = 1.25f;
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float ActionRange { get;  set; } = 1.25f;


        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float DotDamage { get;  set; }
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float KnockBackForce { get;  set; }
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float KnockDownForce { get;  set; }


        //Default to -1 so that the final attack won't combo into anything
        //Control if can combo to next Attack
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: FormerlySerializedAs("ComboStateIndex")]
        [field: SerializeField] public int NextComboStateIndex { get;  set; } = -1;

        //How far through an attack before it lets you go to the next one 
        [field: FoldoutGroup("Attack Stats", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public float ComboAttackTime { get;  set; }
        #endregion


        #region MELEE ATTACK ATTRIBUTES
        [field: FoldoutGroup("Attack Attributes", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public bool IsShieldBreak { get;  set; } = false;
        [field: FoldoutGroup("Attack Attributes", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public bool IsExecution { get;  set; }
        [field: FoldoutGroup("Attack Attributes", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public FeedbackType FeedbackType { get;  set; } = FeedbackType.Medium;
        [field: FoldoutGroup("Attack Attributes", Expanded = false)]
        [field: ShowIf("IsAttack")]
        [field: SerializeField] public AudioImpact AudioImpact { get;  set; } = AudioImpact.Blade;
        #endregion


        #region SPELL CONFIGURATION
        [field: ShowIf("HasSpell")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public float[] TimesBeforeSpells { get;  set; }


        // [field: ShowIf("HasSpell")]
        // [field: FoldoutGroup("Spells and Effects", expanded: false)]
        // [field: FormerlySerializedAs("Spell")]
        // [field: SerializeField] public SpellData SpellData { get;  set; } 

        [field: ShowIf("HasSpell")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public Spell Spell { get;  set; }


        [field: ShowIf("HasEffect")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public float TimeBeforeEffect { get;  set; }
        [field: ShowIf("HasEffect")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public GameObject Effect { get;  set; }

        [field: ShowIf("HasAudio")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public float TimeBeforeAudio { get;  set; }
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public List<float> TimesBeforeAudio { get;  set; } = new();
        [field: ShowIf("HasAudio")]
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public AudioClip Audio { get;  set; }
        [field: FoldoutGroup("Spells and Effects", expanded: false)]
        [field: SerializeField] public List<AudioClip> AudioClips { get;  set; } = new();
        #endregion


        #region RANGED ATTACK CONFIGURATION
        [field: ShowIf("HasProjectile")]
        [field: FoldoutGroup("Ranged Attack", expanded: false)]
        [field: SerializeField] public float[] TimesBeforeProjectile { get;  set; }

        [field: ShowIf("HasProjectile")]
        [field: FoldoutGroup("Ranged Attack", expanded: false)]
        [field: SerializeField] public float AimTime { get;  set; }

        [field: ShowIf("HasProjectile")]
        [field: FoldoutGroup("Ranged Attack", expanded: false)]
        [field: SerializeField] public float TimeBeforeDrawAudio { get;  set; }
        #endregion


        [field: ShowIf("Cooldown")]
        [field: SerializeField] public Cooldown CooldownPrefab { get;  set; }
        float currentCooldown;

        bool IsReady => !CooldownManager.Instance.actionsOnCooldownWithImage.Contains(this) &&
                        !CooldownManager.Instance.actionsOncooldownNoImage.Contains(this);

        public bool CheckIfReady()
        {
            if (CooldownManager.Instance == null)
                return true;

            return IsReady;
        }

        public void SetCurrentCooldown(float maxCooldown)
        {
            currentCooldown = maxCooldown;
        }

        public void DecrementCurrentCooldown(float cooldown)
        {
            currentCooldown -= cooldown;
        }

        public float GetCurrentCooldown() => currentCooldown;
        public void UpdateKnockbackForce(float _force) => KnockBackForce = _force;
        public void UpdateDamage(float _damage) => Damage = _damage;

        public void UpdateForces(float _force)
        {
            for (int i = 0; i < Forces.Length; i++)
            {
                Forces[i] = _force;
            }
        }

        public void UpdateAdjacentRange(float range)
        {
            AdjacentDistance = range;
        }
    }


    [Serializable]
    public class ActionTimes
    {
        public float TimeBeforeAction;
        public string ActionDescription;
    }
}